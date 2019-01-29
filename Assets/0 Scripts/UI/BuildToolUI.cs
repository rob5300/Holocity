using HoloToolkit.Unity.Buttons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildEnums;
using UnityEngine.Events;
using BuildTool;

public class BuildToolUI : MonoBehaviour {

    enum MenuState {MainMenu, BuildingSelect}

    MenuState menuState = MenuState.MainMenu;
    

    [Header("Main Menu")]
    public GameObject mainMenu;
    public CompoundButton BuildButton;
    public CompoundButton RoadButton;
    public CompoundButton DestroyButton;

    [Header("Building Menu")]
    public GameObject BuildingMenu;
    public GameObject buildingBtnPrefab;
    public List<CompoundButton> buildingButtons;

    public void Start()
    {
        BuildButton.OnButtonClicked += BuildPressed;    
        RoadButton.OnButtonClicked += RoadPressed;    
        DestroyButton.OnButtonClicked += DestroyPressed;
        
        foreach(CompoundButton btn in buildingButtons)
        {
            btn.OnButtonClicked += BuildingPressed;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Button Event Methods

    void BuildPressed(GameObject go)
    {
        SwitchState(MenuState.BuildingSelect);
        //GenerateBuildingButtons();
    }

    void RoadPressed(GameObject go)
    {
        Debug.Log("road");
    }

    void DestroyPressed(GameObject go)
    {
        Debug.Log("destroy");
    }

    void BuildingPressed(GameObject go)
    {
        Vector2Int pos = transform.parent.GetComponent<WorldGridTile>().Position;
        ModernBuildings building = go.GetComponent<BuildingButton>().buildingEnum;

        Tools.SpawnBuilding(pos, building);
        
    }

    //BuildTool Methods

    public void MoveUI(WorldGridTile tile)
    {
        if (!ToggleUI(tile)) return;

        transform.SetParent(tile.transform);
        transform.localPosition = new Vector2(0, 0);
    }

    public bool ToggleUI(WorldGridTile tile)
    {
        //check if player has tapped on the same grid
        if(gameObject.activeSelf && transform.parent == tile.transform)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            SwitchState(MenuState.MainMenu);
        }

        return gameObject.activeSelf;
    }

    void SwitchState(MenuState menuState)
    {
        if (!mainMenu || !BuildingMenu) return;

        switch (menuState)
        {
            case MenuState.MainMenu:
                mainMenu.SetActive(true);
                BuildingMenu.SetActive(false);
                break;
            case MenuState.BuildingSelect:
                mainMenu.SetActive(false);
                BuildingMenu.SetActive(true);
                break;
            default: return;
        }
    }
    
    void GenerateBuildingButtons()
    {

    }

}
