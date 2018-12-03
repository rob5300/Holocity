using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Modern_CityBuildings : Building
    {
        public Modern_CityBuildings()
        {
            Name = "City Buildings";
            BuildingPrefabPath = "City Buildings Modern Day";
        }
    }
}
