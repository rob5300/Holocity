using Infrastructure;
using System;
using UnityEngine;

namespace Infrastructure.Grid.Entities
{
    public class TileEntity
    {
        /// <summary>
        /// Can this entity be moved?
        /// </summary>
        public bool CanBeMoved = true;
        /// <summary>
        /// Can this entity be rotated?
        /// </summary>
        public bool AllowRotation = true;
        public GridTile ParentTile;
        public uint Cost = 0;

        /// <summary>
        /// Name of the prefab to get in the corrisponding Resources folder.
        /// </summary>
        public string PrefabName;
        /// <summary>
        /// Path to resources folder to look for model within.
        /// </summary>
        public string ResourcesFolderPath = "";

        [NonSerialized]
        private GameObject _tilePrefab;

        public virtual void OnEntityProduced(GridSystem grid) { }

        public virtual bool QueryPlace(GridTile tile, WorldGrid grid)
        {
            return true;
        }

        public virtual void OnWorldGridTileCreated(WorldGridTile tile) { }

        public virtual void OnInteract() { }

        public virtual void OnDestroy() { }

        public virtual void OnMoveStart() { }

        public virtual void OnMoveCancelled() { }

        public virtual void OnMoveComplete() { }

        public virtual GameObject GetModel()
        {
            if (_tilePrefab == null)
            {
                string pathtoob = ResourcesFolderPath + PrefabName;
                _tilePrefab = Resources.Load<GameObject>(pathtoob);
#if UNITY_EDITOR
                if (_tilePrefab == null) Debug.LogError("Path was invalid! " + pathtoob);
#endif
            }
            return _tilePrefab;
        }

        /// <summary>
        /// Occupy other tiles that this entity will overlap over.
        /// </summary>
        /// <param name="requiredTiles"></param>
        protected void OccupyTiles(bool[,] requiredTiles, bool occupy = true)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    //Skip relative 0,0 as that is us already.
                    if (x - 1 == 0 && y - 1 == 0) continue;
                    //If this tile offset should be occupied.
                    if (requiredTiles[y, x])
                    {
                        Vector2Int checkPosOffset = new Vector2Int(x - 1, y - 1);
                        GridTile tile = ParentTile.ParentGridSystem.GetTile(ParentTile.Position + checkPosOffset);
                        tile.MultiTileOccupier = occupy ? this : null;
                        ParentTile.ParentGridSystem.WorldGrid.GetTile(ParentTile.Position + checkPosOffset).TileBorder.SetActive(!occupy);
                    }
                }
            }
        }

        protected void UnOccupyTiles(bool[,] requiredTiles)
        {
            OccupyTiles(requiredTiles, false);
        }

        /// <summary>
        /// Get adjacent grid tiles. Can return null in the array if a tile doesn't exist.
        /// </summary>
        /// <returns>An array of adjacent grid tiles. Index is null if it did not exist.</returns>
        protected virtual GridTile[] GetAdjacentGridTiles()
        {
            GridSystem sys = ParentTile.ParentGridSystem;
            Vector2Int position = ParentTile.Position;
            GridTile[] adjacentTiles = {
                sys.GetTile(position + new Vector2Int(0, 1)),
                sys.GetTile(position + new Vector2Int(1, 0)),
                sys.GetTile(position + new Vector2Int(0, -1)),
                sys.GetTile(position + new Vector2Int(-1, 0))
            };
            return adjacentTiles;
        }
    }
}