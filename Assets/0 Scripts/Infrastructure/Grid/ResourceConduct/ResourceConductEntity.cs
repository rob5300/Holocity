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

        /// <summary>
        /// Look for new resources on neighbours.
        /// </summary>
        /// <returns>If there were any new resources found.</returns>
        public bool LookForResources(bool shouldInformNeighboursOfNewResources = true)
        {
            //We were just placed, lets look for resources on our neighbours.
            bool anyNewResources = false;
            foreach(GridTile tile in ParentTile.GetAdjacentGridTiles())
            {
                if (tile == null) continue;
                ResourceConductEntity rEntity = tile.Entity as ResourceConductEntity;
                if(rEntity != null)
                {
                    if (CompareAndGetNewResources(rEntity.CurrentResources)) anyNewResources = true;
                }
            }

            if (anyNewResources)
            {
                //Inform neighbours of our new resources.
                //Get a new list with all the resources we have.
                //We want to send this list onwards via a resource inform.
                if(shouldInformNeighboursOfNewResources) BeginResourceInform(GetResourcesAsList());
            }
            return anyNewResources;
        }

        public void BeginResourceInform(List<ResourceData> resourceDatas)
        {
            //We are the first object to start this process.
            //We create a new hashset of tiles to ignore and start with adding ourselves.
            HashSet<GridTile> tilesToIgnore = new HashSet<GridTile>();
            tilesToIgnore.Add(ParentTile);

            foreach(GridTile tile in ParentTile.GetAdjacentGridTiles())
            {
                if (tile == null) continue;

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
            int newResources = 0;
            for (int i = resourceData.Count - 1; i >= 0; i--)
            {
                //If this was a new resource then we keep track.
                if(AddNewResource(resourceData[i].resource.GetType(), resourceData[i]))
                {
                    newResources++;
                }
            }
            if(newResources > 0)
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
                if (tile == null) continue;
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

        public override void OnEntityProduced(GridSystem grid)
        {
            //Perform setup tasks.
            if (!LookForResources())
            {
                //If there were no new resources to get, inform about our resources.
                BeginResourceInform(GetResourcesAsList());
            }
        }

        /// <summary>
        /// Add the resource to our resources list.
        /// </summary>
        /// <param name="rType">Type of the resource</param>
        /// <param name="resourceData">The new resource data</param>
        /// <returns>If this was a new resource and not a duplicate</returns>
        protected bool AddNewResource(Type rType, ResourceData resourceData)
        {
            //If the list for this resource type doesnt exist, make it.
            if (!CurrentResources.ContainsKey(rType)) CurrentResources.Add(rType, new List<ResourceData>());
            //If this resource list already has this resource, do not add it. Return false to notify that this was not a new resource.
            if (CurrentResources[rType].Contains(resourceData)) return false;
            else
            {
                //This was a new resource, add it and return true.
                ParentTile.ParentGridSystem.AddResourceReference(resourceData);
                CurrentResources[rType].Add(resourceData);
            }
            return true;
        }

        /// <summary>
        /// Compares the resources given from another <see cref="ResourceConductEntity"/> and takes them for ourselves.
        /// </summary>
        /// <param name="listOfResource"></param>
        /// <returns></returns>
        private bool CompareAndGetNewResources(Dictionary<Type, List<ResourceData>> listOfResource)
        {
            bool anyNewResources = false;
            if (listOfResource.Count == 0) return false;
            //Itterate through all the Lists for each resource type.
            foreach(KeyValuePair<Type, List<ResourceData>> resourceDataList in listOfResource)
            {
                //Itterate through each resource in this list and try to add it to our list.
                foreach(ResourceData resourceData in resourceDataList.Value)
                {
                    //if this was a new resource anyNewResources will now be true.
                    if (AddNewResource(resourceDataList.Key, resourceData)) anyNewResources = true;
                }
            }
            return anyNewResources;
        }

        private List<ResourceData> GetResourcesAsList()
        {
            List<ResourceData> resouceDataToInformWith = new List<ResourceData>();
            foreach (List<ResourceData> resourceLists in CurrentResources.Values)
            {
                resouceDataToInformWith.AddRange(resourceLists);
            }
            return resouceDataToInformWith;
        }
    }

}
