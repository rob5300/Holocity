
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Medieval_Farm : Commercial2x2
    {
        public Medieval_Farm()
        {
            PrefabName = "Medieval/Farm";
            Name = "Farm";

            Jobs = new List<Job>();
            for (int i = 0; i < 15; i++)
            {
                Jobs.Add(new Job(this));
            }
        }
    }
}
