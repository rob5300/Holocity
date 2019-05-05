using Infrastructure.Grid.Entities.Buildings;
using Infrastructure.Residents;
using Settings.Adjustment;

namespace Infrastructure
{
    public class Job
    {
        public AdjustableFloat Salary;
        public bool Taken = false;
        public Resident Holder;
        public Commercial Origin;

        public Job(Commercial origin)
        {
            Salary = Game.CurrentSession.Settings.BaseSalary;
            Origin = origin;

            Happiness.EnableJob = true;
        }

    }
}
