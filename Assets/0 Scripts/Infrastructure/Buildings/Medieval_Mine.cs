using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Medieval_Mine : Commercial
    {
        public Medieval_Mine()
        {
            Name = "Mine";
            PrefabName = "Medieval/Mine";
            Cost = 2500;

            Jobs = new List<Job>();
            for (int i = 0; i < 25; i++)
            {
                Jobs.Add(new Job(this));
            }
        }
    }
}
