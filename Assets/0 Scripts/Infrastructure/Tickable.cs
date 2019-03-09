using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tick
{
    public interface Tickable
    {
        /// <summary>
        /// Tick event callback that is called by a TickManager.
        /// </summary>
        /// <param name="time">Time delay in seconds for ticks.</param>
        void Tick(float time);
        /// <summary>
        /// If this tickable should be removed when it is next ticked.
        /// </summary>
        bool ShouldBeRemoved { get; set; }
    }
}
