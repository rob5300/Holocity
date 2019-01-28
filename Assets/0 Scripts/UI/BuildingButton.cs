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

    // Use this for initialization
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        //Would like to assign the mesh as it's instantiated.
       // if (mesh)
         //   mesh = FindBuilding(mesh.name);
        
        /*
        if(mesh != null && building != null)
           mesh = building.BuildingPrefab.GetComponent<MeshFilter>().mesh;
           */
    }

    //TEMPORARY
    /*
    Mesh FindBuilding(string name)
    {
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
        
       // return Tools.GetBuilding(buildingEnum).BuildingPrefab.GetComponent<MeshFilter>().GetComponent<Mesh>();
    }
    */
    void Update()
    {
        
    }
}
