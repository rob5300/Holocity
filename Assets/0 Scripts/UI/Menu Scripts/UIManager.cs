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
    public Action<bool> StateChanged = delegate { };
    public MenuState menuState = MenuState.MainMenu;

    [Header("Main Menu")]
    public SessionCreator sessionCreator;
    public GameObject mainMenu;
    public CompoundButton[] mainMenuButtons;

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

    public void Start()
    {

        animator = GetComponent<Animator>();
        roadTool = GetComponent<RoadTool>();

        Menus = new GameObject[] { buildingMenu, mainMenu, buildMenu };
        
        foreach(CompoundButton menuButton in buildMenuButtons)
        {
            menuButton.OnButtonClicked += BuildMenuButtonPressed;
        }

        foreach (CompoundButton menuButton in mainMenuButtons)
        {
            menuButton.OnButtonClicked += MainMenuButtonPressed;
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
                sessionCreator.StartNewGame();
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
    public void MoveToTile(WorldGridTile tile)
    {
        if (roadTool.active) return;

        // move the menu up if surrounding tiles have things on them.. or scale it up,.
        if (targetTile == tile && menuState != MenuState.Off)
        {
            SwitchState(MenuState.Off);
            return;
        }
        
        targetTile = tile;

        float height = GetHeight(tile) + 0.025f;

        Vector3 pos = tile.transform.position;
        pos.y += height;
        transform.position = pos;

        //if (targetTile.Model)
        //{
        //    Vector3 pos = tile.transform.position;
        //    pos.y += targetTile.Model.GetComponent<MeshRenderer>().bounds.size.y + 0.025f;
        //    transform.position = pos;
        //}
        //else
        //{
        //    transform.position = tile.transform.position;
        //}

        SwitchState(MenuState.BuildMenu);
        
    }

    float GetHeight(WorldGridTile tile)
    {
        float height = 0f;
        Vector2Int pos = tile.Position;
        WorldGrid grid = tile.ParentGrid;


        for(int i = pos.x - 1; i < pos.x + 2; i++)
        {
            for(int j = pos.y -1; j < pos.y + 2; j++)
            {
                WorldGridTile adjTile = grid.GetTile(i, j);
                if (adjTile == null) continue;

                if (adjTile.Model)
                {
                    height = (adjTile.Model.GetComponent<MeshRenderer>().bounds.size.y > height) ? adjTile.Model.GetComponent<MeshRenderer>().bounds.size.y : height;
                }
            }
        }


        return height;
    }


    public void SwitchState(MenuState newState)
    {
        menuState = newState;
        
        if (menuState != MenuState.BuildingSelect)
            _buildingMenu.DestroyBuildingButtons();

        StateChanged((MenuState.Off == menuState));
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
