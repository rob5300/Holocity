using CityResources;
using Infrastructure.Tick;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Grid.Entities
{
    /// <summary>
    /// A TileEntity that is capible of conducting resources via direct contact as well as using them.
    /// </summary>
    public class ResourceConductEntity : TileEntity, Tickable
    {
        public bool CanConduct = true;
        public List<ResourceData> NewResources;
        public Dictionary<Type, List<ResourceData>> CurrentResources;

        private bool ShouldContinueResourceInform = false;
        private HashSet<GridTile> _tilesToIgnore;
        private List<ResourceData> _newResourceData;

        public ResourceConductEntity()
        {
            NewResources = new List<ResourceData>();
            CurrentResources = new Dictionary<Type, List<ResourceData>>();
        }

        public virtual void LookForResources()
        {
            //We were just placed, lets look for resources on our neighbours.
        }

        public void BeginResourceInform()
        {
            //We are the first object to start this process
        }

        public virtual void OnNewResourceViaConnection_Event(List<ResourceData> resourceData, HashSet<GridTile> tilesToIgnore)
        {
            //We are being told about a new resource by a neighbour. We are given the resource information as well as the tiles to ignore for when we pass this on.
            //At this level we will just pass the info on and not store any.
            tilesToIgnore.Add(ParentTile);
            foreach(ResourceData data in resourceData)
            {
                AddNewResource(data.GetType(), data);
            }
        }

        public void ContinueResourceInform(ResourceData resourceData, HashSet<GridTile> tilesToIgnore)
        {
            ContinueResourceInform(new List<ResourceData> { resourceData }, tilesToIgnore);
        }

        public void ContinueResourceInform(List<ResourceData> resourceData, HashSet<GridTile> tilesToIgnore)
        {
            //Tell our neighbours that this resource exists.
            GridTile[] tiles = ParentTile.GetAdjacentGridTiles();
            foreach (GridTile tile in tiles)
            {
                //Skip this tile if it was informed already from this operation queue.
                if (tilesToIgnore.Contains(tile)) continue;
                if (tile != null && tile.Entity != null)
                {
                    ResourceConductEntity ent = tile.Entity as ResourceConductEntity;
                    ent?.OnNewResourceViaConnection_Event(resourceData, tilesToIgnore);
                }
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            //Inform neighbours that we were destroyed
            GridTile[] tiles = ParentTile.GetAdjacentGridTiles();
            foreach(GridTile tile in tiles)
            {
                if(tile != null && tile.Entity != null)
                {
                    ResourceConductEntity ent = tile.Entity as ResourceConductEntity;
                    ent?.OnNeighbourDestroyed(this);
                }
            }
        }

        public virtual void OnNeighbourDestroyed(TileEntity neighbour)
        {

        }

        public virtual void Tick(float time)
        {
            //Check if we have new resources to tell neighbours about.
            //Here is where we will continue the resource inform process

            if (ShouldContinueResourceInform)
            {
                ShouldContinueResourceInform = false;

                ContinueResourceInform(_newResourceData, _tilesToIgnore);
            }
        }

        private void AddNewResource(Type rType, ResourceData resourceData)
        {
            //If the list for this resource type doesnt exist, make it.
            if (!CurrentResources.ContainsKey(rType)) CurrentResources.Add(rType, new List<ResourceData>());

            CurrentResources[rType].Add(resourceData);
        }
    }

}
