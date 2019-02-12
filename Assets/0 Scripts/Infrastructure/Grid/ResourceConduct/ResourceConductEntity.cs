using CityResources;
using System;
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities
{
    /// <summary>
    /// A TileEntity that is capible of conducting resources via direct contact.
    /// </summary>
    public class ResourceConductEntity : TileEntity
    {
        public bool CanConduct = true;
        public ResourceReferenceManager ResourceReferenceManager;

        public ResourceConductEntity()
        {
            
        }

        public override void OnEntityProduced(GridSystem grid)
        {
            //Check neighbouring tile entities for conducting entities and get the reference to the manager.
            //If there is not one, we make a new one as we assume we have no connections and need to establish a new one.
            //Its important to check if there are more than one and if these are different.

            GridTile[] tiles = ParentTile.GetAdjacentGridTiles();
            List<ResourceReferenceManager> foundManagers = new List<ResourceReferenceManager>(4);
            ResourceReferenceManager managerToUse;

            foreach(GridTile tile in tiles)
            {
                if(tile.Entity != null && tile.Entity is ResourceConductEntity)
                {
                    ResourceReferenceManager foundRef = ((ResourceConductEntity)tile.Entity).ResourceReferenceManager;
                    //If this list doesnt have this manager already, add it. We only want to store each unique manager found.
                    if (!foundManagers.Contains(foundRef)) foundManagers.Add(foundRef);
                }
            }
            //If we have found any managers.
            if(foundManagers.Count > 0)
            {
                //Is there only one manager?
                if(foundManagers.Count == 1)
                {
                    //Assign this manager ref
                    managerToUse = foundManagers[0];
                }
                //There are 2 or more managers.
                else
                {
                    //We need to resolve these managers and combine them.
                    //Lets choose a manager to keep and assign at the end.
                    managerToUse = foundManagers[0];
                    for (int i = 1; i < foundManagers.Count; i++)
                    {
                        if (foundManagers[i].Users > managerToUse.Users) managerToUse = foundManagers[i];
                    }
                    //We now have chosen the manager with the most users, this tries to keep the event calls to a minimum.

                    foundManagers.Remove(managerToUse);
                    managerToUse.ResolveAndMergeInManagers(foundManagers);
                }
            }
            else
            {
                //We found no managers, lets request one and add the reference to ourselves.
                managerToUse = ResourceReferenceManager.GetNewManager();
            }

            ResourceReferenceManager = managerToUse;
            ResourceReferenceManager.AddUser();
            ResourceReferenceManager.ManagerObsolete += ResourceReferenceManager_ManagerObsolete;
        }


        private void ResourceReferenceManager_ManagerObsolete(ResourceReferenceManager newMan)
        {
            //Remove ourselves from the old manager and sub to the new one.
            ResourceReferenceManager.RemoveUser();
            ResourceReferenceManager.ManagerObsolete -= ResourceReferenceManager_ManagerObsolete;
            //New manager.
            ResourceReferenceManager = newMan;
            ResourceReferenceManager.AddUser();
            ResourceReferenceManager.ManagerObsolete += ResourceReferenceManager_ManagerObsolete;
        }
    }
}
