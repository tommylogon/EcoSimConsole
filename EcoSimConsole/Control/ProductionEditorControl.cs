using EcoSimConsole.Data;
using EcoSimConsole.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Control
{
    public class ProductionEditorControl : TopControl
    {
       // private Production _process = new Production(MainControl.main.SelectedLocation);
        //private Production _productionProcess = new Production();
        private Commodity _commodity = new Commodity();

        private Location _location;
        private ObservableCollection<Commodity> _nonProduceedCommodities = new ObservableCollection<Commodity>();

        private string _maxRate;
        private string _maxStorage;
        private string _price;

        private bool _isConsumed;

        //public ProductionEditorControl()
        //{
        //    if (MainControl.main.SelectedLocation != null)
        //    {
        //        _location = MainControl.main.SelectedLocation;
        //        if (MainControl.main.SelectedProduction != null)
        //        {
        //            Process = MainControl.main.SelectedProduction;
        //            //SelectedCommodity = Process.commodity;
        //            //MaxRate = Process.NetRate.ToString();
        //            //MaxStorage = Process.MaxStorage.ToString();
        //            Price = Process.price.ToString();
        //        }
        //    }
        //}

        public ProductionEditorControl(Location l)
        {
            _location = l;
        }

        //public ProductionEditorControl(Production productionProcess, Location l)
        //{
        //    _location = l;
        //    ProductionProcess = productionProcess;
        //    IsConsumed = true;
        //}

        //public Production Process
        //{
        //    get
        //    {
        //        return _process;
        //    }
        //    set
        //    {
        //        if (_process != value)
        //        {
        //            _process = value;
        //            OnPropertyChanged("Process");
        //        }
        //    }
        //}

        internal void Remove()
        {
            //bool exists = false;
            //if (ProductionLocation.Productions.Any(c => c.commodity.Name == Process.commodity.Name))
            //{
            //    exists = true;
            //}
            //if (exists)
            //{
            //    ProductionLocation.Productions.Remove(ProductionLocation.Productions.First(c => c.commodity.Name == Process.commodity.Name));
            //    MainControl.main.SaveToFile(1);
            //}
        }

       // public Production ProductionProcess
        //{
            //get
            //{
            //    return _productionProcess;
            //}
            //set
            //{
            //    if (_productionProcess != value)
            //    {
            //        _productionProcess = value;
            //        OnPropertyChanged("ProductionProcess");
            //    }
            //}
        //}

        public Location ProductionLocation
        {
            get
            {
                return _location;
            }
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged("ProductionLocation");
                }
            }
        }

        //public ObservableCollection<Commodity> ProducedCommodities
        //{
        //    //get
        //    //{
        //    //    _nonProduceedCommodities.Clear();
        //    //    foreach (var c in MainControl.main.Commodities)
        //    //    {
        //    //        if (_location != null && c != null)
        //    //        {
        //    //            if (!_location.Productions.Any(p => p.commodity.Name == c.Name) && !_location.Productions.Any(p => p.Requirements.Any(i => i.commodity.Name == c.Name)))
        //    //            {
        //    //                _nonProduceedCommodities.Add(c);
        //    //            }
        //    //        }
        //    //    }

        //    //    return _nonProduceedCommodities;
        //    //}
        //}

        //public ObservableCollection<Commodity> NonProducedCommodities
        //{
        //    get
        //    {
        //        _nonProduceedCommodities.Clear();
        //        foreach (var c in MainControl.main.Commodities)
        //        {
        //            if (_location != null)
        //            {
        //                if (!_location.Productions.Any(p => p.commodity.ID == c.ID))
        //                {
        //                    _nonProduceedCommodities.Add(c);
        //                }
        //            }
        //        }
        //        return _nonProduceedCommodities;
        //    }
        //    set
        //    {
        //        if (_nonProduceedCommodities != value)
        //        {
        //            _nonProduceedCommodities = value;
        //            OnPropertyChanged("NonProducedCommodities");
        //        }
        //    }
        //}

        public Commodity SelectedCommodity
        {
            get
            {
                return _commodity;
            }
            set
            {
                if (_commodity != value)
                {
                    _commodity = value;
                    if (_commodity != null)
                    {
                        if (IsConsumed)
                        {
                            Price = _commodity.defaultBuyPrice.ToString();
                        }
                        else
                        {
                            Price = _commodity.defaultSellPrice.ToString();
                        }
                    }
                    OnPropertyChanged("SelectedCommodity");
                }
            }
        }

        public string MaxRate
        {
            get
            {
                return _maxRate;
            }

            set
            {
                if (_maxRate != value)
                {
                    _maxRate = value;
                    OnPropertyChanged("MaxRate");
                }
            }
        }

        public string MaxStorage
        {
            get
            {
                return _maxStorage;
            }

            set
            {
                if (_maxStorage != value)
                {
                    _maxStorage = value;
                    OnPropertyChanged("MaxStorage");
                }
            }
        }

        public string Price
        {
            get
            {
                return _price;
            }

            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged("Price");
                }
            }
        }

        public bool IsConsumed
        {
            get
            {
                return _isConsumed;
            }

            set
            {
                if (_isConsumed != value)
                {
                    _isConsumed = value;
                    if (_commodity != null)
                    {
                        if (_isConsumed)
                        {
                            Price = _commodity.defaultBuyPrice.ToString();
                        }
                        else
                        {
                            Price = _commodity.defaultSellPrice.ToString();
                        }
                    }
                    OnPropertyChanged("IsConsumed");
                }
            }
        }

        //public void Save()
        //{
        //    Process.commodity = SelectedCommodity;
        //    Process.maxNetRate = int.Parse(MaxRate);
        //    Process.maxStorage = int.Parse(MaxStorage);
        //    Process.price = double.Parse(Price);
        //    Process.isConsumed = IsConsumed;

        //    bool exists = false;
        //    if (Process.Requirements.Count == 0 && !IsConsumed)
        //    {
        //        Process.isRawProducer = true;
        //    }

        //    if (ProductionProcess != null && ProductionProcess.commodity != null)
        //    {
        //        if (int.Parse(MaxRate) > 0 && int.Parse(MaxStorage) > 0)
        //        {
        //            if (ProductionProcess.Requirements.Any(c => c.commodity.Name == Process.commodity.Name))
        //            {
        //                exists = true;
        //            }
        //            if (exists)
        //            {
        //                ProductionProcess.Requirements.Remove(ProductionProcess.Requirements.First(c => c.commodity.Name == Process.commodity.Name));
        //                ProductionProcess.Requirements.Add(Process);
        //            }
        //            else
        //            {
        //                ProductionProcess.Requirements.Add(Process);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (int.Parse(MaxRate) > 0 && int.Parse(MaxStorage) > 0)
        //        {
        //            if (ProductionLocation.Productions.Any(c => c.commodity.Name == Process.commodity.Name))
        //            {
        //                exists = true;
        //            }
        //            if (exists)
        //            {
        //                ProductionLocation.Productions.Remove(ProductionLocation.Productions.First(c => c.commodity.Name == Process.commodity.Name));
        //                ProductionLocation.Productions.Add(Process);
        //                MainControl.main.SaveToFile(1);
        //            }
        //            else
        //            {
        //                ProductionLocation.Productions.Add(Process);
        //                MainControl.main.SaveToFile(1);
        //            }
        //        }
        //    }

        //    Process = new Production();
        //}

        public bool Cancel()
        {
            return false;
        }

        public void CreateNewCommodity()
        {
            CommodityEditor commodity = new CommodityEditor();

            commodity.Show();
        }

        //internal void AddRequirement()
        //{
        //    ProductionEditor productionWindow = new ProductionEditor(Process, _location);

        //    productionWindow.Show();
        //}
    }
}