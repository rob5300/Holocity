
namespace CityResources
{
    public class Electricity : Resource
    {
        public Electricity()
        {
            CapValue = true;
            ValueCap = 30000;
            TimeoutTime = 5f;
        }
    }
}
