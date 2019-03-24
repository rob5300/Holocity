using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Grid.Entities.Buildings
{
    [TileEntityMeta("", MultiTileSize.x2)]
    public class MultiTileHouse : House
    {
        public MultiTileHouse()
        {
            PrefabName = "2x2House";
            Cost = 1;
            AllowRotation = false;
            CanBeMoved = false;
        }

        public override void OnEntityProduced(GridSystem grid)
        {
            base.OnEntityProduced(grid);

            //Occupy the adjacent tiles depending on our size
            //Get the layout from the enum.
            OccupyTiles(TileEntityMetaAttribute.GetTileLayout(MultiTileSize.x2));
        }

        public override void OnWorldGridTileCreated(WorldGridTile tile)
        {
            base.OnWorldGridTileCreated(tile);

            tile.Model.transform.localPosition = new Vector3(-0.075f, 0, -0.075f);
        }
    }
}
