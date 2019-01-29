using Infrastructure;
using System;
using UnityEngine;

namespace Infrastructure.Grid.Entities
{
    public class TileEntity
    {
        public bool CanBeMoved = true;
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

        public virtual void OnEntityProduced(GridSystem grid)
        {

        }

        public virtual bool QueryPlace(Grid.GridTile tile)
        {
            return true;
        }

        public virtual void OnWorldGridTileCreated(WorldGridTile tile)
        {

        }

        public virtual void OnInteract()
        {

        }

        public virtual void OnDestroy()
        {

        }

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
    }
}