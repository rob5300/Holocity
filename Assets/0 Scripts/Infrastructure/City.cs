using Resources;
using System.Collections.Generic;

namespace Infrastructure {
    public class City {

        public string Name;
        public string Owner;
        
        protected Dictionary<System.Type, Resource> Resources = new Dictionary<System.Type, Resource>();

        public City(string name, string owner)
        {
            name = name != string.Empty ? name : System.DateTime.Now.ToShortTimeString() + ":" + System.DateTime.Now.ToShortDateString();
            owner = owner != string.Empty ? owner : "Mayor";

        }

        public Resource GetResource<T>() where T : Resource {
            if (Resources.ContainsKey(typeof(T))) return Resources[typeof(T)];
            else return null;
        }

    }
}
