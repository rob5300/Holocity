
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Shop : Commercial
    {
        public Shop()
        {
            Name = "Shop";
            PrefabName = "Shop Fixed";
            Cost = 500;
            category = BuildingCategory.Commercial;

            Jobs = new List<Job>();
            for (int i = 0; i < 10; i++)
            {
                Jobs.Add(new Job(this));
            }
        }
    }
}
