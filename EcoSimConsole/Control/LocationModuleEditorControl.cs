using EcoSimConsole.Data;
using EcoSimConsole.Data.LocationModules;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Control
{
    public class LocationModuleEditorControl : TopControl
    {
        private Location location;
        private LocationModule _selectedModule;
        private string _name;
        private object _value1;
        private object _value2;
        private object _value3;
        public LocationModuleEditorControl(Location location)
        {
            this.location = location;
        }
        public List<ISeries> Series { get; set; } = new List<ISeries>
        {
            
            new ColumnSeries<int>
            {
                Values = new List<int> { 6},
                
            },
             new ColumnSeries<int>
            {
                Values = new List<int> { 3},

            }
        };
        public string Name 
        {
            get 
            {
                return _name;
            }
            set 
            {
                if(_name != value)
                {

                    _name = value;
                    OnPropertyChanged("Name");
                }
            } 
        }
        public object Value1 
        {
            get
            {
                return _value1;
            }
            set
            {
                if (_value1 != value)
                {

                    _value1 = value;
                    OnPropertyChanged("Value1");
                }
            }
        }
        public object Value2
        {
            get
            {
                return _value2;
            }
            set
            {
                if (_value2 != value)
                {

                    _value2 = value;
                    OnPropertyChanged("Value2");
                }
            }
        }
        public object Value3
        {
            get
            {
                return _value3;
            }
            set
            {
                if (_value3 != value)
                {

                    _value3 = value;
                    OnPropertyChanged("Value3");
                }
            }
        }
        public Location SelectedLocation
        {
            get
            {
                return location;
            }

        }

        public LocationModule SelectedModule
        {
            get
            {
                return _selectedModule;
            }
            set
            {
                if(_selectedModule != value)
                {
                    _selectedModule = value;
                    UpdateInformation();
                    OnPropertyChanged("SelectedModule");
                }
            }
        }

        private void UpdateInformation()
        {
            Name = SelectedModule.Name;
            if (SelectedModule is StorageModule sm)
            {
                Value1 = sm.MaxStorage;
                Value2 = sm.CurrentStorage;
                Value3 = sm.storageType;
                Series.Clear();
                foreach (var item in sm.StoredItems)
                {
                    var column = new ColumnSeries<int>
                    {
                        Values = new List<int> { item.units },

                    };
                    Series.Add(column);
                }


            }
            else if(SelectedModule is MiningModule mm)
            {
                //Value1 = mm.MaxStorage.ToString();
                //Value2 = mm.MaxStorage.ToString();
                //Value3 = mm.MaxStorage.ToString();
            }
            else if(SelectedModule is HousingModule hm)
            {

            }
           
            
        }
    }
}
