using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityResources
{
    public class Water : Resource
    {
        public Water()
        {
            CapValue = true;
            ValueCap = 500000;
            TimeoutTime = 10f;
        }
    }
}
