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
        public int Power = 0;
        public int PowerIncreaseRate = 5;

        public PowerPlant()
        {
            BuildingPrefabPath = "Powerplant";
        }

        public override void OnWorldGridTileCreated(WorldGridTile tile)
        {
            //PowerPrint p = tile.gameObject.AddComponent<PowerPrint>();
            //p.plant = this;
        }

        public void Tick()
        {
            Power += PowerIncreaseRate;
        }
    }
}
