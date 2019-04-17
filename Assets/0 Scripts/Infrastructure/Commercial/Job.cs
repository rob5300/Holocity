using Infrastructure.Residents;

namespace Infrastructure
{
    public class Job
    {
        public float Salary = 0;
        public bool Taken = false;
        public Resident Holder;

        public Job(Resident holder)
        {
            Holder = holder;
        }

        public Job(Resident holder, float salary) : this(holder)
        {
            Salary = salary;
        }
    }
}
