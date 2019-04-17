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
    public BuildingTool buildingTool;
    private MoveUI moveUI;

    public void Start()
    {
        animator = GetComponent<Animator>();
        roadTool = GetComponent<RoadTool>();
        buildingTool = GetComponent<BuildingTool>();
        moveUI = GetComponent<MoveUI>();

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

        SwitchState(MenuState.BuildMenu);
    }

    public void SwitchState(MenuState newState)
    {
        menuState = newState;
        
        if (menuState != MenuState.BuildingSelect)
            _buildingMenu.DestroyBuildingButtons();

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
