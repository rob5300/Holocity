using Infrastructure;
using Infrastructure.Grid.Entities.Buildings;
using UnityEngine;
using UnityEngine.XR.WSA;

namespace Infrastructure.Grid
{
    public class GridSystem
    {
        public int Id = 0;
        public City ParentCity;
        public WorldGrid WorldGrid;
        public Vector3 Position { get { return WorldGrid.transform.position; } }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public GridTile[][] Tiles;

        internal GridSystem(int width, int height, int id, City parentCity)
        {
            Id = id;
            ParentCity = parentCity;
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

            WorldGrid = CreateWorldGrid();
        }
        
        //private static GameObject GetDefaultAnchor(int Id)
        //{
        //    GameObject ob = new GameObject("Grid Anchor: " + Id);
        //    ob.transform.position = Vector3.zero;
        //    ob.AddComponent<WorldAnchor>();
        //    return ob;
        //}

        public GridTile GetTile(Vector2Int position)
        {
            if (position.x <= Width && position.y <= Height) return Tiles[position.x][position.y];
            return null;
        }

        public WorldGrid CreateWorldGrid()
        {
            GameObject gridObject = new GameObject("World Grid: " + Id);
            WorldGrid grid = gridObject.AddComponent<WorldGrid>();
            grid.Initialize(Width, Height);
            return grid;
        }

        public bool AddBuildingToTile(int x, int y, Building building)
        {
            GridTile tile = GetTile(new Vector2Int(x, y));
            tile.SetEntity(this, building);
            AddBuildingtoWorldGrid(building, x, y);
            return true;
        }

        private void AddBuildingtoWorldGrid(Building building, int x, int y)
        {
            WorldGridTile worldGridTile = WorldGrid.GetTile(x,y);
            worldGridTile.TileBorder.SetActive(false);
            worldGridTile.Model = Object.Instantiate(building.BuildingPrefab, worldGridTile.transform);
        }
    }
}
