using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GridSearch;
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
        public Sprite icon;

        public event Action<Residential> OnNewResidentialBuilding;
        public event Action<Commercial> OnNewCommercialBuilding;

        private Dictionary<Type, List<ResourceData>> _gridResources;
        private TickManager _tickManager;
        

        internal GridSystem(int width, int height, int id, City parentCity, TickManager tickMan, Vector3 worldGridPosition)
        {
            Id = id;
            ParentCity = parentCity;
            Width = width;
            Height = height;
            _tickManager = tickMan;

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

            Residents = new List<Resident>();
            _gridResources = new Dictionary<Type, List<ResourceData>>();

            WorldGrid = CreateWorldGrid(worldGridPosition);
        }
        
        //private static GameObject GetDefaultAnchor(int Id)
        //{
        //    GameObject ob = new GameObject("Grid Anchor: " + Id);
        //    ob.transform.position = Vector3.zero;
        //    ob.AddComponent<WorldAnchor>();
        //    return ob;
        //}


        public List<Vector3Int> GetTilesForSaving()
        {
            List<Vector3Int> tiles = new List<Vector3Int>();

            for (int i =0; i < Width; i++)
            {
                for(int j = 0; j < Height; j++)
                {
                    GridTile tile = GetTile(new Vector2Int(i, j));

                    if (tile.Entity != null)
                    {
                        int index = BuildingLibrary.SetupModernBuildings.FindIndex(x => x == tile.Entity.GetType());
                        Vector3Int ent = new Vector3Int(tile.Position.x, tile.Position.y, index);
                        tiles.Add(ent);

                    }

                }

            }

            return tiles;
            
        }
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
            if (tileEnt is ITickable)
            {
                _tickManager.IncomingTickableQueue.Enqueue((ITickable)tileEnt);
            }
            if (tileEnt is Residential)
            {
                //We call the event to let the city know that we have a new residential building.
                OnNewResidentialBuilding?.Invoke((Residential)tileEnt);
            }
            if (tileEnt is Commercial)
            {
                OnNewCommercialBuilding?.Invoke((Commercial)tileEnt);
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

        public void DestroyTileEntity(Vector2Int position)
        {
            GridTile tile =  GetTile(position);
            if (tile.Entity != null) tile.Entity.OnDestroy();
            ITickable eTick = tile.Entity as ITickable;
            if (eTick != null) eTick.ShouldBeRemoved = true;
            WorldGrid.GetTile(position).RemoveModel();
            tile.DestroyEntity();
        }

        /// <summary>
        /// Query if this type can be placed in this position.
        /// </summary>
        /// <param name="type">Type of the tile entity</param>
        /// <param name="TargetPosition">Target tile position</param>
        /// <returns>If the placement can take place</returns>
        public bool QueryPlaceByType(Type type, Vector2Int TargetPosition)
        {
            //Try to get the TileEntityMeta attribute
            TileEntityMetaAttribute meta = Attribute.GetCustomAttribute(type, typeof(TileEntityMetaAttribute)) as TileEntityMetaAttribute;
            if(meta != null)
            {
                bool[,] requiredTiles = meta.RequiredTiles;
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        //If this tile offset should be occupied.
                        if (requiredTiles[y, x])
                        {
                            Vector2Int checkPosOffset = new Vector2Int(x - 1, y - 1);
                            //If this tile is occupied, return false.
                            GridTile tile = GetTile(TargetPosition + checkPosOffset);
                            if (tile == null || tile.Occipied) return false;
                        }
                    }
                }
            }
            //We can place as no checked positions were occupied.
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
            if (Residents.Count != 0) AverageResidentHappiness = totalhappiness / Residents.Count;
            else AverageResidentHappiness = 0;
            return AverageResidentHappiness;
        }

        public List<ResourceData> GetResourceList(Type type)
        {
            if (_gridResources.ContainsKey(type))
            {
                return _gridResources[type];
            }
            return null;
        }

        public void ResetResourceTickCounters()
        {
            foreach(List<ResourceData> resourceList in _gridResources.Values)
            {
                foreach(ResourceData data in resourceList)
                {
                    data.resource.ResetCounters();
                }
            }
        }

        public void AddResourceReference(ResourceData data)
        {
            //Add a new list for the type if needed.
            if (!_gridResources.ContainsKey(data.resource.GetType())) _gridResources.Add(data.resource.GetType(), new List<ResourceData>());
            //If this list doesnt already have this data, add it.
            if(!_gridResources[data.resource.GetType()].Contains(data)) _gridResources[data.resource.GetType()].Add(data);
        }

        public Node[,] GetNodeSet()
        {
            Node[,] nodeset = new Node[Width, Height];
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    nodeset[w, h] = new Node(Tiles[w][h]);
                }
            }
            return nodeset;
        }

    }
}
