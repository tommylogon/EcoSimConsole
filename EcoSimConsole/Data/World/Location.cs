using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using EcoSimConsole.Control;
using System.Windows.Threading;
using System.Diagnostics;
using EcoSimConsole.Data.LocationModules;

namespace EcoSimConsole.Data
{
    public class Location : StellarObject
    {
        [JsonProperty]
        private double _uec = 100000;

        [JsonProperty]
        private double _wear = 100;

        [JsonProperty]
        private double _fuel = 10000;

        [JsonProperty]
        private bool _isOnSurface;

        [JsonProperty]
        private ObservableCollection<Production> _producedResource = new ObservableCollection<Production>();

        [JsonProperty]
        private ObservableCollection<LocationModule> _modules = new ObservableCollection<LocationModule>();

        [JsonProperty]
        private ObservableCollection<Ship> _shipStorage = new ObservableCollection<Ship>();

        [JsonProperty]
        private int _maxHangars = 4;

        [JsonProperty]
        private int _freeHangars = 4;

              

        private ObservableCollection<Ship> _shipQueue = new ObservableCollection<Ship>();
        private ObservableCollection<Ship> _hangars = new ObservableCollection<Ship>();
        private int _currentTransactions = 0;
        private int _previousTransactions = 0;

        public Location()
        {
            StorageModule smi = new StorageModule(StorageType.Import);
            StorageModule sme = new StorageModule(StorageType.Export);
            HousingModule hm = new HousingModule();
            hm.MaxHousing = 50;
            hm.FreeHousing = 25;
            smi.MaxStorage = 20000;
            
            sme.MaxStorage = 20000;
            
            Modules.Add(smi);
            Modules.Add(sme);
            Modules.Add(hm);

        }


        public double UEC
        {
            get
            {
                return _uec;
            }
            set
            {
                if (_uec != value)
                {
                    _uec = value;
                    OnPropertyChanged("UEC");
                }
            }
        }

        public double Wear
        {
            get
            {
                return _wear;
            }
            set
            {
                if (_wear != value)
                {
                    _wear = value;
                    OnPropertyChanged("Wear");
                }
            }
        }
        public int MaxHangars
        {
            get
            {
                return _maxHangars;
            }
            set
            {
                if (_maxHangars != value)
                {
                    _maxHangars = value;
                    OnPropertyChanged("MaxHangars");
                }
            }
        }
        public int FreeHangars
        {
            get
            {
                return _freeHangars;
            }
            set
            {
                if (_freeHangars != value)
                {
                    _freeHangars = value;
                    OnPropertyChanged("FreeHangars");
                }
            }
        }
        public int MaxHousing
        {
            get
            {
                if (Modules.Count > 0)
                {
                    foreach(var module in Modules)
                    {
                        if(module is HousingModule hm)
                        {
                            return hm.MaxHousing;
                        }
                    }
                }
                return 0;
            }
            set
            {
                if (Modules.Count > 0 && MaxHousing != value)
                {
                    foreach (var module in Modules)
                    {
                        if (module is HousingModule hm)
                        {
                             hm.MaxHousing = value;
                        }
                    }
                }
               
                    OnPropertyChanged("MaxHousing");
                


            }
        }
        public int FreeHousing
        {
            get
            {

                if (Modules.Count > 0)
                {
                    foreach (var module in Modules)
                    {
                        if (module is HousingModule hm)
                        {
                            return hm.FreeHousing;
                        }
                    }
                }
                return 0;
            }
            set
            {

                if (Modules.Count > 0 && FreeHousing != value)
                {
                    foreach (var module in Modules)
                    {
                        if (module is HousingModule hm)
                        {
                            hm.FreeHousing = value;
                        }
                    }
                }
                OnPropertyChanged("FreeHousing");
                }


            
        }

        public ObservableCollection<Production> Productions
        {
            get
            {
                return _producedResource;
            }
            set
            {
                if (_producedResource != value)
                {
                    _producedResource = value;
                    OnPropertyChanged("Productions");
                }
            }
        }

        public ObservableCollection<Ship> Hangars
        {
            get
            {
                return _hangars;
            }
            set
            {
                if (_hangars != value)
                {
                    _hangars = value;
                    OnPropertyChanged("Hangars");
                }
            }
        }
        public ObservableCollection<LocationModule> Modules
        {
            get
            {
                return _modules;
            }
            set
            {
                if (_modules != value)
                {
                    _modules = value;
                    OnPropertyChanged("Modules");
                }
            }
        }

        public ObservableCollection<Ship> ShipQueue
        {
            get
            {
                return _shipQueue;
            }
            set
            {
                if (_shipQueue != value)
                {
                    _shipQueue = value;
                    OnPropertyChanged("ShipQueue");
                }
            }
        }

        public bool IsOnSurface
        {
            get
            {
                return _isOnSurface;
            }
            set
            {
                if (_isOnSurface != value)
                {
                    _isOnSurface = value;
                    OnPropertyChanged("IsOnSurface");
                }
            }
        }

        public string GetHangarInfo
        {
            get
            {
                _freeHangars = _maxHangars - _hangars.Count;
                return "Hangars: " + _freeHangars + "/" + _maxHangars;
            }
        }

        public string GetQueueInfo
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Queue: " + _shipQueue.Count);

                return sb.ToString();
            }
        }

        public string GetTransactions
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Transactiopns last Tick: " + _previousTransactions).AppendLine();
                sb.Append("Transactions: " + _currentTransactions);
                return sb.ToString();
            }
        }

        public override void OnUpdate()
        {

            Produce();
            
          

            _previousTransactions = _currentTransactions;
            _currentTransactions = 0;
            //CalculateProductivity();
            //ApplyNetRate();
            AdvanceLandingQueue();

            OnPropertyChanged("GetTransactions");
            OnPropertyChanged("GetHangarInfo");
            OnPropertyChanged("Consumed");
            isUpdating = false;
        }

        private void Produce()
        {
           foreach(var module in _modules)
            {
                if(module is MiningModule mm)
                {
                    mm.Mine();
                }
            }
        }

        public StorageModule GetStorage(StorageType storageType)
        {
            foreach(var lm in _modules)
            {
                if(lm is StorageModule sm)
                {
                    if(sm.storageType == storageType)
                    {
                        return sm;
                    }
                }
            }
            return null;
        }

        //private void PowerProduction()
        //{
        //    if (_modules.Count > 0)
        //    {
        //        bool foundPm = false;
        //        bool foundSm = false;
        //        PowerModule pm = new PowerModule();
        //        StorageModule sm = new StorageModule();
        //        double currentPower = 0;
        //        foreach (var module in _modules)
        //        {
        //            if (module is PowerModule p)
        //            {
        //                pm = p;
        //                foundPm = true;
        //            }
        //            else if (module is StorageModule s)
        //            {
        //                sm = s;
        //                foundSm = true;
        //                currentPower += s.RequiredPower;
        //            }
        //            else
        //            {
        //                currentPower += module.RequiredPower;
        //            }
        //        }
        //        if (foundPm && foundSm)
        //        {
        //            pm.PowerConsumption = currentPower;
        //            pm.ProducePower(sm);
        //        }
        //    }
        //}

        //private void UpdateMarketInfo()
        //{
        //    foreach (var produced in Productions)
        //    {
        //        MarketInfo mi = new MarketInfo();
        //        foreach (var m in MainControl.main.MarketInfos)
        //        {
        //            if (m.commodity.ID == produced.producedCommodity.ID && m.minSellPrice > produced.price)
        //            {
        //                mi = m;
        //                break;
        //            }
        //        }

        //        if (mi.commodity != null && mi.commodity.ID == produced.producedCommodity.ID)
        //        {
        //            MainControl.main.MarketInfos.Remove(mi);
        //            mi = new MarketInfo();
        //            mi.commodity = produced.producedCommodity;
        //            mi.minSellPrice = produced.price;
        //            MainControl.main.MarketInfos.Add(mi);
        //        }
        //        if (MainControl.main.MarketInfos.Count == 0)
        //        {
        //            mi = new MarketInfo()
        //            {
        //                commodity = produced.producedCommodity,
        //                minSellPrice = produced.Price
        //            };
        //            MainControl.main.MarketInfos.Add(mi);
        //        }
        //    }
        //}

        //private void CalculateProductivity()
        //{
        //    double inStorage = 0;
        //    double total = 0;
        //    if (Productions.Count > 0)
        //    {
        //        foreach (var process in Productions)
        //        {
        //            if (process.Requirements.Count > 0)
        //            {
        //                foreach (var consumed in process.Requirements)
        //                {
        //                    if (consumed.CurrentStorage > 0)
        //                    {
        //                        inStorage++;
        //                        total++;
        //                    }
        //                    else
        //                    {
        //                        total++;
        //                    }
        //                }
        //            }
        //        }

        //        _productivity = inStorage / total;
        //    }
        //}

        private void AdvanceLandingQueue()
        {
            if (ShipQueue.Count > 0)
            {
                if (Hangars.Count > 0)
                {
                    var toRemove = new ObservableCollection<Ship>();
                    foreach (var ship in Hangars)
                    {
                        if (ship.Owner.CurrentLocation != this)
                        {
                            toRemove.Add(ship);
                        }
                    }
                    foreach (var ship in toRemove)
                    {
                        Hangars.Remove(ship);
                    }
                }
                RequestLanding(_shipQueue[0]);

                OnPropertyChanged("GetQueueInfo");
            }
        }

        public bool RequestLanding(Ship ship)
        {
            if (_hangars.Count < _maxHangars)
            {
                if (_shipQueue.Contains(ship))
                {
                    _shipQueue.Remove(ship);
                }
                _hangars.Add(ship);
                _freeHangars = _maxHangars - _hangars.Count;
                OnPropertyChanged("GetHangarInfo");
                return true;
            }
            else
            {
                if (!_shipQueue.Contains(ship))
                {
                    _shipQueue.Add(ship);
                }
            }
            return false;
        }

        public bool RequestTakeoff(Ship ship)
        {
            if (_hangars.Contains(ship))
            {
                _hangars.Remove(ship);
                _freeHangars = _maxHangars - _hangars.Count;
            }
            else if (_shipQueue.Contains(ship))
            {
                _shipQueue.Remove(ship);
            }
            OnPropertyChanged("GetHangarInfo");
            return true;
        }

        //private bool UpdateProcessData(Production process)
        //{
        //    process.NetRate = process.maxNetRate;

        //    if (process.CurrentStorage > 0)
        //    {
        //        process.CurrentStorage -= process.NetRate;
        //        return true;
        //    }
        //    else if (process.currentStorage <= 0)
        //    {
        //        process.CurrentStorage = 0;
        //        return false;
        //    }
        //    return false;
        //}

        //public void ApplyNetRate()
        //{
        //    foreach (var process in Productions)
        //    {
        //        bool hasRequired = false;

        //        if (process.Requirements.Count > 0)
        //        {
        //            foreach (var consumed in process.Requirements)
        //            {
        //                UpdateProcessData(consumed);
        //                ApplySupplyDemand(consumed);
        //            }
        //        }
        //        if (process.isConsumed)
        //        {
        //            UpdateProcessData(process);
        //            ApplySupplyDemand(process);
        //        }
        //        if (hasRequired)
        //        {
        //            process.NetRate = Convert.ToInt32(process.maxNetRate * _productivity);
        //            if (process.CurrentStorage < process.maxStorage)
        //            {
        //                process.CurrentStorage += process.NetRate;
        //            }
        //        }
        //        else if (process.isRawProducer)
        //        {
        //            if (process.CurrentStorage < process.maxStorage)
        //            {
        //                process.NetRate = process.maxNetRate;
        //                if (process.CurrentStorage < process.maxStorage)
        //                {
        //                    process.CurrentStorage += process.NetRate;
        //                }
        //            }
        //        }
        //        ApplySupplyDemand(process);
        //    }
        //}

    //    private void ApplySupplyDemand(Production process)
    //    {
    //        _currentTransactions += Convert.ToInt32(process.transactions * 100);
    //        if (process.isConsumed)
    //        {
    //            if (process.transactions > 0)
    //            {
    //                process.Price -= process.transactions;
    //            }
    //            else
    //            {
    //                if (process.CurrentStorage / process.MaxStorage < 0.1)
    //                {
    //                    process.price += 0.03;
    //                }
    //                else if (process.CurrentStorage / process.MaxStorage > 0.9)
    //                {
    //                    process.price -= 0.05;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (process.transactions > 0)
    //            {
    //                process.price += process.transactions;
    //            }
    //            else
    //            {
    //                if (process.CurrentStorage / process.MaxStorage > 0.9)
    //                {
    //                    process.price -= 0.03;
    //                }
    //                if (process.price < 0)
    //                {
    //                    process.price = 0.01;
    //                }
    //            }
    //        }

    //        if (process.CurrentStorage / process.MaxStorage > 0.9)
    //        {
    //            process.Price -= 0.01;
    //        }

    //        process.transactions = 0;
    //    }
    }

    public struct TransactionData
    {
        public Production resource;
        public int amount;
        public TransationcType type;
        public Citizen transactor;
    }

    public enum TransationcType
    {
        Sold,
        Bought
    }
}