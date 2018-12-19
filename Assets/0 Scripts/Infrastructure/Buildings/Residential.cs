
using Infrastructure.Residents;
using Infrastructure.Tick;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Residential : Building
    {
        public Resident Resident { get; private set; }
        public bool IsVacant { get { return Resident == null; } }

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
