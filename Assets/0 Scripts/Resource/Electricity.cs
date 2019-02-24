using Infrastructure.Residents;

namespace CityResources
{
    public class Electricity : Resource
    {
        public Electricity(int gridid) : base(gridid)
        {
            CapValue = true;
            ValueCap = 30000;
            TimeoutTime = 5f;

            //Ensure that we evaluate Electricity for happiness now
            Happiness.EvaluateElectricity = true;
        }
    }
}
