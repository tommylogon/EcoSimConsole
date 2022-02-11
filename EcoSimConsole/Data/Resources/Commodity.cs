using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Data
{
    public class Commodity
    {
        public int ID;
        public string Name { get; set; }
        public CommodityType Type;
        public double defaultBuyPrice;
        public double defaultSellPrice;

        public Commodity()
        {

        }
    }

    public enum CommodityType
    {
        Agricultural,
        Alloy,
        Components,
        ConsumerGoods,
        Food,
        Fuel,
        Gas,
        Medical, 
        Metal,
        Mineral,
        Natural,
        NonMetals,
        ProcessGoods,
        Vice,
        Waste


    }
}