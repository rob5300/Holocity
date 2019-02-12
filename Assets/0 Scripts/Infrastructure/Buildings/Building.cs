using Infrastructure.Grid;
using System;
using UnityEngine;

namespace Infrastructure.Grid.Entities.Buildings
{
    public abstract class Building : ResourceConductEntity
    {
        public string Name;

        public Building()
        {
            ResourcesFolderPath = "Buildings/";
        }
        
    }
}
