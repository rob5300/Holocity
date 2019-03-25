using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Grid.Entities.Buildings
{
    [TileEntityMeta("Buildings/OldTownHall", MultiTileSize.x2)]
    public class Old_TownHall : MultiTileHouse
    {
        public Old_TownHall()
        {
            PrefabName = "OldTownHall";
            Name = "Town Hall";
            Cost = 500;
        }

        public override void Tick(float time)
        {
            base.Tick(time);

            if (!IsVacant && HasWaterSupply && HasPower)
            {
                Game.CurrentSession.AddFunds(15);
            }
        }
    }
}
