using Infrastructure.Grid.Entities;
using System;
using System.Collections.Generic;

namespace CityResources
{
    public class ResourceReferenceManager
    {
        internal static List<List<ResourceReferenceManager>> Managers = new List<List<ResourceReferenceManager>>();
        private static int idtally = 0;
        private static List<bool> ManagerListDirtys = new List<bool>();

        /// <summary>
        /// Creates and returns a new manager. ONLY to be used if the Entity could not locate any.
        /// </summary>
        /// <returns>New manager reference.</returns>
        public static ResourceReferenceManager GetNewManager(int gridID)
        {
            ResourceReferenceManager man = new ResourceReferenceManager(++idtally, gridID);
            if (Managers.Count < gridID)
            {
                Managers.Add(new List<ResourceReferenceManager>());
                if (ManagerListDirtys.Count < gridID) ManagerListDirtys.Add(true);
            }
            Managers[gridID].Add(man);
            ManagerListDirtys[gridID] = true;
            return man;
        }

        public static List<ResourceReferenceManager> GetManagersForGrid(int gridID)
        {
            return Managers[gridID];
        }

        public int Users { get; private set; }
        /// <summary>
        /// Unique ID for this manager. Read Only.
        /// </summary>
        public readonly int ID;
        public readonly int GridID;
        public event Action<ResourceReferenceManager> ManagerObsolete;

        internal Dictionary<Type, Resource> _resources = new Dictionary<Type, Resource>();

        private ResourceReferenceManager(int id, int gridID)
        {
            Users = 1;
            ID = id;
            GridID = gridID;
        }

        public bool AddResource<T>(T origin) where T : Resource
        {
            Type rType = typeof(T);
            //Add this resource. If this exists already then return false.
            if (!HaveResourceConnection<T>())
            {
                //We can add. Lets do that then return true.
                _resources.Add(rType, origin);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns if this resource connection exists or not.
        /// </summary>
        /// <typeparam name="T">Resource type to check</typeparam>
        /// <returns>If the Resource is present</returns>
        public bool HaveResourceConnection<T>() where T : Resource
        {
            //Check if we have the resource.
            Type rType = typeof(T);
            return HaveResourceConnection(rType);
        }

        public bool HaveResourceConnection(Type rType)
        {
            if (_resources.ContainsKey(rType))
            {
                //If this resource is not null then we can return true. If it was null we remove it and return false.
                if (_resources[rType] == null)
                {
                    _resources.Remove(rType);
                    return false;
                }
                else return true;
            }
            return false;
        }

        public T GetResource<T>() where T : Resource
        {
            return (T)GetResource(typeof(T));
        }

        public Resource GetResource(Type rType)
        {
            if (HaveResourceConnection(rType)) return _resources[rType];
            else return null;
        }

        public void AddUser()
        {
            Users++;
        }

        public void RemoveUser()
        {
            Users--;
            if(Users < 1)
            {
                //We have no users anymore, remove ourselves from the list.
                Managers[GridID].Remove(this); //This should let us be GC'd. (Sorry hololens ;( )
            }
        }

        /// <summary>
        /// Take missing resources from provided managers and move them into this manager
        /// </summary>
        /// <param name="managerstoMergeIn"></param>
        public void ResolveAndMergeInManagers(List<ResourceReferenceManager> managerstoMergeIn)
        {
            foreach(ResourceReferenceManager manToMerge in Managers[GridID])
            {
                //Go through all the resources in this merge target
                foreach(KeyValuePair<Type, Resource> r in manToMerge._resources)
                {
                    //We need to merge two ways.
                    //1. Take the whole resource object and add it if we dont have this resource here.
                    //2. If we have the resource type and a merger also has it, we need to combine the resources avaliable together.
                    //3. We have it but the merger doesnt, nothing to do.
                    if (!HaveResourceConnection(r.Key)) _resources.Add(r.Key, r.Value);
                    else
                    {
                        //Take the value and add it to this resources value.
                        Resource ourR = GetResource(r.Key);
                        ourR.AddFromMerge(r.Value.Value);
                    }
                }

                //Tell the manager to call its Obsolete event.
                manToMerge.FireObsoleteEvent(this);
            }
        }

        public void FireObsoleteEvent(ResourceReferenceManager newManager)
        {
            ManagerObsolete?.Invoke(newManager);
        }

    }
}
