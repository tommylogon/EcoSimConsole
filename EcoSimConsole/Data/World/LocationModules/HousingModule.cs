using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Data.LocationModules
{
    internal class HousingModule : LocationModule
    {
        private int _maxHousingSlots;
        private int _idleWorkers;
        private int _freeHousingSlots;

        public HousingModule()
        {
            name = "Housing Module";
        }
        public int MaxHousing
        {
            get
            {
                return _maxHousingSlots;
            }
            set
            {
                if (_maxHousingSlots != value)
                {
                    _maxHousingSlots = value;
                    OnPropertyChanged("HousingSlots");
                }
            }
        }
        public int FreeHousing
        {
            get
            {
                return _freeHousingSlots;
            }
            set
            {
                if (_freeHousingSlots != value)
                {
                    _freeHousingSlots = value;
                    OnPropertyChanged("FreeHousing");
                }
            }
        }
        public int IdleWorkers
        {
            get
            {
                return _idleWorkers;
            }
            set
            {
                if (_idleWorkers != value)
                {
                    _idleWorkers = value;
                    OnPropertyChanged("IdleWorkers");
                }
            }
        }
    }
}