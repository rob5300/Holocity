using CityResources;
using Infrastructure.Residents;
using Infrastructure.Tick;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class House : Residential, Tickable
    {
        public float ElectricityDrain = 1;
        public float WaterDrain = 0.3f;
        public float[] ResourceMissingTimes = { 0f, 0f };

        private bool showingElectricityWarning = false;
        private float timeoutTime = 3f;
        WorldGridTile gridtile;

        private WorldGridTaskManager.WorldGridTask setResTask;

        public House()
        {
            Name = "House";
            PrefabName = "Basic House";
            Cost = 100;
        }

        public override void OnWorldGridTileCreated(WorldGridTile tile)
        {
            gridtile = tile;
            tile.Model.GetComponent<MeshRenderer>().material.color = Color.grey;
            //ITS GREY NOT GRAY.
        }

        public override void SetResident(Resident res)
        {
            //Must call base on this to actually set the resident.
            base.SetResident(res);

            //Define a delegate with some thing to do on the Unity thread.
            setResTask = (wGrid) => { gridtile.Model.GetComponent<MeshRenderer>().material.color = Color.green; };
            //This is where we queue the delegate. This is thread safe now!
            gridtile.ParentGrid.TaskManager.WorldGridTasks.Enqueue(setResTask);
        }

        public override void Tick(float time)
        {
            base.Tick(time);

            if (IsVacant) return;

            //Request the drain amount of each resource. If the request and recieved amount doesnt match we handle that.
            #region Electricity
            float request_electricity = ElectricityDrain * time;
            float recieved_electricity = 0;

            if (CurrentResources.ContainsKey(typeof(Electricity)))
            {
                List<ResourceData> electricity = CurrentResources[typeof(Electricity)];
                foreach (ResourceData rData in electricity)
                {
                    recieved_electricity = rData.resource.Recieve(request_electricity);
                    if (request_electricity == recieved_electricity) break;
                } 
            }

            if (recieved_electricity != request_electricity)
            {
                //Track how long we have not had the power we requested. If its above the acceptable value we dont consider this house to have power.
                ResourceMissingTimes[0] += time;
                if (time > timeoutTime) HasPower = false;
                gridtile.ParentGrid.TaskManager.WorldGridTasks.Enqueue((wGrid) => { Debug.Log("House does not have power."); });
            }
            else
            {
                //We got the power we requested.
                ResourceMissingTimes[0] = 0;
                HasPower = true;
                gridtile.ParentGrid.TaskManager.WorldGridTasks.Enqueue((wGrid) => { Debug.Log("House has power."); });
            }
            #endregion
            #region Water
            float request_water = WaterDrain * time;
            float recieved_water = 0;

            if (CurrentResources.ContainsKey(typeof(Water)))
            {
                List<ResourceData> water = CurrentResources[typeof(Water)];
                foreach (ResourceData rData in water)
                {
                    recieved_water = rData.resource.Recieve(request_water);
                    if (request_water == recieved_water) break;
                } 
            }

            if (recieved_water != request_water)
            {
                //Track how long we have not had the water we requested. If its above the acceptable value we dont consider this house to have water.
                ResourceMissingTimes[1] += time;
                if (time > timeoutTime) HasWaterSupply = false;
            }
            else
            {
                //We got the power we requested.
                ResourceMissingTimes[1] = 0;
                HasWaterSupply = true;
            }
            #endregion
        }
    }
}
