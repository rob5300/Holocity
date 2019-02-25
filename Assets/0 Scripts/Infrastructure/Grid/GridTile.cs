using Infrastructure.Grid.Entities;
using Infrastructure.Tick;
using UnityEngine;

namespace Infrastructure.Grid
{
    public class GridTile
    {
        public GridSystem ParentGridSystem;
        public Vector2Int Position { get; private set; }
        public TileEntity Entity { get; private set;  }

        public GridTile(Vector2Int position, GridSystem system)
        {
            Position = position;
            ParentGridSystem = system;
        }

        public void SetEntity(GridSystem grid, TileEntity entity)
        {
            Entity = entity;
            Entity.ParentTile = this;
            Entity.OnEntityProduced(grid);
        }

        public void DestroyEntity()
        {
            Entity = null;
        }

        /// <summary>
        /// Sets the tile entity and doesn't call the placed event. Do not use for other situations.
        /// </summary>
        /// <param name="ent"></param>
        public void SetEntityFromSwap(TileEntity ent)
        {
            Entity = ent;
            if(Entity != null) Entity.ParentTile = this;
        }

        /// <summary>
        /// Get adjacent grid tiles. Can return null in the array if a tile doesn't exist.
        /// </summary>
        /// <returns>An array of adjacent grid tiles. Index is null if it did not exist.</returns>
        public GridTile[] GetAdjacentGridTiles()
        {
            GridTile[] adjacentTiles = {
                ParentGridSystem.GetTile(Position + new Vector2Int(0, 1)),
                ParentGridSystem.GetTile(Position + new Vector2Int(1, 0)),
                ParentGridSystem.GetTile(Position + new Vector2Int(0, -1)),
                ParentGridSystem.GetTile(Position + new Vector2Int(-1, 0))
            };
            return adjacentTiles;
        }
    }
}
