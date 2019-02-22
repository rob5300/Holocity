using Infrastructure.Grid;
using System;
using UnityEngine;

namespace Infrastructure.Grid.Entities.Buildings
{
    public enum BuildingCategory
    {
        Residential,
        Commercial,
        Resource,
        All
    }



    public abstract class Building : TileEntity
    {
        public string Name;
        public BuildingCategory category;

        public Building()
        {
            ResourcesFolderPath = "Buildings/";
        }
        
    }
}
