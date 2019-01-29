using HoloToolkit.Unity.Buttons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildEnums;
using UnityEngine.Events;
using BuildTool;
using Infrastructure.Grid.Entities;
using System;

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

    private List<BuildingMap> Buildings;

    public void Start()
    {
        BuildButton.OnButtonClicked += BuildPressed;
        RoadButton.OnButtonClicked += RoadPressed;
        DestroyButton.OnButtonClicked += DestroyPressed;

        Buildings = BuildingLibrary.GetListForTimePeriod();

        for (int i = 0; i < Buildings.Count; i++)
        {
            GameObject go = Instantiate(buildingBtnPrefab, BuildingMenu.transform);
            GameObject model = Instantiate(Buildings[i].Model, go.transform);
            model.transform.localScale *= 5;
            go.GetComponent<BuildingButton>().index = i;
            go.GetComponent<CompoundButton>().OnButtonClicked += BuildingPressed;
            go.SetActive(true);
        }
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
        Tools.SpawnBuilding(pos, Activator.CreateInstance(Buildings[go.GetComponent<BuildingButton>().index].BuildingType) as TileEntity);
        ToggleUI();
    }

    //BuildTool Methods

    public void MoveUI(WorldGridTile tile)
    {
        if (!ToggleUI(tile)) return;

        transform.SetParent(tile.transform);
        transform.localPosition = new Vector2(0, 0);
    }

    public void ToggleUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        SwitchState(MenuState.MainMenu);
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
