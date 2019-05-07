using System;
using System.Collections.Generic;
using Infrastructure.Grid.Entities.Buildings;
using UnityEngine;


public static class BuildingLibrary
{
    public static List<Type> SetupModernBuildings = new List<Type>() {
        typeof(Modern_Airport),
        typeof(Modern_Bank),
        typeof(Modern_BusStop),
        typeof(Modern_Flats),
        typeof(Modern_House),
        typeof(Modern_LocalStore),
        typeof(Modern_Museum),
        typeof(Modern_Office),
        typeof(Modern_PoliceStation),
        typeof(Modern_Powerplant),
        typeof(Modern_Waterplant)
    };

    public static List<Type> SetupMedievalBuildings = new List<Type>()
    {
        typeof(Medieval_Tavern),
        typeof(Medieval_Theatre),
        typeof(Medieval_Townhall),
        typeof(Medieval_Well),
        typeof(Medieval_House),
        typeof(Medieval_Fountain),
        typeof(Medieval_HouseLarge),
        typeof(Medieval_HouseMedium),
        typeof(Medieval_Inn),
        typeof(Medieval_Mine),
        typeof(Medieval_Bank),
        typeof(Medieval_Church),
        typeof(Medieval_Shop),
        typeof(Medieval_Bank),
    };

    public static List<Type> SetupFuturisticBuildings = new List<Type>()
    {
        typeof(Future_Airport),
        typeof(Future_Bank),
        typeof(Future_Bar),
        typeof(Future_BusStation),
        typeof(Future_Church),
        typeof(Future_Clocktower),
        typeof(Future_Farm),
        typeof(Future_Gallery),
        typeof(Future_PowerPlant),
        typeof(Future_Statue),
        typeof(Future_Townhall),
        typeof(Future_Trainstation),
        typeof(Future_WaterPlant),
        typeof(Flat1),
        typeof(Flat2),
        typeof(Future_House)
    };


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
                    if (b.GetModel() != null)
                    {
                        BuildingMap map = new BuildingMap(b.GetModel(), typee, b.category, b.Name, b.Cost);
                        Buildings.Add(map); 
                    }
                }
            }
            catch (Exception)
            {
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

