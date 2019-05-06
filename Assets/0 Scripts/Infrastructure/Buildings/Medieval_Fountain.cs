using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Medieval_Fountain : Commercial
    {
        public Medieval_Fountain()
        {
            PrefabName = "Medieval/Fountain";
            Cost = 650;
            Name = "Fountain";

            Jobs = new List<Job>();
            for (int i = 0; i < 2; i++)
            {
                Jobs.Add(new Job(this));
            }
        }


    }
}
