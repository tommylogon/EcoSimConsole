using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Data.LocationModules
{
    internal class PowerModule : LocationModule
    {
        private bool _isActive = false;
        private double _maxPowerProduced = 1000;
        private double _currentPowerConsumption = 0;
        private Commodity _fuelType;
        private int _fuelRate = 92;

        public double PowerConsumption
        {
            get
            {
                return _currentPowerConsumption;
            }
            set
            {
                if (_currentPowerConsumption != value)
                {
                    _currentPowerConsumption = value;
                    OnPropertyChanged("PowerConsumption");
                }
            }
        }

        public bool ProducePower(StorageModule sm)
        {
            //_isActive = true;
            //StorageInfo storage = new StorageInfo();
            //if (sm.CurrentStorage.Count > 0)
            //{
            //    foreach (StorageInfo stored in sm.CurrentStorage)
            //    {
            //        if (stored.commodity.Name == _fuelType.Name && _fuelRate <= stored.units)
            //        {
            //            storage = stored;
            //            storage.units -= _fuelRate;
            //            if (_currentPowerConsumption > _maxPowerProduced)
            //            {
            //                _isActive = false;
            //            }
            //            else
            //            {
            //                _isActive = true;
            //            }
            //        }
            //    }
            //}

            _isActive = false;
            return _isActive;
        }
    }
}