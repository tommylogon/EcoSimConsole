using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Data
{
    public class Recipie
    {
        public double productionTime;
        public List<ResourceData> Consumed;
        public ResourceData Produced;


        public Recipie()
        {
            Consumed = new List<ResourceData>();
        }

    }
}
