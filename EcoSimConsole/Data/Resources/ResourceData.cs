using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Data
{
    public class ResourceData
    {
        public Commodity Commodity;
        public int Amount;

        public ResourceData(Commodity commodity, int amount )
        {
            Commodity = commodity;
            Amount = amount;
        }
    }
}
