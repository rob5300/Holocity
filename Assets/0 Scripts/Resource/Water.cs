using Infrastructure.Residents;

namespace CityResources
{
    public class Water : Resource
    {
        public Water()
        {
            CapValue = true;
            ValueCap = 500000;
            TimeoutTime = 10f;

            Happiness.EvaluateWater = true;
        }
    }
}
