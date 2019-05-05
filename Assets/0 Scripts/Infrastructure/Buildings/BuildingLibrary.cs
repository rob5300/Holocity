using System;
using System.Collections.Generic;
using Infrastructure.Grid.Entities.Buildings;
using UnityEngine;


public static class BuildingLibrary
{
    public static List<Type> SetupModernBuildings = new List<Type>() {
            typeof(House),
            typeof(Modern_CityBuildings),
            typeof(Old_Church),
            typeof(PowerPlant),
            typeof(WaterPlant),
            typeof(Future_House),
            typeof(Old_House_1),
            typeof(Shop),
            typeof(Flat1),
            typeof(Flat2),
            typeof(MultiTileHouse),
            typeof(Old_TownHall)
            };

    public static List<Type> SetupMedievalBuildings = new List<Type>()
    {
        typeof(Medieval_Tavern),
        typeof(Medieval_Theatre),
        typeof(Medieval_Townhall),
        typeof(Medieval_Well)
    };

    public static List<Type> SetupFuturisticBuildings = new List<Type>();

    public static List<BuildingMap> Buildings = new List<BuildingMap>();

    static BuildingLibrary()
    {
        //Assign list depending on time period
        List<Type> btypes;
        switch (Game.CurrentSession.Settings.CurrentTimePeriod)
        {
            case Settings.TimePeriod.Futuristic:
                btypes = SetupFuturisticBuildings;
                break;
            case Settings.TimePeriod.Medieval:
                btypes = SetupMedievalBuildings;
                break;
            default:
            case Settings.TimePeriod.Modern:
                btypes = SetupModernBuildings;
                break;
        }

        foreach (Type typee in btypes)
        {
            try
            {
                object o = Activator.CreateInstance(typee);
                if (o is Building)
                {
                    Building b = (Building)o;
                    Buildings.Add(new BuildingMap(b.GetModel(), typee, b.category, b.Name, b.Cost));
                }
            }
            catch (Exception e)
            {
                #if UNITY_EDITOR
                Debug.LogError(e.Message);
                #endif
            }

        }
    }

    public static List<BuildingMap> GetBuildingsForCategory(BuildingCategory category)
    {
        if(category == BuildingCategory.All)
        {
            return Buildings;
        }
        else
        {
            List<BuildingMap> map = new List<BuildingMap>(); //this will get the time period list

            foreach (BuildingMap buildingMap in Buildings)
            {
                if (buildingMap.Category == category)
                    map.Add(buildingMap);
            }

            return map;
        }
    }

}

public struct BuildingMap
{
    public GameObject Model;
    public Type BuildingType;
    public BuildingCategory Category;
    public string Name;
    public uint Cost;

    public BuildingMap(GameObject model, Type buildingType, BuildingCategory category, string name, uint cost)
    {
        Model = model;
        BuildingType = buildingType;
        Category = category;
        Name = name;
        Cost = cost;
    }
}

