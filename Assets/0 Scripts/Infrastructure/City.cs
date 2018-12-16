﻿using CityResources;
using System.Collections.Generic;
using Infrastructure.Grid;
using UnityEngine;
using System;

namespace Infrastructure {
    public class City {
        
        public string Owner;
        public Session ParentSession;

        protected Dictionary<System.Type, Resource> Resources = new Dictionary<System.Type, Resource>();
        protected List<GridSystem> CityGrids = new List<GridSystem>();

        public City(string owner, Session sess)
        {
            owner = owner != string.Empty ? owner : "Mayor";
            ParentSession = sess;
        }

        public Resource GetResource<T>() where T : Resource {
            if (Resources.ContainsKey(typeof(T))) return Resources[typeof(T)];
            else return null;
        }

        public bool CreateGrid(int width, int height, Vector3 worldGridPosition)
        {
            GridSystem newGrid = new GridSystem(width, height, CityGrids.Count, this, worldGridPosition);
            CityGrids.Add(newGrid);
            //We add a command in the queue to add the new grid systems Tickable queue to the Tick system.
            ParentSession.TickManager.NewGridSystems.Enqueue(newGrid);
            return true;
        }

        public GridSystem GetGrid(int id)
        {
            if (CityGrids.Count > id) return CityGrids[id];
            return null;
        }
    }
}
