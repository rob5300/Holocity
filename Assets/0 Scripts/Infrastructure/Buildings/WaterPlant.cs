using CityResources;
using Infrastructure.Tick;
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class WaterPlant : Commercial, ITickable
    {
        public int PowerIncreaseRate = 15;

        public float[] ResourceMissingTimes = { 0f, 0f };
        public float ElectricityDrain = 1;
        public float WaterDrain = 0.3f;
        private float timeoutTime = 3f;

        private Water _waterResource;
        private ResourceData waterData;

        public WaterPlant()
        {
            Name = "Water Plant";
            PrefabName = "Waterplant";
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
            _waterResource = new Water(ParentTile.ParentGridSystem.Id);
            waterData = new ResourceData(_waterResource, this);
            AddNewResource(typeof(Water), waterData);

            //We need to call base to distribute our resource AFTER we make ours.
            base.OnEntityProduced(grid);

            HasWaterSupply = true;

            Game.CurrentSession.TaskManager.Tasks.Enqueue(waterWarningTask);
        }

        public override void Tick(float time)
        {
            base.Tick(time);
            _waterResource.Add(PowerIncreaseRate * time);

            bool startElecState = HasPower;
            bool startWaterState = HasWaterSupply;

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
                if (ResourceMissingTimes[0] > timeoutTime) HasPower = false;
            }
            else
            {
                //We got the power we requested.
                ResourceMissingTimes[0] = 0;
                HasPower = true;
            }

            if (startElecState != HasPower) Game.CurrentSession.TaskManager.Tasks.Enqueue(electricityWarningTask);
            #endregion
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            waterData.Destroy();
        }
    }
}
