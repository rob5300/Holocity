
using Infrastructure.Residents;
using Infrastructure.Tick;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Residential : Building
    {
        public Resident Resident { get; private set; }
        public bool IsVacant { get { return Resident == null; } }
        /// <summary>
        /// If this building has power;
        /// </summary>
        public bool HasPower = false;
        /// <summary>
        /// How long without the full amount of a requested resource before the resource type is no longer present.
        /// </summary>
        private float AcceptableLossTime = 1f;
        /// <summary>
        /// If this building has a water supply.
        /// </summary>
        public bool HasWaterSupply = false;

        public virtual void SetResident(Resident res)
        {
            if (Resident == null) Resident = res;
            if(res is Tickable)
            {
                //Add this resident to the TickManagers tickables.
                Game.CurrentSession.TickManager.IncomingTickableQueue.Enqueue(res);
                res.Home = this;
            }
        }
    }
}
