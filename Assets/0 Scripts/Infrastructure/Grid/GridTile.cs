using Infrastructure.Grid.Entities;
using UnityEngine;

namespace Infrastructure.Grid
{
    public class GridTile
    {
        public Vector2Int Position { get; private set; }
        public TileEntity Entity { get; private set;  }

        public GridTile(Vector2Int position)
        {
            Position = position;
        }

        public void SetEntity(GridSystem grid, TileEntity entity)
        {
            Entity = entity;
            Entity.OnEntityProduced(grid);
        }

        /// <summary>
        /// Sets the tile entity and doesn't call the placed event. Do not use for other situations.
        /// </summary>
        /// <param name="ent"></param>
        public void SetEntityFromSwap(TileEntity ent)
        {
            Entity = ent;
        }
    }
}
