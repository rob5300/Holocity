using HoloToolkit.Unity.Buttons;
using UnityEngine;
using Infrastructure.Grid.Entities;
using System;

public class UIManager : MonoBehaviour {
    // option menu
    // test bimanual gestures.

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
    public Action<int> StateChanged = delegate { };
    public MenuState menuState = MenuState.MainMenu;

    [Header("Main Menu")]
    public SessionCreator sessionCreator;
    public SessionManager sessionManager;
    public GameObject mainMenu;
    public GameObject mainOne;
    public GameObject mainTwo;
    public CompoundButton[] mainMenuButtons;
    public CompoundButton[] timeButtons;

    [Header("Build Menu")]
    public GameObject buildMenu;
    public CompoundButton[] buildMenuButtons;

    [Header("Building Menu")]
    public GameObject buildingMenu;
    public BuildingMenu _buildingMenu;
    
    private Animator animator;
    private GameObject[] Menus;

    [HideInInspector]
    public WorldGridTile targetTile;
    public RoadTool roadTool;
    public BuildingTool buildingTool;
    private MoveUI moveUI;
    public TileHighlighter tileHighlighter;

    public void Start()
    {
        animator = GetComponent<Animator>();
        roadTool = GetComponent<RoadTool>();
        buildingTool = GetComponent<BuildingTool>();
        moveUI = GetComponent<MoveUI>();
        tileHighlighter = GetComponent<TileHighlighter>();

        Menus = new GameObject[] { buildingMenu, mainMenu, buildMenu };
        
        foreach(CompoundButton menuButton in buildMenuButtons)
        {
            menuButton.OnButtonClicked += BuildMenuButtonPressed;
        }

        foreach (CompoundButton menuButton in mainMenuButtons)
        {
            menuButton.OnButtonClicked += MainMenuButtonPressed;
        }

        foreach (CompoundButton timeButton in timeButtons)
        {
            timeButton.OnButtonClicked += TimeButtonPressed;
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
                SwitchMainMenu(true);
                break; 
            case "quit":
                Application.Quit();
                break;
            case "save":
                sessionManager.SaveGame();
                SwitchState(MenuState.Off);
                break;
            case "load":
                sessionManager.LoadGame("");
                SwitchState(MenuState.Off);
                break;
            default:
                break;
        }
        
        

    }

    void TimeButtonPressed(GameObject go)
    {
        string Name = "";

        foreach (CompoundButton timeButton in timeButtons)
        {
            if (timeButton == go.GetComponent<CompoundButton>())
            {
                timeButton.ButtonState = ButtonStateEnum.Disabled;
                Name = timeButton.name.ToLower();
                break;
            }
        }
        
        //Convert the name to an int to then cast to the enum.
        sessionCreator.StartNewGame(Convert.ToInt32(Name));
        SwitchState(MenuState.Off);
    }

    void SwitchMainMenu(bool timeSelection)
    {
        mainOne.SetActive(!timeSelection);
        mainTwo.SetActive(timeSelection);
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
        //Start Road Tool
        roadTool.StartTool(targetTile);

        //    Vector2Int pos = targetTile.Position;
        //    targetTile.ParentGrid.GridSystem.AddTileEntityToTile(pos, new Road());
        //
    }
    void DestroyTile(GameObject go)
    {
        Game.CurrentSession.City.GetGrid(targetTile.ParentGrid.Id).DestroyTileEntity(targetTile.Position);
    }

    #endregion
    
    #region Menu Control
    public void TilePressed(WorldGridTile tile)
    {
        if (roadTool.active || buildingTool.active) return;

        // move the menu up if surrounding tiles have things on them.. or scale it up,.
        if (targetTile == tile && menuState != MenuState.Off)
        {
            SwitchState(MenuState.Off);
            return;
        }
        targetTile = tile;

        moveUI.MoveToTile(targetTile);
        tileHighlighter.currentTarget = targetTile.transform.GetChild(0).gameObject;

        SwitchState(MenuState.BuildMenu);
    }

    public void SwitchState(MenuState newState)
    {
        menuState = newState;
        SwitchMainMenu(false);

        if (menuState != MenuState.BuildingSelect)
        {
            _buildingMenu.DestroyBuildingButtons();
        }
        else
        {
            _buildingMenu.ChangeBuildingCategory("all");
        }
        StateChanged((int)menuState);
        animator.SetInteger("MenuState", (int)menuState);
    }

    public void AnimFinished()
    {
        switch (menuState)
        {
            case MenuState.BuildingSelect:
                _buildingMenu.ShowAllBuildings();
                break;
            default: break;
        }
    }
    #endregion

}
