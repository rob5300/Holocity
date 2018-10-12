using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tick
{
    public class TickManager
    {
        public float TickRate;
        public event EventHandler Tick;

        private float TickTime { get {
                return 1 / TickRate;
            } }
        private float timeSinceLastTick = 0;

        public TickManager(float tickrate = 20)
        {
            TickRate = tickrate;
        }

        public void RateUpdate(float time)
        {
            timeSinceLastTick += time;
            if(timeSinceLastTick >= TickTime)
            {
                Tick.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
