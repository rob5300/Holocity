
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Medieval_Bank : Commercial2x2
    {
        public Medieval_Bank()
        {
            PrefabName = "Medieval/Bank";
            Name = "Bank";
            Cost = 1500;

            Jobs = new List<Job>();
            for (int i = 0; i < 30; i++)
            {
                Jobs.Add(new Job(this));
            }
        }
    }
}
