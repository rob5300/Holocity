using CityResources;
using Infrastructure.Residents;
using Infrastructure.Tick;
using UnityEngine;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class House : Residential, Tickable
    {
        public float ElectricityDrain = 1;
        public float WaterDrain = 0.3f;
        public float[] ResourceMissingTimes = { 0f, 0f };

        private Electricity _elec;
        private Water _water;
        WorldGridTile gridtile;
        //Hoping this helps with garbage soo i can still use a lambda xD
        private WorldGridTaskManager.WorldGridTask setResTask;

        public House()
        {
            Name = "House";
            PrefabName = "Basic House";
            Cost = 100;
        }

        public override void OnEntityProduced(GridSystem grid)
        {
            _elec = grid.ParentCity.GetResource<Electricity>();
            _water = grid.ParentCity.GetResource<Water>();
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

            //This is to set the material of this house to be green.
            //We are using a delegate to do this as this is executed on the tick thread and thus isnt allowed by unity.
            //There is a component called WorldGridTaskManager to execute a Queue of delegates on each worldgrid for these tasks,
            //Therefore this will happen on the main thread safley and we can keep the threads going.
            setResTask = (wGrid) => { gridtile.Model.GetComponent<MeshRenderer>().material.color = Color.green; };
            //This is where we queue the delegate. This is not thread safe currently but should work sometimes.
            gridtile.ParentGrid.TaskManager.WorldGridTasks.Enqueue(setResTask);
        }

        public void Tick(float time)
        {
            if (IsVacant) return;

            //Request the drain amount of each resource. If the request and recieved amount doesnt match we handle that.
            #region Electricity
            float request_electricity = ElectricityDrain * time;
            float recieved_electricity = _elec.Recieve(request_electricity);
            if (recieved_electricity != request_electricity)
            {
                //Track how long we have not had the power we requested. If its above the acceptable value we dont consider this house to have power.
                ResourceMissingTimes[0] += time;
                if (time > _elec.TimeoutTime) HasPower = false;
            }
            else
            {
                //We got the power we requested.
                ResourceMissingTimes[0] = 0;
                HasPower = true;
            }
            #endregion
            #region Water
            float request_water = WaterDrain * time;
            float recieved_water = _water.Recieve(request_water);
            if(recieved_water != request_water)
            {
                ResourceMissingTimes[1] += time;
                if (time > _water.TimeoutTime) HasWaterSupply = false;
            }
            else
            {
                HasWaterSupply = true;
                ResourceMissingTimes[1] = 0;
            }
            #endregion
        }
    }
}
