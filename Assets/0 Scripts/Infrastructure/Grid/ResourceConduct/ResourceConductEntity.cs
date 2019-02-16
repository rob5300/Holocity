using CityResources;
using Infrastructure.Tick;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        //New resource informing members
        private bool _shouldContinueResourceInform = false;
        private HashSet<GridTile> _tilesToIgnore_continueInform;
        private List<ResourceData> _newResourceData_continueInform;

        //Destroy check members
        public List<Task<bool>> ResourcePathCheckTasks = new List<Task<bool>>();

        public ResourceConductEntity()
        {
            NewResources = new List<ResourceData>();
            CurrentResources = new Dictionary<Type, List<ResourceData>>();
        }

        public virtual void LookForResources()
        {
            //We were just placed, lets look for resources on our neighbours.
        }

        public void BeginResourceInform(List<ResourceData> resourceDatas)
        {
            //We are the first object to start this process.
            //We create a new hashset of tiles to ignore and start with adding ourselves.
            HashSet<GridTile> tilesToIgnore = new HashSet<GridTile>();
            tilesToIgnore.Add(ParentTile);

            foreach(GridTile tile in ParentTile.GetAdjacentGridTiles())
            {
                //If this tile has a resource conduct entity, we will inform it of the resources
                ResourceConductEntity cEntity = tile?.Entity as ResourceConductEntity;
                if(cEntity != null)
                {
                    cEntity.OnNewResourceViaConnection_Event(resourceDatas, tilesToIgnore);
                }
            }
        }

        public virtual void OnNewResourceViaConnection_Event(List<ResourceData> resourceData, HashSet<GridTile> tilesToIgnore)
        {
            //We are being told about a new resource by a neighbour. We are given the resource information as well as the tiles to ignore for when we pass this on.
            //At this level we will just pass the info on and not store any.
            tilesToIgnore.Add(ParentTile);
            for (int i = resourceData.Count - 1; i > 0; i--)
            {
                //If this was not a new resource then we need to remove it from the data list.
                //This is soo we don't inform neighbours of a resource they should already have.
                if(!AddNewResource(resourceData[i].resource.GetType(), resourceData[i]))
                {
                    resourceData.RemoveAt(i);
                }
            }
            if(resourceData.Count > 1)
            {
                //We have resources that were new, inform neighbours of these.
                _newResourceData_continueInform = resourceData;
                _tilesToIgnore_continueInform = tilesToIgnore;
                _shouldContinueResourceInform = true;
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

        public virtual void OnLostResourceConnection_Event(List<ResourceData> resourcesLost, HashSet<GridTile> tilesToIgnore)
        {

        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            HashSet<GridTile> tilesToIgnore = new HashSet<GridTile>();
            tilesToIgnore.Add(ParentTile);

            //Inform neighbours that we were destroyed
            GridTile[] tiles = ParentTile.GetAdjacentGridTiles();
            foreach(GridTile tile in tiles)
            {
                if(tile != null && tile.Entity != null)
                {
                    ResourceConductEntity ent = tile.Entity as ResourceConductEntity;
                    ent?.OnNeighbourDestroyed(this, tilesToIgnore);
                }
            }
        }

        public virtual void OnNeighbourDestroyed(TileEntity neighbour, HashSet<GridTile> tilesToIgnore)
        {

        }

        public virtual void Tick(float time)
        {
            //Check if we have new resources to tell neighbours about.
            //Here is where we will continue the resource inform process

            if (_shouldContinueResourceInform)
            {
                _shouldContinueResourceInform = false;

                ContinueResourceInform(_newResourceData_continueInform, _tilesToIgnore_continueInform);
                _newResourceData_continueInform = null;
                _tilesToIgnore_continueInform = null;
            }


        }

        /// <summary>
        /// Add the resource to our resources list.
        /// </summary>
        /// <param name="rType">Type of the resource</param>
        /// <param name="resourceData">The new resource data</param>
        /// <returns>If this was a new resource and not a duplicate</returns>
        private bool AddNewResource(Type rType, ResourceData resourceData)
        {
            //If the list for this resource type doesnt exist, make it.
            if (!CurrentResources.ContainsKey(rType)) CurrentResources.Add(rType, new List<ResourceData>());
            //If this resource list already has this resource, do not add it. Return false to notify that this was not a new resource.
            if (CurrentResources[rType].Contains(resourceData)) return false;
            //This was a new resource, add it and return true.
            else CurrentResources[rType].Add(resourceData);
            return true;
        }
    }

}
