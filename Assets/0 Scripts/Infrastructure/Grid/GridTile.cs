using Infrastructure.Grid.Entities;
using UnityEngine;

namespace Infrastructure.Grid
{
    public class GridTile
    {
        public Vector2Int Position { get; private set; }
        public GridEntity Entity { get; private set;  }

        public GridTile(Vector2Int position)
        {
            Position = position;
        }

        public void SetEntity(GridSystem grid, GridEntity entity)
        {
            Entity = entity;
            Entity.OnEntityProduced(grid);
        }
    }
}
