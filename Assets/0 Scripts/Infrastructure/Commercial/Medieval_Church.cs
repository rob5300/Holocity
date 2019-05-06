
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Medieval_Church : Commercial2x2
    {
        public Medieval_Church()
        {
            PrefabName = "Medieval/Church";
            Name = "Church";
            Cost = 400;

            Jobs = new List<Job>();
            for (int i = 0; i < 5; i++)
            {
                Jobs.Add(new Job(this));
            }
        }
    }
}
