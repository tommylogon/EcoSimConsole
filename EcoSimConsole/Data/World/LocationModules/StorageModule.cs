using EcoSimConsole.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Data.LocationModules
{
    public class StorageModule : LocationModule
    {
        public StorageType storageType;
        private int _maxStorage;
        private int _currentStorage;
        private ObservableCollection<StorageInfo> _storedItems = new ObservableCollection<StorageInfo>();

        public StorageModule(StorageType type)
        {
            storageType = type;
            if (storageType == StorageType.Import)
            {
                name = "Storage Module import";
            }
            else
            {
                name = "Storage Module Export";
            }
            
        }

        public int MaxStorage
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

        public int CurrentStorage
        {
            get
            {
                return _currentStorage;
            }
            set
            {
                if (_currentStorage != value)
                {
                    _currentStorage = value;
                    OnPropertyChanged("CurrentStorage");
                }
            }
        }

        public ObservableCollection<StorageInfo> StoredItems
        {
            get
            {
                return _storedItems;
            }
            set
            {
                if (_storedItems != value)
                {
                    _storedItems = value;
                    OnPropertyChanged("StoredItems");
                }
            }
        }
        public override void OnUpdate()
        {
            
            isUpdating = false;
        }
        public void UpdateStorage()
        {
            CurrentStorage = 0;
            foreach (var c in StoredItems)
            {
                CurrentStorage += c.units;
            }

        }
        public bool ConsumeFromStorage(int ID, int value)
        {
            StorageInfo si = StoredItems.First(c => c.commodity.ID == ID);
            if(si.units >= value)
            {
                si.units -= value;
                UpdateStorage();
                return true;
            }
            return false;
            
        }
        public StorageInfo GetItem(int ID)
        {
            return StoredItems.First(c => c.commodity.ID == ID);
        }

        internal bool AddToStorage(int ID, int amount)
        {
            if(CurrentStorage <= MaxStorage)
            {

                if (StoredItems.Any(c => c.commodity.ID == ID))
                {

                    StorageInfo si = StoredItems.First(c => c.commodity.ID == ID);
                    StoredItems.Remove(si);
                    si.units += amount;
                    StoredItems.Add(si);
                    UpdateStorage();
                    return true;
                }
                else
                {
                    StorageInfo si = new StorageInfo();
                    si.commodity = MainControl.main.Commodities.First(co => co.ID == ID);
                    si.units = amount;
                    StoredItems.Add(si);
                    UpdateStorage();
                    return true;
                }
            }


                return false;
        }
    }

    public struct StorageInfo
    {
        public Commodity commodity;
        public int units;
    }
    public enum StorageType
    {
        Import,
        Export
    }
}