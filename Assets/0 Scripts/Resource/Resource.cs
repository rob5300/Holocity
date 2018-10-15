using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityResources
{
    public class Resource
    {
        public Resource()
        {

        }

        public int Avaliable { get; private set; }

        public void Add(int amount)
        {
            Avaliable += amount;
        }

        /// <summary>
        /// Request an amount of this resource. Returns what is avaliable.
        /// </summary>
        /// <param name="amount">How many of this resource to request.</param>
        /// <returns>Part or whole of resources requested.</returns>
        public int Recieve(int amount)
        {
            int toReturn;
            if (Avaliable >= amount) toReturn = amount;
            else toReturn = Avaliable;

            Avaliable -= toReturn;

            return toReturn;
        }
    }
}
