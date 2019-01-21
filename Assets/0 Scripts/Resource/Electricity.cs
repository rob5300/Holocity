using Infrastructure.Residents;

namespace CityResources
{
    public class Electricity : Resource
    {
        public Electricity()
        {
            CapValue = true;
            ValueCap = 30000;
            TimeoutTime = 5f;

            //Ensure that we evaluate Electricity for happiness now
            Happiness.EvaluateElectricity = true;
        }
    }
}
