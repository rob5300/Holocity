using CityResources;
using Infrastructure.Tick;
using Settings.Adjustment;
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Commercial : Building, ITickable
    {
        public float ElectricityDrain = 1;
        public float WaterDrain = 0.3f;
        public float[] ResourceMissingTimes = { 0f, 0f };

        private float timeoutTime = 3f;

        public List<Job> Jobs;

        private AdjustableFloat commercialRate;

        public Commercial()
        {
            category = BuildingCategory.Commercial;
        }

        public override void OnEntityProduced(GridSystem grid)
        {
            base.OnEntityProduced(grid);

            commercialRate = Game.CurrentSession.Settings.CommercialTaxRateModifier;
        }

        public override void Tick(float time)
        {
            base.Tick(time);

            //Loop through each job and pay a small amount
            foreach(Job j in Jobs)
            {
                if (j.Taken)
                {
                    Game.CurrentSession.AddFunds((uint)System.Math.Floor(j.Salary.Value * commercialRate.Value));
                }
            }

            bool startElecState = HasPower;
            bool startWaterState = HasWaterSupply;

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
            
            foreach(Job job in Jobs)
            {
                if(job.Holder != null)
                {
                    job.Holder.RemoveJob();
                }
            }

            ParentTile.ParentGridSystem.ParentCity.ProcessDestroyedCommercial(this);
        }

        protected void AddJobs(int amount)
        {
            Jobs = new List<Job>();
            for (int i = 0; i < amount; i++)
            {
                Jobs.Add(new Job(this));
            }
        }
    }
}
