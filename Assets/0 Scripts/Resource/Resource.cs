using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public class Resource
    {
        public Resource()
        {

        }

        public int Avaliable { get; private set; }

        public void Add(int amount)
        {
            Avaliable += amount;
        }
    }
}
