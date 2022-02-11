using EcoSimConsole.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Control
{
    public class CommodityEditorControl : TopControl
    {
        private Commodity commodity = new Commodity();

        private string id;
        private string name;
        private CommodityType type;
        private string defaultBuyPrice;
        private string defaultSellPrice;

        public CommodityEditorControl()
        {
            ID = MainControl.main.Commodities.Count.ToString();
        }

        public ObservableCollection<Commodity> Commodities
        {
            get
            {
                return MainControl.main.Commodities;
            }
            set
            {
                if (MainControl.main.Commodities != value)
                {
                    MainControl.main.Commodities = value;
                    OnPropertyChanged("Commodities");
                }
            }
        }

        public Commodity SelectedCommodity
        {
            get
            {
                return commodity;
            }
            set
            {
                if (commodity != value)
                {
                    commodity = value;
                    OnPropertyChanged("SelectedCommodity");
                }
            }
        }

        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public CommodityType SelectedType
        {
            get
            {
                return type;
            }
            set
            {
                if (type != value)
                {
                    type = value;
                    OnPropertyChanged("SelectedType");
                }
            }
        }

        public string DefaultBuyPrice
        {
            get
            {
                return defaultBuyPrice;
            }
            set
            {
                if (defaultBuyPrice != value)
                {
                    defaultBuyPrice = value;
                    OnPropertyChanged("DefaultBuyPrice");
                }
            }
        }

        public string DefaultSellPrice
        {
            get
            {
                return defaultSellPrice;
            }
            set
            {
                if (defaultSellPrice != value)
                {
                    defaultSellPrice = value;
                    OnPropertyChanged("DefaultSellPrice");
                }
            }
        }

        public void Save()
        {
            commodity = new Commodity();
            commodity.ID = int.Parse(ID);
            commodity.Name = Name;
            commodity.Type = SelectedType;
            commodity.defaultBuyPrice = double.Parse(DefaultBuyPrice);
            commodity.defaultSellPrice = double.Parse(DefaultSellPrice);

            bool exists = false;

            Commodity old = new Commodity();
            foreach (Commodity c in MainControl.main.Commodities)
            {
                if (c != null && (c.ID == int.Parse(ID) || c.Name == Name && commodity.Name.Length > 0))
                {
                    exists = true;
                    old = c;
                    break;
                }
            }
            if (exists)
            {
                MainControl.main.Commodities.Remove(old);
                MainControl.main.Commodities.Add(commodity);
                MainControl.main.SaveToFile(0);

                ID = MainControl.main.Commodities.Count.ToString();
            }
            else
            {
                MainControl.main.Commodities.Add(commodity);
                MainControl.main.SaveToFile(0);

                ID = MainControl.main.Commodities.Count.ToString();
            }
        }

        public void ClearAll()
        {
            ID = "";
            Name = "";
            DefaultBuyPrice = "";
            DefaultSellPrice = "";
        }

        public void NotifyAll(System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                commodity = (Commodity)e.AddedItems[0];

                ID = commodity.ID.ToString();
                Name = commodity.Name;
                SelectedType = commodity.Type;
                DefaultBuyPrice = commodity.defaultBuyPrice.ToString();
                DefaultSellPrice = commodity.defaultSellPrice.ToString();
            }
            OnPropertyChanged("ID");
            OnPropertyChanged("Name");
            OnPropertyChanged("Type");
            OnPropertyChanged("DefaultSell");
            OnPropertyChanged("DefaultBuy");
        }
    }
}