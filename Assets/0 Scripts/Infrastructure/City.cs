using CityResources;
using System.Collections.Generic;
using Infrastructure.Grid;
using UnityEngine;

namespace Infrastructure {
    public class City {
        
        public string Owner;
        
        protected Dictionary<System.Type, Resource> Resources = new Dictionary<System.Type, Resource>();
        protected List<GridSystem> CityGrids = new List<GridSystem>();

        public City(string owner)
        {
            owner = owner != string.Empty ? owner : "Mayor";
        }

        public Resource GetResource<T>() where T : Resource {
            if (Resources.ContainsKey(typeof(T))) return Resources[typeof(T)];
            else return null;
        }

        public bool CreateGrid(int width, int height, Vector3 worldGridPosition)
        {
            CityGrids.Add(new GridSystem(width, height, CityGrids.Count, this, worldGridPosition));
            return true;
        }

        public GridSystem GetGrid(int id)
        {
            if (CityGrids.Count > id) return CityGrids[id];
            return null;
        }
    }
}
