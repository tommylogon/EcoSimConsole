using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Data.LocationModules
{
    internal class MiningModule : LocationModule
    {
        

        private Production _MiningProcess;

        public MiningModule(Location location)
        {
            name = "Mining Module";
            ModuleLocation = location;
            Mining = new Production(this);
        }


        public Production Mining
        {
            get
            {
                return _MiningProcess;
            }
            set
            {
                if (_MiningProcess != value)
                {
                    _MiningProcess = value;
                    OnPropertyChanged("Mining");
                }
            }
        }
        public void Mine()
        {
            _MiningProcess.Produce();
        }
        public override void OnUpdate()
        {
           
            
            
            isUpdating = false;
        }
    }
}