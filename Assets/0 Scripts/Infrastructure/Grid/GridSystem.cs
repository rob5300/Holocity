using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Infrastructure.Grid.Entities;
using Infrastructure.Grid.Entities.Buildings;
using Infrastructure.Residents;
using Infrastructure.Tick;
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
        public float AverageResidentHappiness = 0;
        public List<Resident> Residents;
        public EdgeGuidance edgeGuidance;
        public Sprite icon;

        /// <summary>
        /// A list of all tickables that will be ticked by the owning city and session tick manager.
        /// </summary>
        public ConcurrentQueue<Tickable> TickAddQueue;
        /// <summary>
        /// YET TO IMPLEMENT
        /// </summary>
        public ConcurrentQueue<Tickable> ToRemoveFromTickSystem;
        public event Action<Residential> OnNewResidentialBuilding;

        internal GridSystem(int width, int height, int id, City parentCity, Vector3 worldGridPosition)
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
                    Tiles[x][y] = new GridTile(new Vector2Int(x, y), this);
                }
            }

            TickAddQueue = new ConcurrentQueue<Tickable>();
            Residents = new List<Resident>();

            WorldGrid = CreateWorldGrid(worldGridPosition);
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
            //Make sure the given position is inbounds of the array, if not we return null.
            if ((position.x < Width && position.y < Height) && (position.x >= 0 && position.y >= 0)) return Tiles[position.x][position.y];
            return null;
        }

        private WorldGrid CreateWorldGrid(Vector3 position)
        {
            GameObject gridObject = new GameObject("World Grid: " + Id);
            gridObject.transform.position = position + new Vector3(0, 0.05f, 0);
            gridObject.AddComponent<WorldAnchor>();
            WorldGrid grid = gridObject.AddComponent<WorldGrid>();
            grid.Initialize(Id, Width, Height, this);

            return grid;
        }

        /// <summary>
        /// Adds this tile entity/building to this tile. Fails if the tile already has a building/entity.
        /// </summary>
        /// <param name="x">x position.</param>
        /// <param name="y">y position.</param>
        /// <param name="building">The building to add to the tile.</param>
        /// <returns>If the operation was successful.</returns>
        public bool AddTileEntityToTile(int x, int y, TileEntity tileEnt)
        {
            GridTile tile = GetTile(new Vector2Int(x, y));
            if (tile.Entity != null) return false;
            tile.SetEntity(this, tileEnt);
            AddTileEntityToWorldGrid(tileEnt, x, y);
            //We check if this is Tickable, if soo we add this to the tick manager in our session.
            if(tileEnt is Tickable)
            {
                TickAddQueue.Enqueue((Tickable)tileEnt);
            }
            if(tileEnt is Residential)
            {
                //We call the event to let the city know that we have a new residential building.
                OnNewResidentialBuilding?.Invoke((Residential)tileEnt);
            }
            return true;
        }

        /// <summary>
        /// Adds this tile entity/building to this tile. Fails if the tile already has a building/entity.
        /// </summary>
        /// <param name="position">Position of tile</param>
        /// <param name="building">The building to add to the tile.</param>
        /// <returns>If the operation was successful.</returns>
        public bool AddTileEntityToTile(Vector2Int position, TileEntity tileEnt)
        {
            return AddTileEntityToTile(position.x, position.y, tileEnt);
        }

        /// <summary>
        /// Swap the tile entities between the two tiles.
        /// </summary>
        /// <param name="tileaposition">First tile position.</param>
        /// <param name="tilebposition">Second tile position.</param>
        /// <returns>If the operation was successful or not.</returns>
        public bool SwapTileEntities(Vector2Int tileaposition, Vector2Int tilebposition)
        {
            GridTile tileA = GetTile(tileaposition);
            GridTile tileB = GetTile(tilebposition);

            //Return false if both are null or if any cannot be moved.
            //Have to do the null check incase one of the entities is null.
            if ((tileA.Entity == null && tileB.Entity == null) ||
                (tileA.Entity != null && !tileA.Entity.CanBeMoved) || (tileB.Entity != null && !tileB.Entity.CanBeMoved)
                ) return false;

            //Store the tile entity of a.
            TileEntity aEntity = tileA.Entity;
            //Swap the entities.
            tileA.SetEntityFromSwap(tileB.Entity);
            tileB.SetEntityFromSwap(aEntity);
            //Swap the entities on the WorldGrid:
            WorldGrid.SwapGridTiles(tileaposition, tilebposition);

            return true;
        }

        private void AddTileEntityToWorldGrid(TileEntity tileEnt, int x, int y)
        {
            WorldGrid.GetTile(x,y).AddModelToTile(tileEnt);
        }

        public float UpdateHappiness()
        {
            //Get an average of all residents happinesses levels.
            float totalhappiness = 0;
            foreach(Resident res in Residents)
            {
                totalhappiness += res.Happiness.Level;
            }
            AverageResidentHappiness = totalhappiness / Residents.Count;
            return AverageResidentHappiness;
        }
    }
}
