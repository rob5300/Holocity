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
            Value = 0;
        }

        public float Value { get; private set; }
        public bool CapValue { get; protected set; }
        public float ValueCap;

        //Changes for last tick
        public float AddedLastTick { get; private set; }
        public float RecievedLastTick { get; private set; }
        //Changes for this tick.
        public float AddedThisTick { get; private set; }
        public float RecievedThisTick { get; private set; }

        public float TimeoutTime { get; protected set; }

        public void Add(float amount)
        {
            Value += amount;
            if (CapValue && Value > ValueCap) Value = ValueCap;
            AddedThisTick += amount;
        }

        /// <summary>
        /// Request an amount of this resource. Returns what is avaliable.
        /// </summary>
        /// <param name="amount">How many of this resource to request.</param>
        /// <returns>Part or whole of resources requested.</returns>
        public float Recieve(float amount)
        {
            float toReturn;

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
