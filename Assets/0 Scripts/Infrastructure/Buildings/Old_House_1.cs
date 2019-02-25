using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Old_House_1 : House
    {
        public Old_House_1()
        {
            Name = "Old House";
            PrefabName = "Old House 1";
            Cost = 250;
            category = BuildingCategory.Residential;
        }
    }
}
