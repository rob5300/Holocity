using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Future_House : House
    {
        public Future_House()
        {
            Name = "Future House";
            PrefabName = "Future/Future House";
            Cost = 250;
            category = BuildingCategory.Residential;
        }
    }
}
