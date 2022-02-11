using EcoSimConsole.Data;
using EcoSimConsole.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EcoSimConsole.Control
{
    public class LocationEditorControll : TopControl
    {
        private StellarObject stellar = new StellarObject();
        private Location location = new Location();

        private LocationType displayFor;
        private ObservableCollection<StellarObject> selectedStarMapObjects = new ObservableCollection<StellarObject>();
        private ObservableCollection<StellarObject> _validOrbiters = new ObservableCollection<StellarObject>();

        private Visibility _showForStellar;
        private Visibility _hideForStellar;
        public bool update;

        private string name;
        private LocationType type;
        private string radius;
        private string degree;
        private string distance;
        private bool isSurfaceLocation;

        private string maxHousing;
        private string maxHangars;

        public StellarObject SelectedLocation
        {
            get
            {
                return location;
            }
            set
            {
                if (value is Location l && l != location)
                {
                    location = l;
                }
                else if (stellar != value)
                {
                    stellar = value;
                }
                NotifyAll();
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

        public string Radius
        {
            get
            {
                return radius;
            }
            set
            {
                if (radius != value)
                {
                    radius = value;
                    OnPropertyChanged("Radius");
                }
            }
        }

        public LocationType SelectedType
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

        public LocationType DisplayType
        {
            get
            {
                return displayFor;
            }
            set
            {
                if (displayFor != value)
                {
                    displayFor = value;
                    OnPropertyChanged("DisplayType");
                }
            }
        }

        public ObservableCollection<StellarObject> StarSystem
        {
            get
            {
                return selectedStarMapObjects;
            }
            set
            {
                if (selectedStarMapObjects != value)
                {
                    selectedStarMapObjects = value;
                    OnPropertyChanged("StarSystem");
                }
            }
        }

        public ObservableCollection<StellarObject> ValidOrbiters
        {
            get
            {
                return _validOrbiters;
            }
            set
            {
                if (_validOrbiters != value)
                {
                    _validOrbiters = value;
                    OnPropertyChanged("ValidOrbiters");
                }
            }
        }

        public StellarObject SelectedStellarObject
        {
            get
            {
                return location.orbiting;
            }
            set
            {
                if (location.orbiting != value)
                {
                    location.orbiting = value;
                    OnPropertyChanged("SelectedStellarObject");
                }
            }
        }

        public string Position
        {
            get
            {
                if (SelectedType == LocationType.Location)
                {
                    return "x=" + location.position.x + ", Y=" + location.position.y;
                }
                else
                {
                    return "x=" + stellar.position.x + ", Y=" + stellar.position.y;
                }
            }
        }

        public string Distance
        {
            get
            {
                return distance;
            }
            set
            {
                if (distance != value)
                {
                    distance = value;
                    OnPropertyChanged("Distance");
                }
            }
        }

        public string Degree
        {
            get
            {
                return degree;
            }
            set
            {
                if (degree != value)
                {
                    degree = value;
                    OnPropertyChanged("Degree");
                }
            }
        }

        public bool IsSurfaceLocation
        {
            get
            {
                return isSurfaceLocation;
            }
            set
            {
                if (isSurfaceLocation != value)
                {
                    isSurfaceLocation = value;
                    OnPropertyChanged("IsSurfaceLocation");
                }
            }
        }

        public string MaxHousing
        {
            get
            {
                return maxHousing;
            }
            set
            {
                if (maxHousing != value)
                {
                    maxHousing = value;
                    OnPropertyChanged("MaxHousing");
                }
            }
        }

        public string MaxHangars
        {
            get
            {
                return maxHangars;
            }
            set
            {
                if (maxHangars != value)
                {
                    maxHangars = value;
                    OnPropertyChanged("MaxHangars");
                }
            }
        }

        public Visibility ShowForStellar
        {
            get
            {
                return _showForStellar;
            }
            set
            {
                if (_showForStellar != value)
                {
                    _showForStellar = value;
                    OnPropertyChanged("ShowForStellar");
                }
            }
        }

        public Visibility HideForStellar
        {
            get
            {
                return _hideForStellar;
            }
            set
            {
                if (_hideForStellar != value)
                {
                    _hideForStellar = value;
                    OnPropertyChanged("HideForStellar");
                }
            }
        }

        internal void NotifyAll()
        {
            if (SelectedLocation.type == LocationType.Location)
            {
                if (location == null)
                {
                    location = new Location();
                }
                Name = location.name;
                SelectedType = location.type;
                SelectedStellarObject = location.orbiting;
                Degree = location.degree.ToString();
                IsSurfaceLocation = location.IsOnSurface;
                if (IsSurfaceLocation && SelectedStellarObject != null)
                {
                    location.distance = SelectedStellarObject.radius;
                }
                Distance = location.distance.ToString();
                MaxHangars = location.MaxHangars.ToString();
                MaxHousing = location.MaxHousing.ToString();
            }
            else
            {
                if (stellar == null)
                {
                    stellar = new StellarObject();
                }
                Name = stellar.name;
                Radius = stellar.radius.ToString();
                SelectedType = stellar.type;
                SelectedStellarObject = stellar.orbiting;
                Degree = stellar.degree.ToString();
                Distance = stellar.distance.ToString();
            }
            OnPropertyChanged("Name");
            OnPropertyChanged("SelectedType");
            OnPropertyChanged("SelectedStellarObject");

            OnPropertyChanged("Degree");
            OnPropertyChanged("IsSurfaceLocation");
            OnPropertyChanged("Distance");
            OnPropertyChanged("Position");
            OnPropertyChanged("MaxHousing");
            OnPropertyChanged("MaxHangars");
        }

        internal void ChangeContext()
        {
            if (SelectedType == LocationType.Planet || SelectedType == LocationType.Moon)
            {
                ValidOrbiters.Clear();
                if (SelectedType == LocationType.Moon)
                {
                    foreach (var s in MainControl.main.StarSystem)
                    {
                        if (s.type == LocationType.Planet)
                        {
                            ValidOrbiters.Add(s);
                        }
                    }
                }
                else
                {
                    foreach (var s in MainControl.main.StarSystem)
                    {
                        if (s.type == LocationType.Star)
                        {
                            ValidOrbiters.Add(s);
                        }
                    }
                }
                ShowForStellar = Visibility.Visible;
                HideForStellar = Visibility.Hidden;
            }
            else if (SelectedType == LocationType.Location)
            {
                ValidOrbiters = new ObservableCollection<StellarObject>(MainControl.main.StarSystem);
                ShowForStellar = Visibility.Hidden;
                HideForStellar = Visibility.Visible;
            }
            OnPropertyChanged("ValidOrbiters");
        }

        internal void ChangeAvailableFor()
        {
            StarSystem.Clear();
            if (DisplayType == LocationType.Location)
            {
                foreach (var s in MainControl.main.Locations)
                {
                    if (s.type == DisplayType)
                        StarSystem.Add(s);
                }
            }
            else
            {
                foreach (var s in MainControl.main.StarSystem)
                {
                    if (s.type == DisplayType)
                        StarSystem.Add(s);
                }
            }
            OnPropertyChanged("StarSystem");
        }

        public void SetValues()
        {
            if (type == LocationType.Location)
            {
                location.name = Name;
                location.type = SelectedType;
                location.orbiting = SelectedStellarObject;
                location.degree = double.Parse(Degree);
                location.IsOnSurface = IsSurfaceLocation;
                if (IsSurfaceLocation)
                {
                    location.distance = location.orbiting.radius;
                }
                else
                {
                    location.distance = double.Parse(Distance);
                }
                SetPosition();
                location.MaxHousing = int.Parse(MaxHousing);
                location.MaxHangars = int.Parse(MaxHangars);
            }
            else
            {
                stellar.name = Name;
                stellar.type = SelectedType;
                stellar.orbiting = SelectedStellarObject;
                stellar.degree = double.Parse(Degree);
                stellar.distance = double.Parse(Distance);
                SetPosition();
                NotifyAll();
            }
        }

        public void Save()
        {
            bool exists = false;
            SetValues();

            if (SelectedType != LocationType.Location)
            {
                StellarObject old = new StellarObject();
                foreach (StellarObject s in MainControl.main.StarSystem)
                {
                    if (s.name == stellar.name)
                    {
                        old = s;
                        exists = true;
                    }
                }
                if (exists)
                {
                    MainControl.main.StarSystem.Remove(old);
                    MainControl.main.StarSystem.Add(stellar);
                    MainControl.main.SaveToFile(2);
                }
                else
                {
                    MainControl.main.StarSystem.Add(stellar);
                    MainControl.main.SaveToFile(2);
                }
            }
            else if (SelectedType == LocationType.Location)
            {
                Location old = new Location();
                foreach (Location l in MainControl.main.Locations)
                {
                    if (l.name == location.name)
                    {
                        old = l;
                        exists = true;
                    }
                }
                if (exists)
                {
                    MainControl.main.Locations.Remove(old);
                    MainControl.main.Locations.Add(location);
                    MainControl.main.SaveToFile(1);
                }
                else
                {
                    MainControl.main.Locations.Add(location);
                    MainControl.main.SaveToFile(1);
                }
            }
        }

        public void AddProduction()
        {
            ProductionEditor pe = new ProductionEditor(location);

            pe.Show();
        }

        public void SetPosition()
        {
            if (location.orbiting != null && Degree != null && Degree.Length > 0)
            {
                if (SelectedType == LocationType.Location)
                {
                    if (IsSurfaceLocation)
                    {
                        location.SetPosition(location.orbiting.radius, double.Parse(Degree));
                    }
                    else
                    {
                        location.SetPosition(double.Parse(Distance), double.Parse(Degree));
                    }
                }
                else
                {
                    stellar.SetPosition(double.Parse(Distance), double.Parse(Degree));
                }
            }
            OnPropertyChanged("Position");
        }
    }
}