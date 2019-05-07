using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Medieval_Mine : PowerPlant
    {
        public Medieval_Mine()
        {
            Name = "Mine";
            PrefabName = "Medieval/Mine";
            Cost = 2500;
        }
    }
}
