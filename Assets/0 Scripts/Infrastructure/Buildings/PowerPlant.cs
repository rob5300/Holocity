using CityResources;
using Infrastructure.Tick;
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class PowerPlant : Commercial, ITickable
    {
        public int PowerIncreaseRate = 5;

        public float[] ResourceMissingTimes = { 0f, 0f };
        public float ElectricityDrain = 1;
        public float WaterDrain = 0.3f;
        private float timeoutTime = 3f;

        private Electricity elecResource;
        private ResourceData elecData;

        public PowerPlant()
        {
            Name = "Powerplant";
            PrefabName = "Powerplant Future";
            Cost = 2500;
            category = BuildingCategory.Resource;

            Jobs = new List<Job>();
            for (int i = 0; i < 5; i++)
            {
                Jobs.Add(new Job(this));
            }
        }

        public override void OnEntityProduced(GridSystem grid)
        {
            elecResource = new Electricity(ParentTile.ParentGridSystem.Id);
            elecData = new ResourceData(elecResource, this);
            AddNewResource(typeof(Electricity), elecData);

            //Must be called after soo the resources exist.
            base.OnEntityProduced(grid);

            HasPower = true;

            Game.CurrentSession.TaskManager.Tasks.Enqueue(electricityWarningTask);
        }

        public override void Tick(float time)
        {
            base.Tick(time);

            elecResource.Add(PowerIncreaseRate * time);

            bool startElecState = HasPower;
            bool startWaterState = HasWaterSupply;

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
                if (ResourceMissingTimes[1] > timeoutTime) HasWaterSupply = false;
            }
            else
            {
                //We got the power we requested.
                ResourceMissingTimes[1] = 0;
                HasWaterSupply = true;
            }

            if (startWaterState != HasWaterSupply) Game.CurrentSession.TaskManager.Tasks.Enqueue(waterWarningTask);
            #endregion
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            elecData.Destroy();
        }
    }
}
