
using Infrastructure.Tick;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class Modern_Bank: Commercial, ITickable
    {
        public Modern_Bank()
        {
            PrefabName = "Modern/Bank";
            Name = "Bank";
            Cost = 800;

            AddJobs(5);
        }

        public override void Tick(float time)
        {
            base.Tick(time);

            Game.CurrentSession.AddFunds((uint)System.Math.Floor(1 * time));
        }
    }
}
