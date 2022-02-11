using EcoSimConsole.Data.LocationModules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Production : INotifyPropertyChanged
    {
        [JsonProperty]
        public List<Recipie> Recipies;

        public LocationModule Building;

        [JsonProperty]
        public double price;

        private double _productivity = 1;

        private int _maxWorkerSlots = 1;
        private int _currentWorkerSlots = 0;


        


        public Production(LocationModule building)
        {
            Building = building;
            Recipies = new List<Recipie>();
        }

       public void AddWorkers()
        {
            if(_currentWorkerSlots < _maxWorkerSlots)
            {
                _currentWorkerSlots++;

            }
            CalculateProductivity();
        }

        private void CalculateProductivity()
        {
            _productivity = (double)_currentWorkerSlots / (double)_maxWorkerSlots;

            if (_productivity > 1)
            {
                _productivity = 1;
            }
            else if(_productivity < 0)
            {
                _productivity = 0;
            }
        }

        public void Produce()
        {
           
            StorageModule storageImport = Building.ModuleLocation.GetStorage(LocationModules.StorageType.Import);
            StorageModule storageExport = Building.ModuleLocation.GetStorage(LocationModules.StorageType.Export);
            if(Recipies.Count > 0 )
            {
                if(storageExport.StoredItems.Count <= storageExport.MaxStorage)
                {
                    foreach (var recipe in Recipies)
                    {
                        bool canProduce = false;
                        if (recipe.Consumed.Count == 0)
                        {
                            canProduce = true;                            

                        }
                        else if(storageImport.StoredItems.Count > 0)
                        {
                            foreach (var consumedComodity in recipe.Consumed)
                            {
                                foreach (var storedCommodity in storageImport.StoredItems)
                                {
                                    if (storedCommodity.commodity.ID == consumedComodity.Commodity.ID && storedCommodity.units >= recipe.Produced.Amount)
                                    {
                                        canProduce = true;
                                        continue;
                                    }
                                    else
                                    {
                                        canProduce = false;

                                    }
                                }

                            }
                        }
                        
                        if (canProduce)
                        {
                            foreach (var consumed in recipe.Consumed)
                            {
                                storageImport.ConsumeFromStorage(consumed.Commodity.ID, consumed.Amount);

                            }
                            storageExport.AddToStorage(recipe.Produced.Commodity.ID, recipe.Produced.Amount);
                        }


                    }
                }
                
            }
            
        }

        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged("Price");
                }
            }
        }
        public double Productivity
        {
            get
            {
                return _productivity;
            }
            set
            {
                if (_productivity != value)
                {
                    _productivity = value;
                    OnPropertyChanged("Productivity");
                }
            }
        }
        public int MaxWorkerSlots
        {
            get
            {
                return _maxWorkerSlots;
            }
            set
            {
                if (_maxWorkerSlots != value)
                {
                    _maxWorkerSlots = value;
                    OnPropertyChanged("MaxWorkerSlots");
                }
            }
        }
        public int CurrentWorkerSlots
        {
            get
            {
                return _currentWorkerSlots;
            }
            set
            {
                if (_currentWorkerSlots != value)
                {
                    _currentWorkerSlots = value;
                    OnPropertyChanged("FreeWorkerSlots");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
   
}