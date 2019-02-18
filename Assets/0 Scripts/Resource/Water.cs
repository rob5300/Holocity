using Infrastructure.Residents;

namespace CityResources
{
    public class Water : Resource
    {
        public Water(int gridid) : base(gridid)
        {
            CapValue = true;
            ValueCap = 500000;
            TimeoutTime = 10f;

            Happiness.EvaluateWater = true;
        }
    }
}
