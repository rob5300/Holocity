using UnityEngine;

namespace Grid
{
    public class GridTile
    {
        public Vector2Int Position { get; private set; }
        public GridEntity Entity;

        public GridTile(Vector2Int position)
        {
            Position = position;
        }
    }
}
