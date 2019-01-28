using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildEnums;
using BuildTool;
using Infrastructure.Grid.Entities.Buildings;

public class BuildingButton : MonoBehaviour {

    //will need to find a better way
    public Building building = new House();
    public ModernBuildings buildingEnum = ModernBuildings.House;

    float Cost = 50;

    // Use this for initialization
    void Start()
    {
        SetBuilding();
    }

    void Update()
    {

    }

    //TEMPORARY
    void SetBuilding()
    {
       string name = GetComponentInChildren<MeshFilter>().mesh.name;

        switch (name)
        {
            case "House Instance":
                buildingEnum = ModernBuildings.House;
                break; 
            case "Future House Instance":
                buildingEnum = ModernBuildings.Future_House;
                break;
            case "City Buildings Instance":
                buildingEnum = ModernBuildings.Modern_CityBuildings;
                break;
            case "Church Instance":
                buildingEnum = ModernBuildings.Old_Church;
                break;
            default:
                buildingEnum = ModernBuildings.House;
                break;
        }
    }
}
