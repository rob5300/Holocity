using UnityEngine;
using UnityEngine.XR.WSA;

namespace Grid
{
    public class GridSystem
    {
        public int Id = 0;
        public GameObject AnchorObject;
        public Vector3 Position { get { return AnchorObject.transform.position; } }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public GridTile[][] Tiles;

        public GridSystem(int width, int height, int id) : this(width, height, id, GetDefaultAnchor(id))
        {
            
        }

        public GridSystem(int width, int height, int id, GameObject anchor)
        {
            Id = id;
            AnchorObject = anchor;
            if (!anchor.GetComponent<WorldAnchor>()) anchor.AddComponent<WorldAnchor>();
            Width = width;
            Height = height;

            Tiles = new GridTile[Width][];

            //Initialize Tiles array with empty tiles:
            for(int x = 0; x < Width; x++)
            {
                Tiles[x] = new GridTile[Height];
                for(int y = 0; y < Height; y++)
                {
                    Tiles[x][y] = new GridTile(new Vector2Int(x, y));
                }
            }
        }
        
        private static GameObject GetDefaultAnchor(int Id)
        {
            GameObject ob = new GameObject("Grid Anchor: " + Id);
            ob.transform.position = Vector3.zero;
            ob.AddComponent<WorldAnchor>();
            return ob;
        }

        public GridTile GetTile(Vector2Int position)
        {
            if (position.x <= Width && position.y <= Height) return Tiles[position.x][position.y];
            return null;
        }
    }
}
