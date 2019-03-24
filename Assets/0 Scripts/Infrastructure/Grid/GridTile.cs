using Infrastructure.Grid.Entities;
using Infrastructure.Tick;
using UnityEngine;

namespace Infrastructure.Grid
{
    public class GridTile
    {
        public GridSystem ParentGridSystem;
        public Vector2Int Position { get; private set; }
        public TileEntity Entity { get; private set; }
        public TileEntity MultiTileOccupier { get; set; }
        public bool Occipied {
            get {
                return (Entity != null || MultiTileOccupier != null);
            }
        }

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

    }
}
