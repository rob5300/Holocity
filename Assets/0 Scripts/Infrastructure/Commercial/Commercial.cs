using Infrastructure.Tick;
using Settings.Adjustment;
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Commercial : Building, ITickable
    {
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
    }
}
