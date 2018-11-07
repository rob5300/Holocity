using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class House : Building
    {
        public House()
        {
            Name = "House";
            BuildingPrefabPath = "Basic House";
        }
    }
}
