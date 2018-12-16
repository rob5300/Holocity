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
            CapValue = false;
        }

        public int Value { get; private set; }
        public bool CapValue { get; private set; }
        public int ValueCap;

        //Changes for last tick
        public int AddedLastTick { get; private set; }
        public int RecievedLastTick { get; private set; }
        //Changes for this tick.
        public int AddedThisTick { get; private set; }
        public int RecievedThisTick { get; private set; }

        public void Add(int amount)
        {
            Value += amount;
            AddedThisTick += amount;
        }

        /// <summary>
        /// Request an amount of this resource. Returns what is avaliable.
        /// </summary>
        /// <param name="amount">How many of this resource to request.</param>
        /// <returns>Part or whole of resources requested.</returns>
        public int Recieve(int amount)
        {
            int toReturn;
            if (Value >= amount) toReturn = amount;
            else toReturn = Value;

            Value -= toReturn;
            RecievedThisTick -= toReturn;

            return toReturn;
        }

        /// <summary>
        /// Reset recieved tick counters and update last tick values.
        /// </summary>
        public void ResetCounters()
        {
            AddedLastTick = AddedThisTick;
            AddedThisTick = 0;
            RecievedLastTick = RecievedThisTick;
            RecievedThisTick = 0;
        }
    }
}
