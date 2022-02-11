using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Data.LocationModules
{
    internal class FoodCourtModule : LocationModule
    {
        private int _eatingSpots;
        private ObservableCollection<Production> _producedFoods;
    }
}