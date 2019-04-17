using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Commercial : Building
    {
        public List<Job> Jobs;

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
