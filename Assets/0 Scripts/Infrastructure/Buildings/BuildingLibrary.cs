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
            typeof(MultiTileHouse)
            };
    public static List<BuildingMap> ModernBuildings = new List<BuildingMap>();

    static BuildingLibrary()
    {
        foreach (Type typee in SetupModernBuildings)
        {
            try
            {
                object o = Activator.CreateInstance(typee);
                if (o is Building)
                {
                    Building b = (Building)o;
                    ModernBuildings.Add(new BuildingMap(b.GetModel(), typee, b.category, b.Name, b.Cost));
                }
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError("BUILDINGLIBRARY FAILED!");
#endif
            }

        }
#if UNITY_EDITOR
        Debug.Log("Built Building Library");
#endif
    }

    public static List<BuildingMap> GetListForTimePeriod(BuildingCategory category)
    {

      

        if(category == BuildingCategory.All)
        {
            return ModernBuildings;
        }
        else
        {

            List<BuildingMap> map = new List<BuildingMap>(); //this will get the time period list


            foreach (BuildingMap buildingMap in ModernBuildings)
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

