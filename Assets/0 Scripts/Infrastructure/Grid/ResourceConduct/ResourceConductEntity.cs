using CityResources;
using GridSearch;
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
        private List<Task<bool>> resourcePathCheckTasks = new List<Task<bool>>();
        private List<ResourceData> resourceListForConnetionCheck;
        private HashSet<GridTile> tilesToIgnoreDestroy;

        private bool _shouldContinueDestroyInform = false;
        private List<ResourceData> _resourcesToRemove_inform;
        private HashSet<GridTile> _tilesToIgnore_destroyInform;

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
            foreach (GridTile tile in ParentTile.GetAdjacentGridTiles())
            {
                if (tile == null) continue;
                ResourceConductEntity rEntity = tile.Entity as ResourceConductEntity;
                if (rEntity != null)
                {
                    if (CompareAndGetNewResources(rEntity.CurrentResources)) anyNewResources = true;
                }
            }

            if (anyNewResources)
            {
                //Inform neighbours of our new resources.
                //Get a new list with all the resources we have.
                //We want to send this list onwards via a resource inform.
                if (shouldInformNeighboursOfNewResources) BeginResourceInform(GetResourcesAsList());
            }
            return anyNewResources;
        }

        #region Resource Informining Methods.
        public void BeginResourceInform(List<ResourceData> resourceDatas)
        {
            //We are the first object to start this process.
            //We create a new hashset of tiles to ignore and start with adding ourselves.
            HashSet<GridTile> tilesToIgnore = new HashSet<GridTile>();
            tilesToIgnore.Add(ParentTile);

            foreach (GridTile tile in ParentTile.GetAdjacentGridTiles())
            {
                if (tile == null) continue;

                //If this tile has a resource conduct entity, we will inform it of the resources
                ResourceConductEntity cEntity = tile?.Entity as ResourceConductEntity;
                if (cEntity != null)
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
                if (AddNewResource(resourceData[i].resource.GetType(), resourceData[i]))
                {
                    newResources++;
                }
            }
            if (newResources > 0)
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
        #endregion

        #region Resource Lost & Neighbour Destroyed Methods.
        public override void OnDestroy()
        {
            HashSet<GridTile> tilesToIgnore = new HashSet<GridTile>();
            tilesToIgnore.Add(ParentTile);

            //Inform neighbours that we were destroyed
            GridTile[] tiles = ParentTile.GetAdjacentGridTiles();
            foreach (GridTile tile in tiles)
            {
                if (tile != null && tile.Entity != null)
                {
                    ResourceConductEntity ent = tile.Entity as ResourceConductEntity;

                    //Get a new node set for the grid we live on
                    //Manually change the tile we were on to not be populated anymore.
                    Node[,] nodeSet = ParentTile.ParentGridSystem.GetNodeSet();
                    nodeSet[ParentTile.Position.x, ParentTile.Position.y].Populated = false;
                    ent?.OnNeighbourDestroyed(this, nodeSet, tilesToIgnore);
                }
            }
        }

        public virtual void OnNeighbourDestroyed(TileEntity neighbour, Node[,] nodeSet, HashSet<GridTile> tilesToIgnore)
        {
            //Check if we can access any of our resources still by direct connection.
            //We shall set off a task to check for a connection for all of our resources and await the results on our tick.
            resourcePathCheckTasks.Clear();
            resourceListForConnetionCheck = GetResourcesAsList();
            tilesToIgnore.Add(ParentTile);
            tilesToIgnoreDestroy = tilesToIgnore;

            foreach (ResourceData resource in resourceListForConnetionCheck)
            {
                Func<bool> newFunc = () => { return new GridPathSearcher(nodeSet, ParentTile.Position, resource.entityOrigin.ParentTile.Position, ParentTile.ParentGridSystem.Width).Start(); };
                resourcePathCheckTasks.Add(Task.Run(newFunc));
            }
        }

        public virtual void BeginInformNoConnectionResource(List<ResourceData> resourceToRemove)
        {
            foreach (GridTile tile in ParentTile.GetAdjacentGridTiles())
            {
                if (tile == null) continue;

                //If this tile has a resource conduct entity, we will inform it of the resources to remove.
                ResourceConductEntity cEntity = tile?.Entity as ResourceConductEntity;
                if (cEntity != null)
                {
                    cEntity.ResourceLostConnection_Event(resourceToRemove, tilesToIgnoreDestroy);
                }
            }
        }

        public virtual void ContinueNoConnectionResourceInform(List<ResourceData> resourcesToRemove, HashSet<GridTile> tilesToIgnore)
        {
            //Tell our neighbours that these resource has no connection anymore.
            GridTile[] tiles = ParentTile.GetAdjacentGridTiles();
            foreach (GridTile tile in tiles)
            {
                if (tile == null) continue;
                //Skip this tile if it was informed already from this operation queue.
                if (tilesToIgnore.Contains(tile)) continue;
                if (tile != null && tile.Entity != null)
                {
                    ResourceConductEntity ent = tile.Entity as ResourceConductEntity;
                    ent?.ResourceLostConnection_Event(resourcesToRemove, tilesToIgnore);
                }
            }
        }

        public virtual void ResourceLostConnection_Event(List<ResourceData> resourcesToRemove, HashSet<GridTile> tilesToIgnore)
        {
            //We are being informed that resources need to be removed due to a loss of connection.
            tilesToIgnore.Add(ParentTile);

            foreach(ResourceData resource in resourcesToRemove)
            {
                if (CurrentResources[resource.resource.GetType()].Contains(resource))
                {
                    CurrentResources[resource.resource.GetType()].Remove(resource);
                }
            }

            //Setup to continue the destroy connection inform next tick
            _resourcesToRemove_inform = resourcesToRemove;
            _tilesToIgnore_destroyInform = tilesToIgnore;
            _shouldContinueDestroyInform = true;
        }
        #endregion

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

            //Check if we have any tasks running that are checking for resource connections.
            //If any are complete then we can take the results and act on them.
            if(resourcePathCheckTasks.Count > 0)
            {
                List<ResourceData> resourcesToRemove = new List<ResourceData>();

                for (int i = resourcePathCheckTasks.Count - 1; i >= 0; i--)
                {
                    if (resourcePathCheckTasks[i].IsCompleted)
                    {
                        if (!resourcePathCheckTasks[i].Result)
                        {
                            //This resource path check failed to get a route. Add this to the list to inform with.
                            resourcesToRemove.Add(resourceListForConnetionCheck[i]);
                            //Remove the resourse from ourselves
                            CurrentResources[resourceListForConnetionCheck[i].resource.GetType()].Remove(resourceListForConnetionCheck[i]);
                        }
                        //Task completed, remove it from the list
                        resourcePathCheckTasks.RemoveAt(i);
                        if (resourcePathCheckTasks.Count == 0) break;
                    }
                }
                if(resourcesToRemove.Count != 0) BeginInformNoConnectionResource(resourcesToRemove);
            }

            //Check if we should continue to inform about a resource to remove from a destruction
            if (_shouldContinueDestroyInform)
            {
                _shouldContinueDestroyInform = false;
                ContinueNoConnectionResourceInform(_resourcesToRemove_inform, _tilesToIgnore_destroyInform);
                _resourcesToRemove_inform = null;
                _tilesToIgnore_destroyInform = null;
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
