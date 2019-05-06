using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Medieval_HouseMedium : House
    {
        public Medieval_HouseMedium()
        {
            PrefabName = "Medieval/Medium-House";
            Cost = 300;
            Name = "Medium House";
        }
    }
}
