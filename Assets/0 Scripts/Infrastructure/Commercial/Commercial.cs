using Infrastructure.Tick;
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Commercial : Building, ITickable
    {
        public List<Job> Jobs;

        public override void Tick(float time)
        {
            base.Tick(time);

            //Loop through each job and pay a small amount
            foreach(Job j in Jobs)
            {
                if (j.Taken)
                {
                    Game.CurrentSession.AddFunds((uint)System.Math.Floor(j.Salary.Value * 0.05f));
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
