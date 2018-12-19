using CityResources;
using Infrastructure.Residents;
using Infrastructure.Tick;
using UnityEngine;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class House : Residential, Tickable
    {
        public float ElectricityDrain = 1;

        private Electricity _elec;
        WorldGridTile gridtile;
        //Hoping this helps with garbage soo i can still use a lambda xD
        private WorldGridTaskManager.WorldGridTask setResTask;

        public House()
        {
            Name = "House";
            BuildingPrefabPath = "Basic House";
        }

        public override void OnEntityProduced(GridSystem grid)
        {
            _elec = grid.ParentCity.GetResource<Electricity>();
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
            float request = ElectricityDrain * time;
            float recieved = _elec.Recieve(request);
            if (recieved != request)
            {
                //If the amount we requested is not the same as what we got, track this.
                //This will be to handle uphappiness later.

            }
        }
    }
}
