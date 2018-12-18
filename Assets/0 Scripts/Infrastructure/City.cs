using CityResources;
using System.Collections.Generic;
using Infrastructure.Grid;
using UnityEngine;
using System;
using Infrastructure.Residents;
using Infrastructure.Grid.Entities.Buildings;

namespace Infrastructure {
    public class City {
        
        public string Owner;
        public Session ParentSession;

        private List<Resident> Residents;
        protected Dictionary<Type, Resource> Resources = new Dictionary<Type, Resource>();
        protected List<GridSystem> CityGrids = new List<GridSystem>();

        private List<Residential> ResidentialBuildings;
        private List<Residential> VacantResidentialBuildings;

        /// <summary>
        /// The demand for residents to move in. If there is avaliable housing found or a new house built, this will reduce to fill the houses.
        /// </summary>
        public int ResidentDemand { get; private set; }

        private float _residentUpdateTime;

        public City(string owner, Session sess)
        {
            owner = owner != string.Empty ? owner : "Mayor";
            ParentSession = sess;
            Residents = new List<Resident>(10);
            ResidentDemand = 10;
        }

        /// <summary>
        /// Used to setup various components once all others are created.
        /// </summary>
        public void PostSetup()
        {
            ParentSession.TickManager.PostTick += ResetResourceTickCounters;

        }

        public T GetResource<T>() where T : Resource {
            if (!Resources.ContainsKey(typeof(T))) Resources.Add(typeof(T), Activator.CreateInstance<T>());
            return (T)Resources[typeof(T)];
        }

        public bool CreateGrid(int width, int height, Vector3 worldGridPosition)
        {
            GridSystem newGrid = new GridSystem(width, height, CityGrids.Count, this, worldGridPosition);
            CityGrids.Add(newGrid);
            //We add a command in the queue to add the new grid systems Tickable queue to the Tick system.
            ParentSession.TickManager.NewGridSystems.Enqueue(newGrid);
            newGrid.OnNewResidentialBuilding += NewResidential;
            return true;
        }

        public GridSystem GetGrid(int id)
        {
            if (CityGrids.Count > id) return CityGrids[id];
            return null;
        }

        private void ResetResourceTickCounters(Session s, float time)
        {
            foreach(Resource r in Resources.Values)
            {
                r.ResetCounters();
            }
        }
        
        private void NewResidential(Residential res)
        {
            if (!ResidentialBuildings.Contains(res))
            {
                //If the building is Vacant, we put it in the vacant buildings list.
                //We need to sort everything now at the expense of memory.
                //Cant afford to check all buildings each time or use LINQ ;(.
                if (res.IsVacant)   VacantResidentialBuildings.Add(res);
                else                ResidentialBuildings.Add(res);
            }
        }

        private void ResidentVacancyUpdate(Session s, float time)
        {
            _residentUpdateTime += time;
            if(time > 1000)
            {
                
            }
        }
    }
}
