using CityResources;
using Infrastructure.Tick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class PowerPlant : Building, Tickable
    {
        public int PowerIncreaseRate = 5;

        private Electricity elecResource;

        public PowerPlant()
        {
            BuildingPrefabPath = "Powerplant";
        }

        public override void OnEntityProduced(GridSystem grid)
        {
            elecResource = grid.ParentCity.GetResource<Electricity>();
        }

        public void Tick()
        {
            elecResource.Add(PowerIncreaseRate);
        }
    }
}
