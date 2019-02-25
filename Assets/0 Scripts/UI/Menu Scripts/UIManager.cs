using HoloToolkit.Unity.Buttons;
using System.Collections.Generic;
using UnityEngine;
using Infrastructure.Grid.Entities;
using System;
using Infrastructure.Grid.Entities.Buildings;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public static UIManager Instance;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public enum MenuState
    {
        Off,
        MainMenu,
        BuildMenu,
        BuildingSelect,
    }

    public Action<int> BuildingsGenerated = delegate { };
    public Action<bool> StateChanged = delegate { };
    public MenuState menuState = MenuState.MainMenu;
    float iconSize = 0.0391f;


    [Header("Main Menu")]
    public SessionCreator sessionCreator;
    public GameObject mainMenu;
    public CompoundButton[] mainMenuButtons;

    [Header("Build Menu")]
    public GameObject buildMenu;
    public CompoundButton[] buildMenuButtons;

    [Header("Building Menu")]
    public GameObject buildingMenu;
    public Transform buildingMenuCanvas;
    public GameObject buildingBtnPrefab;
    public CompoundButton[] textButtons;

    [Header("Text Materials")]
    public Material On;
    public Material Off;

    private Animator animator;
    private List<BuildingMap> Buildings;
    private GameObject[] Menus;

    private WorldGridTile targetTile;

    public void Start()
    {
        animator = GetComponent<Animator>();

        Menus = new GameObject[] { buildingMenu, mainMenu, buildMenu };
        
        foreach(CompoundButton menuButton in buildMenuButtons)
        {
            menuButton.OnButtonClicked += BuildMenuButtonPressed;
        }

        foreach (CompoundButton menuButton in mainMenuButtons)
        {
            menuButton.OnButtonClicked += MainMenuButtonPressed;
        }

        foreach (CompoundButton textButton in textButtons)
        {
            textButton.OnButtonClicked += TextButtonPressed;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SwitchState(MenuState.Off);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchState(MenuState.BuildMenu);
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            SwitchState(MenuState.BuildingSelect);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SwitchState(MenuState.MainMenu);
        }
    }


    #region Main Menu
    void MainMenuButtonPressed(GameObject go)
    {
        string Name = "";

        foreach (CompoundButton menuButton in mainMenuButtons)
        {
            if (menuButton == go.GetComponent<CompoundButton>())
            {
                menuButton.ButtonState = ButtonStateEnum.Disabled;
                Name = menuButton.name.ToLower();

              
                break;
            }
        }

        switch (Name)
        {
            case "new":
                sessionCreator.CreateDefaultGrid();
                break; 
            case "quit":
                Application.Quit();
                break;
            default:
                break;
        }
        
        SwitchState(MenuState.Off);

    }

    #endregion

    #region Build Menu

    void BuildMenuButtonPressed(GameObject go)
    {
        string Name = "";

        foreach(CompoundButton menuButton in buildMenuButtons)
        {
            if(menuButton == go.GetComponent<CompoundButton>())
            {
                Name = menuButton.name.ToLower();
                break;
            }
        }

        switch (Name)
        {
            case "build":
                SwitchState(MenuState.BuildingSelect);
                break;
            case "destroy":
                DestroyTile(go);
                SwitchState(MenuState.Off);
                break;
            case "road":
                SpawnRoad(go);
                SwitchState(MenuState.Off);
                break;
            default:
                SwitchState(MenuState.Off);
                break;
        }
    }
    void SpawnRoad(GameObject go)
    {
        Vector2Int pos = targetTile.Position;
        targetTile.ParentGrid.GridSystem.AddTileEntityToTile(pos, new Road());
    }
    void DestroyTile(GameObject go)
    {
        Game.CurrentSession.City.GetGrid(targetTile.ParentGrid.Id).DestroyTileEntity(targetTile.Position);
    }

    #endregion

    #region Building Menu
    void BuildingPressed(GameObject go)
    {
        if (!targetTile) return;

        int gridID =  0;
        gridID = targetTile.ParentGrid.Id;
        Vector2Int pos = targetTile.Position;

        TileEntity tileEnt = Activator.CreateInstance(Buildings[go.GetComponent<BuildingButton>().index].BuildingType) as TileEntity;

        if (targetTile.Model)
        {
            targetTile.UpdateModel(tileEnt.GetModel());
        }
        else
        {
           Game.CurrentSession.City.GetGrid(gridID).AddTileEntityToTile(pos.x, pos.y, tileEnt);
        }
        //Take money
        Game.CurrentSession.TakeFunds(Buildings[go.GetComponent<BuildingButton>().index].Cost);
        SwitchState(MenuState.Off);
    }
    void DestroyBuildingButtons()
    {
        //Clear the page before we spawn the set of buttons.
        foreach (Transform child in buildingMenuCanvas)
        {
            Destroy(child.gameObject);
        }
    }
    void GenerateBuildingButtons(BuildingCategory category)
    {
        Buildings = BuildingLibrary.GetListForTimePeriod(category);

        DestroyBuildingButtons();

        for (int i = 0; i < Buildings.Count; i++)
        {
            GameObject go = Instantiate(buildingBtnPrefab, buildingMenuCanvas);
            BuildingButton buildingButton = go.GetComponentInChildren<BuildingButton>();
            
            GameObject model = Instantiate(Buildings[i].Model, buildingButton.modelParent.transform);
            model.transform.localPosition = Vector3.zero;
            buildingButton.ScaleModel(iconSize);

            buildingButton.index = i;
            buildingButton.costText.text = "£" + Buildings[i].Cost.ToString();
            buildingButton.nameText.text = Buildings[i].Name;
            go.SetActive(true);
            
            buildingButton.GetComponent<CompoundButton>().OnButtonClicked += BuildingPressed;
        }

        BuildingsGenerated(Buildings.Count);
    }
    void TextButtonPressed(GameObject go)
    {
        string Name = "";

        foreach (CompoundButton textButton in textButtons)
        {
            if (textButton == go.GetComponent<CompoundButton>())
            {
                textButton.GetComponent<TextButton>().ChangeMaterial(On);
                textButton.ButtonState = ButtonStateEnum.Pressed;
                Name = textButton.name.ToLower();
            }
            else
            {
                textButton.GetComponent<TextButton>().ChangeMaterial(Off);
            }
        }

        switch (Name)
        {
            case "commercial":
                GenerateBuildingButtons(BuildingCategory.Commercial);
                break;
            case "resource":
                GenerateBuildingButtons(BuildingCategory.Resource);
                break;
            case "residential":
                GenerateBuildingButtons(BuildingCategory.Residential);
                break;
            default:
                GenerateBuildingButtons(BuildingCategory.All);
                break;
        }
    }
    #endregion

    #region Menu Control
    public void MoveToTile(WorldGridTile tile)
    {
        if (targetTile == tile && menuState != MenuState.Off)
        {
            SwitchState(MenuState.Off);
            return;
        }
        
        targetTile = tile;

        if (targetTile.Model)
        {
            Vector3 pos = tile.transform.position;
            pos.y += targetTile.Model.GetComponent<MeshRenderer>().bounds.size.y;
            transform.position = pos;
        }
        else
        {
            transform.position = tile.transform.position;
        }
        SwitchState(MenuState.BuildMenu);
        
    }
    public void SwitchState(MenuState newState)
    {
        menuState = newState;
        
        if (menuState != MenuState.BuildingSelect)
            DestroyBuildingButtons();

        StateChanged((MenuState.Off == menuState));
        animator.SetInteger("MenuState", (int)menuState);
    }

    public void AnimFinished()
    {
        switch (menuState)
        {
            case MenuState.BuildingSelect:
                TextButtonPressed(textButtons[0].gameObject);
                break;
            default: break;
        }
    }
    #endregion

}
