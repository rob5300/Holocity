using Infrastructure.Residents;

namespace Infrastructure
{
    public class Job
    {
        public float Salary = 0;
        public bool Taken = false;
        public Resident Holder;

        public Job()
        {
            
        }

        public Job(float salary)
        {
            Salary = salary;
        }
    }
}
