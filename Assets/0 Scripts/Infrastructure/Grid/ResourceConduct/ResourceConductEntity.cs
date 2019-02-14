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
        public Dictionary<Type, List<ResourceData>> Resources;

        public ResourceConductEntity()
        {
            NewResources = new List<ResourceData>();
            Resources = new Dictionary<Type, List<ResourceData>>();
        }

        public virtual void OnNewResourceViaConnection(GridTile source, ResourceData resourceData)
        {
            
        }

        public void InformNeighboursOfNewResource(ResourceData resourceData, HashSet<GridTile> tilesToIgnore)
        {
            InformNeighboursOfNewResource(new List<ResourceData> { resourceData }, tilesToIgnore);
        }

        public void InformNeighboursOfNewResource(List<ResourceData> resourceData, HashSet<GridTile> tilesToIgnore)
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
                    foreach (ResourceData data in resourceData)
                    {
                        ent?.OnNewResourceViaConnection(ParentTile, data);
                    }
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

        }
    }

}
