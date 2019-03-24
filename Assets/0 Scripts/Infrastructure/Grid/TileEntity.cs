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
        protected void OccupyTiles(bool[,] requiredTiles)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    //Skip relative 0,0 as that is us already.
                    if (x - 1 == 0 && y - 1 == 0) continue;
                    //If this tile offset should be occupied.
                    if (requiredTiles[x, y])
                    {
                        Vector2Int checkPosOffset = new Vector2Int(x - 1, y - 1);
                        GridTile tile = ParentTile.ParentGridSystem.GetTile(ParentTile.Position + checkPosOffset);
                        tile.MultiTileOccupier = this;
                        ParentTile.ParentGridSystem.WorldGrid.GetTile(ParentTile.Position + checkPosOffset).TileBorder.SetActive(false);
                    }
                }
            }
        }
    }
}