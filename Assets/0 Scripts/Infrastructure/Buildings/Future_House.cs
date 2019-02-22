using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Future_House : Building
    {
        public Future_House()
        {
            Name = "Future House";
            PrefabName = "Future House";
            Cost = 250;
            category = BuildingCategory.Residential;
        }
    }
}
