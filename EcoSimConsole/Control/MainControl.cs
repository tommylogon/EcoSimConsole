using EcoSimConsole.Data;
using EcoSimConsole.Data.LocationModules;
using EcoSimConsole.Windows;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Timer = System.Timers.Timer;

namespace EcoSimConsole.Control
{
    public class MainControl : TopControl
    {
        public static MainControl main;
        private static Timer aTimer;

        private Visibility displayLocations = Visibility.Visible;
        private Visibility displayCitizens = Visibility.Hidden;

        private ObservableCollection<Location> locations = new ObservableCollection<Location>();
        private ObservableCollection<StellarObject> starSystem = new ObservableCollection<StellarObject>();
        private ObservableCollection<Citizen> citizens = new ObservableCollection<Citizen>();
        private ObservableCollection<Commodity> commodities = new ObservableCollection<Commodity>();
        private ObservableCollection<Recipie> recipies = new ObservableCollection<Recipie>();
        private ObservableCollection<MarketInfo> _market = new ObservableCollection<MarketInfo>();

        private Location _selectedLocation;
        private Citizen _selectedCitizen;
        private Production _SelectedProduction;
        

        public string pathCommodities = "Commodities.json";
        public string pathStarSystem = "StarSystem.json";
        public string pathLocations = "Locations.json";

        public MainControl()
        {
            main = this;
           

            SetupStarSystem();
            SetupCommodities();
            SetUpLocations();
            SetUpCitizen();
            SetTimer();

            if(Series != null)
            {
                var s = "";
            }
        }

        public ObservableCollection<ISeries> Series { get; set; } = new ObservableCollection<ISeries>
        {
            new ScatterSeries<ObservablePoint>
            {
                Values = new ObservableCollection<ObservablePoint>
                {
                    new ObservablePoint(0, 0),
                    new ObservablePoint(-17993730.460512351,-6549182.2910355795),
                    new ObservablePoint(12655408.918361047,-2231490.0442055846),
                    
                }
            }
        };


        internal void OpenModuleViewer()
        {
            if(SelectedLocation != null)
            {

                LocationModulesEditor LocModEditor = new LocationModulesEditor(SelectedLocation);

                LocModEditor.ShowDialog();
            }
        }

        

       

        internal void ResetPrices()
        {
            aTimer.Stop();

            foreach (var location in Locations)
            {
                //foreach (var production in location.Productions)
                //{
                //    foreach (var consumed in production.Requirements)
                //    {
                //        consumed.price = Commodities.First(c => c.Name == consumed.commodity.Name).defaultBuyPrice;
                //    }
                //    production.price = Commodities.First(c => c.Name == production.commodity.Name).defaultSellPrice;
                //}
                //foreach (var production in location.Buying)
                //{
                //    production.price = Commodities.First(c => c.Name == production.commodity.Name).defaultBuyPrice;
                //}
            }
            SaveToFile(1);
            aTimer.Start();
        }

        internal void CreateNewLocation()
        {
            LocationEditor locationEditor = new LocationEditor();
            locationEditor.Show();
        }

        private void SetupCommodities()
        {
            if (File.Exists(pathCommodities))
            {
                string json = File.ReadAllText(pathCommodities);
                Commodities = JsonConvert.DeserializeObject<ObservableCollection<Commodity>>(json);
            }
            else
            {
                Commodity commodity0 = new Commodity()
                {
                    ID = 0,
                    Name = "Agricultural Supplies"
                };
                Commodity commodity1 = new Commodity()
                {
                    ID = 1,
                    Name = "Medical Supplies"
                };
                Commodity commodity2 = new Commodity()
                {
                    ID = 2,
                    Name = "Titanium"
                };
                Commodity commodity3 = new Commodity()
                {
                    ID = 3,
                    Name = "Processed Food"
                };

                commodities.Add(commodity0);
                commodities.Add(commodity1);
                commodities.Add(commodity2);
                commodities.Add(commodity3);

                SaveToFile(0);
            }
        }

        internal void OpenProductionEditor()
        {
            ProductionEditor productionWindow = new ProductionEditor();

            productionWindow.ShowDialog();
        }

        internal void SaveToFile(int fileType)
        {
            if (fileType == 0)
            {
                string json = JsonConvert.SerializeObject(Commodities, Formatting.Indented);
                if (File.Exists(pathCommodities))
                {
                    try
                    {
                        File.Delete(pathCommodities);
                        File.WriteAllText(pathCommodities, json);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                File.WriteAllText(pathCommodities, json);
            }
            else if (fileType == 1)
            {
                string json = JsonConvert.SerializeObject(locations, Formatting.Indented);
                if (File.Exists(pathLocations))
                {
                    try
                    {
                        File.Delete(pathLocations);
                        File.WriteAllText(pathLocations, json);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                File.WriteAllText(pathLocations, json);
            }
            else if (fileType == 2)
            {
                string json = JsonConvert.SerializeObject(StarSystem, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
                if (File.Exists(pathStarSystem))
                {
                    try
                    {
                        File.Delete(pathStarSystem);
                        File.WriteAllText(pathStarSystem, json);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                File.WriteAllText(pathStarSystem, json);
            }
        }

        public ObservableCollection<Location> Locations
        {
            get
            {
                return locations;
            }
            set
            {
                if (locations != value)
                {
                    locations = value;
                    OnPropertyChanged("Locations");
                }
            }
        }

        public ObservableCollection<Citizen> Citizens
        {
            get
            {
                return citizens;
            }
            set
            {
                if (citizens != value)
                {
                    citizens = value;
                    OnPropertyChanged("Citizens");
                }
            }
        }

        public ObservableCollection<Commodity> Commodities
        {
            get
            {
                return commodities;
            }
            set
            {
                if (commodities != value)
                {
                    commodities = value;
                    OnPropertyChanged("Commodities");
                }
            }
        }

        public ObservableCollection<StellarObject> StarSystem
        {
            get
            {
                return starSystem;
            }
            set
            {
                if (starSystem != value)
                {
                    starSystem = value;
                    OnPropertyChanged("StarSystem");
                }
            }
        }

        public ObservableCollection<MarketInfo> MarketInfos
        {
            get
            {
                return _market;
            }
            set
            {
                if (_market != value)
                {
                    _market = value;
                    OnPropertyChanged("MarketInfos");
                }
            }
        }

        public Location SelectedLocation
        {
            get
            {
                return _selectedLocation;
            }
            set
            {
                if (_selectedLocation != value)
                {
                    _selectedLocation = value;
                    OnPropertyChanged("SelectedLocation");
                }
            }
        }

        public Citizen SelectedCitizen
        {
            get
            {
                return _selectedCitizen;
            }
            set
            {
                if (_selectedCitizen != value)
                {
                    _selectedCitizen = value;
                    OnPropertyChanged("SelectedCitizen");
                }
            }
        }

        public Production SelectedProduction
        {
            get
            {
                return _SelectedProduction;
            }
            set
            {
                if (_SelectedProduction != value)
                {
                    _SelectedProduction = value;
                    OnPropertyChanged("SelectedProduction");
                }
            }
        }

        public Visibility DisplayLocations
        {
            get { return displayLocations; }
            set
            {
                if (displayLocations != value)
                {
                    displayLocations = value;
                    OnPropertyChanged("DisplayLocations");
                }
            }
        }

        public Visibility DisplayCitizens
        {
            get { return displayCitizens; }
            set
            {
                if (displayCitizens != value)
                {
                    displayCitizens = value;
                    OnPropertyChanged("DisplayCitizens");
                }
            }
        }

        private void SetTimer()
        {
            aTimer = new Timer(6000);

            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            foreach (var entity in locations)
            {
                if (!entity.isUpdating)
                {
                    entity.isUpdating = true;
                    entity.OnUpdate();
                }
            }
            foreach (Citizen entity in citizens)
            {
                if (!entity.isUpdating)
                {
                    entity.isUpdating = true;
                    entity.OnUpdate();
                }
            }
            
        }

        private void SetupStarSystem()
        {
            if (File.Exists(pathStarSystem))
            {
                string json = File.ReadAllText(pathStarSystem);
                StarSystem = JsonConvert.DeserializeObject<ObservableCollection<StellarObject>>(json);
            }
            else
            {
                StellarObject Stanton = new StellarObject()
                {
                    name = "Stanton",
                    type = LocationType.Star,
                    X = 0,
                    Y = 0,
                };
                StellarObject Crusader = new StellarObject()
                {
                    name = "Crusader",
                    type = LocationType.Planet,
                    orbiting = Stanton,
                };
                
                Crusader.SetPosition(19148528, 200);
                Stanton.Satellites.Add(Crusader);

                StellarObject Cellin = new StellarObject()
                {
                    name = "Cellin",
                    type = LocationType.Moon,
                    orbiting = Crusader,
                };
                Cellin.SetPosition(50684, 15);
                Crusader.Satellites.Add(Cellin);

                StellarObject Daymar = new StellarObject()
                {
                    name = "Daymar",
                    type = LocationType.Moon,
                    orbiting = Crusader,
                };
                Daymar.SetPosition(63280, 200);
                Crusader.Satellites.Add(Daymar);
                StellarObject Yela = new StellarObject()
                {
                    name = "Yela",
                    type = LocationType.Moon,
                    orbiting = Crusader,
                };
                Yela.SetPosition(79288, 300);
                Crusader.Satellites.Add(Yela);

                StarSystem.Add(Stanton);
                

                SaveToFile(2);
            }

            PopulateMap();


        }

        private void PopulateMap()
        {


            foreach(var obj in StarSystem)
            {
                
              /*  Ellipse vfx = new Ellipse();
                vfx.Width = o.radius;
                vfx.canv
                o.

                    StarMap.Children.Add(vfx)*/
            }
            
        }

        public void AddLocationToStellerObject(string orbitTarget, Location location, double x, double y)
        {
             
            StellarObject target = null;
            foreach (var star in StarSystem)
            {
                target = GetSatellites(star, orbitTarget);
                if(target != null)
                {
                    break;
                }
                
            }

            if (target != null)
            {
                location.orbiting = target;
                target.Satellites.Add(location);
                location.SetPosition(x, y);
            }

            
        }
        public StellarObject GetSatellites(StellarObject parent, string orbitTarget)
        {
            if(parent.Name == orbitTarget)
            {
                return parent;
            }
            else if(parent.Satellites.Count > 0)
            {
                foreach (var satellite in parent.Satellites)
                {
                    var target = GetSatellites(satellite, orbitTarget);
                    if(target != null)
                    {
                        return target;
                    }
                }
            }
            
            return null;
        }
        private void SetUpLocations()
        {
            Locations = new ObservableCollection<Location>();

            if (File.Exists(pathLocations))
            {
                string json = File.ReadAllText(pathLocations);
                Locations = JsonConvert.DeserializeObject<ObservableCollection<Location>>(json);
            }
            else
            {
                try
                {
                    Location olisar = new Location()
                    {
                    name = "Port Olisar",
                    type = LocationType.Location,
                    MaxHangars = 50,
                    FreeHangars = 50,
                    MaxHousing = 50,
                    FreeHousing = 25,
                    
                    };
                    AddLocationToStellerObject("Crusader", olisar, 17660, 225);

                    Location tm = new Location()
                    {
                        name = "Tram and Myhers",
                        type = LocationType.Location,
                        MaxHangars = 3,
                        FreeHangars = 3,
                        MaxHousing = 25,
                        FreeHousing = 25,

                    };
                    MiningModule mm = new MiningModule(tm);
                    Recipie recipie = new Recipie();
                    recipie.Produced = new ResourceData(Commodities.First(co => co.ID == 5), 10);
                    mm.Mining.Recipies.Add(recipie);
                    tm.Modules.Add(mm);
                    AddLocationToStellerObject("Cellin", tm, 260.5, 15);

                    //Production osrs1 = new Production()
                    //{
                    //    commodity = commodities.First(c => c.ID == 0),
                    //    price = 1.01,
                    //    maxStorage = 2304000,
                    //    maxNetRate = 2419
                    //};

                    //Production ors2 = new Production()
                    //{
                    //    commodity = commodities.First(c => c.ID == 1),
                    //    price = 15.75,
                    //    maxStorage = 50000,
                    //    maxNetRate = 20,
                    //    requiresSomething = true,
                    //};

                    //Production ors3 = new Production()
                    //{
                    //    commodity = commodities.First(c => c.ID == 2),
                    //    price = 8.9,
                    //    maxStorage = 2304000,
                    //    maxNetRate = 2000
                    //};

                    //Production ors4 = new Production()
                    //{
                    //    commodity = commodities.First(c => c.ID == 3),
                    //    price = 1.5,
                    //    maxStorage = 2304000,
                    //    maxNetRate = 10000
                    //};

                    //Location grimhex = new Location()
                    //{
                    //    name = "GrimHex",
                    //    type = LocationType.Location,
                    //    maxHangars = 43,
                    //    freeHangars = 43,
                    //    maxHousing = 500,
                    //    freeHousing = 500,
                    //    orbiting = StarSystem.First(s => s.name == "Yela"),
                    //};
                    //grimhex.SetPosition(685, 12);
                    //Production ghrs1 = new Production()
                    //{
                    //    commodity = commodities.First(c => c.ID == 2),
                    //    price = 8.9,
                    //    maxStorage = 2304000,
                    //    maxNetRate = 2000
                    //};
                    //grimhex.consumedResource.Add(ghrs1);


                    //tm.SetPosition(260.5, 15);

                    //Production tmrs1 = new Production();
                    //tmrs1.commodity = commodities.First(c => c.ID == 2);
                    //tmrs1.price = 7.1;
                    //tmrs1.maxStorage = 576000;
                    //tmrs1.maxNetRate = 933;

                    //Production tmrs2 = new Production();
                    //tmrs2.commodity = commodities.First(c => c.ID == 1);
                    //tmrs2.price = 19.25;
                    //tmrs2.maxStorage = 5000;
                    //tmrs2.maxNetRate = 3;

                    //Production tmrs3 = new Production()
                    //{
                    //    commodity = commodities.First(c => c.ID == 3),
                    //    price = 1.5,
                    //    maxStorage = 576000,
                    //    maxNetRate = 1500,
                    //    requiresSomething = true
                    //};

                    //Location daymar1 = new Location()
                    //{
                    //    name = "Bountiful Harvest Hydroponics",
                    //    type = LocationType.Location,
                    //    maxHangars = 3,
                    //    freeHangars = 3,
                    //    maxHousing = 25,
                    //    freeHousing = 25,
                    //    orbiting = StarSystem.First(s => s.name == "Daymar")
                    //};

                    //daymar1.SetPosition(295, 32);
                    //Production daymar1rs1 = new Production()
                    //{
                    //    commodity = commodities.First(c => c.ID == 0),
                    //    price = 1.2,
                    //    maxStorage = 576000,
                    //    maxNetRate = 1092
                    //};
                    //Production daymar1rs2 = new Production()
                    //{
                    //    commodity = commodities.First(c => c.ID == 3),
                    //    price = 1.21,
                    //    maxStorage = 20000000,
                    //    maxNetRate = 200000,
                    //};
                    //Production bntyrs3 = new Production()
                    //{
                    //    commodity = commodities.First(c => c.ID == 1),
                    //    price = 19.25,
                    //    maxStorage = 5000,
                    //    maxNetRate = 5,
                    //};

                    //Location deakins = new Location()
                    //{
                    //    name = "Deakins Research Outpost",
                    //    type = LocationType.Location,
                    //    _maxHangars = 4,
                    //    _freeHangars = 4,
                    //    _maxHousing = 30,
                    //    _freeHousing = 30,
                    //    orbiting = StarSystem.First(s => s.name == "Yela")
                    //};
                    //deakins.SetPosition(313, 45);
                    //Production dkrs1 = new Production()
                    //{
                    //    commodity = commodities.First(c => c.ID == 0),
                    //    price = 1.01,
                    //    maxStorage = 176000,
                    //    maxNetRate = 2038
                    //};
                    //Production dkrs2 = new Production()
                    //{
                    //    commodity = commodities.First(c => c.ID == 3),
                    //    price = 1.5,
                    //    maxStorage = 576000,
                    //    maxNetRate = 2000,
                    //    requiresSomething = true
                    //};
                    //Production dkrs3 = new Production()
                    //{
                    //    commodity = commodities.First(c => c.ID == 1),
                    //    price = 15.75,
                    //    maxStorage = 200000,
                    //    maxNetRate = 1000,
                    //    requiresSomething = true
                    //};
                    //deakins.producedResource.Add(dkrs1);
                    //deakins.consumedResource.Add(dkrs2);
                    //deakins.producedResource.Add(dkrs3);

                    //olisar.producedResource.Add(osrs1);
                    //olisar.producedResource.Add(ors2);
                    //olisar.consumedResource.Add(ors3);
                    //olisar.consumedResource.Add(ors4);

                    //tm.consumedResource.Add(tmrs3);
                    //tm.consumedResource.Add(tmrs2);
                    //tm.producedResource.Add(tmrs1);

                    //daymar1.consumedResource.Add(daymar1rs1);
                    //daymar1.producedResource.Add(daymar1rs2);
                    //daymar1.consumedResource.Add(bntyrs3);

                    Locations.Add(olisar);
                    Locations.Add(tm);
                    //Locations.Add(daymar1);
                    //Locations.Add(grimhex);
                    //Locations.Add(deakins);

                    //SaveToFile(1);
                }
               catch (Exception)
               {
                   throw;
               }
            }
        }

        private void SetUpCitizen()
        {
            var ships = new List<Ship>();
            var fNames = new List<string>()
            {
                "James",
                "John",
                "Robert",
                "Michael",
                "William",
                "David",
                "Richard",
                "Joseph",
                "Thomas",
                "Charles",
                "Christopher",
                "Daniel",
                "Matthew",
                "Anthony",
                "Donald",
                "Mark",
                "Paul",
                "Steven",
                "Andrew",
                "Kenneth",
                "Joshua",
                "Kevin",
                "Brian",
                "George",
                "Edward",
                "Ronald",
                "Timothy",
                "Jason",
                "Jeffrey",
                "Ryan",
                "Jacob",
                "Gary",
                "Nicholas",
                "Eric",
                "Jonathan",
                "Stephen",
                "Larry",
                "Justin",
                "Scott",
                "Brandon",
                "Benjamin",
                "Samuel",
                "Frank",
                "Gregory",
                "Raymond",
                "Alexander",
                "Patrick",
                "Jack",
                "Dennis",
                "Jerry",
                "Tyler",
                "Aaron",
                "Jose",
                "Henry",
                "Adam",
                "Douglas",
                "Nathan",
                "Peter",
                "Zachary",
                "Kyle",
                "Walter",
                "Harold",
                "Jeremy",
                "Ethan",
                "Carl",
                "Keith",
                "Roger",
                "Gerald",
                "Christian",
                "Terry",
                "Sean",
                "Arthur",
                "Austin",
                "Noah",
                "Lawrence",
                "Jesse",
                "Joe",
                "Bryan",
                "Billy",
                "Jordan",
                "Albert",
                "Dyla",
                "Bruce",
                "Willie",
                "Gabriel",
                "Alan",
                "Juan",
                "Logan",
                "Wayne",
                "Ralph",
                "Roy",
                "Eugene",
                "Randy",
                "Vincent",
                "Russell",
                "Louis",
                "Philip",
                "Bobby",
                "Johnny",
                "Bradley",
                "Mary",
                "Patricia",
                "Jennifer",
                "Linda",
                "Elizabeth",
                "Barbara",
                "Susan",
                "Jessica",
                "Sarah",
                "Karen",
                "Nancy",
                "Lisa",
                "Margaret",
                "Betty",
                "Sandra",
                "Ashley",
                "Dorothy",
                "Kimberly",
                "Emily",
                "Donna",
                "Michelle",
                "Carol",
                "Amanda",
                "Melissa",
                "Deborah",
                "Stephanie",
                "Rebecca",
                "Laura",
                "Sharon",
                "Cynthia",
                "Kathleen",
                "Amy",
                "Shirley ",
                "Angela",
                "Helen",
                "Anna",
                "Brenda",
                "Pamela",
                "Nicole",
                "Samantha",
                "Katherine",
                "Emma",
                "Ruth",
                "Christine",
                "Catherine",
                "Debra",
                "Rachel",
                "Carolyn",
                "Janet",
                "Virginia",
                "Maria",
                "Heather",
                "Diane",
                "Julie",
                "Joyce",
                "Victoria",
                "Kelly",
                "Christina",
                "Lauren",
                "Joan",
                "Evelyn",
                "Olivia",
                "Judith",
                "Megan",
                "Cheryl",
                "Martha",
                "Andrea",
                "Frances",
                "Hannah",
                "Jacqueline",
                "Ann",
                "Gloria",
                "Jean",
                "Kathryn",
                "Alice",
                "Teresa",
                "Sara",
                "Janice",
                "Doris",
                "Madison",
                "Julia",
                "Grace",
                "Judy",
                "Abigail",
                "Marie",
                "Denise",
                "Beverly",
                "Amber",
                "Theresa",
                "Marilyn",
                "Daniell",
                "Diana",
                "Brittany",
                "Natalie",
                "Sophia",
                "Rose",
                "Isabella",
                "Alexis",
                "Kayla",
            };
            var lNames = new List<string>()
            {
                "Smith",
                "Johnson",
                "Williams",
                "Brown",
                "Jones",
                "Miller",
            };

            Ship hullc = new Ship()
            {
                name = "Hull C",
                maxSCU = 460800,
                maxPassengeers = 0,
                currentSpeed = 0,
                scmSpeed = 0.1,
                quantum1Speed = 637,
                quantum2Speed = 4924,
                quantumMaxSpeed = 69893
            };
            Ship C2 = new Ship()
            {
                name = "Hercules C2",
                maxSCU = 62400,
                maxPassengeers = 0,
                currentSpeed = 0,
                scmSpeed = 0.1,
                quantum1Speed = 1100,
                quantum2Speed = 8690,
                quantumMaxSpeed = 208561
            };
            Ship hulla = new Ship()
            {
                name = "Hull A",
                maxSCU = 4800,
                maxPassengeers = 0,
                currentSpeed = 0,
                scmSpeed = 0.1,
                quantum1Speed = 637,
                quantum2Speed = 8186,
                quantumMaxSpeed = 76969
            };
            Ship cat = new Ship()
            {
                name = "Catterpillar",
                maxSCU = 57600,
                maxPassengeers = 0,
                currentSpeed = 0,
                scmSpeed = 0.1,
                quantum1Speed = 1000,
                quantum2Speed = 8342,
                quantumMaxSpeed = 188698
            };
            Ship aurora = new Ship()
            {
                name = "Aurora ES",
                maxSCU = 300,
                maxPassengeers = 0,
                currentSpeed = 0,
                scmSpeed = 0.2,
                quantum1Speed = 920,
                quantum2Speed = 2284,
                quantumMaxSpeed = 138544
            };
            Ship cutlass = new Ship()
            {
                name = "Cutlass Black",
                maxSCU = 4600,
                maxPassengeers = 6,
                currentSpeed = 0,
                scmSpeed = 0.3,
                quantum1Speed = 750,
                quantum2Speed = 9630,
                quantumMaxSpeed = 124143,
                HP = 3880,
                ShieldsHP = 14584,
                fuel = 2500
            };
            Ship freelancer = new Ship()
            {
                name = "Freelancer",
                maxSCU = 6600,
                maxPassengeers = 0,
                currentSpeed = 0,
                scmSpeed = 0.3,
                quantum1Speed = 750,
                quantum2Speed = 9630,
                quantumMaxSpeed = 124143,
                HP = 3880,
                ShieldsHP = 14584,
                fuel = 2500
            };
            Ship avenger = new Ship()
            {
                name = "Avegner Titan",
                maxSCU = 800,
                maxPassengeers = 0,
                currentSpeed = 0,
                scmSpeed = 0.3,
                quantum1Speed = 625,
                quantum2Speed = 3725,
                quantumMaxSpeed = 283046,
                HP = 3880,
                ShieldsHP = 14584,
                fuel = 2500
            };
            Ship nomad = new Ship()
            {
                name = "Nomad",
                maxSCU = 2400,
                maxPassengeers = 0,
                currentSpeed = 0,
                scmSpeed = 0.3,
                quantum1Speed = 1035,
                quantum2Speed = 3426,
                quantumMaxSpeed = 201112,
                HP = 3880,
                ShieldsHP = 14584,
                fuel = 2500
            };
            Ship reliant = new Ship()
            {
                name = "Reliant Kore",
                maxSCU = 600,
                maxPassengeers = 0,
                currentSpeed = 0,
                scmSpeed = 0.3,
                quantum1Speed = 870,
                quantum2Speed = 2781,
                quantumMaxSpeed = 169828,
                HP = 3880,
                ShieldsHP = 14584,
                fuel = 2500
            };
            Ship origin135c = new Ship()
            {
                name = "135C",
                maxSCU = 600,
                maxPassengeers = 0,
                currentSpeed = 0,
                scmSpeed = 0.3,
                quantum1Speed = 625,
                quantum2Speed = 3724,
                quantumMaxSpeed = 283046,
                HP = 3880,
                ShieldsHP = 14584,
                fuel = 2500
            };
            Ship auraoacl = new Ship()
            {
                name = "Aurora CL",
                maxSCU = 600,
                maxPassengeers = 0,
                currentSpeed = 0,
                scmSpeed = 0.3,
                quantum1Speed = 637,
                quantum2Speed = 2110,
                quantumMaxSpeed = 92363,
                HP = 3880,
                ShieldsHP = 14584,
                fuel = 2500
            };
            for (int i = 0; i < 10; i++)
            {
                ships.Add(aurora);
                ships.Add(auraoacl);
                ships.Add(origin135c);
                ships.Add(reliant);
            }
            for (int i = 0; i < 5; i++)
            {
                ships.Add(cutlass);
                ships.Add(freelancer);
                ships.Add(hulla);
                ships.Add(nomad);
                ships.Add(avenger);
            }
            for (int i = 0; i < 2; i++)
            {
                ships.Add(cat);
                ships.Add(hullc);
                ships.Add(C2);
            }

            //move to other thread
            for (int i = 0; i < 0; i++)
            {
                Random rand = new Random();
                Citizen newCitizen = new Citizen();
                newCitizen.name = fNames[rand.Next(0, fNames.Count)] + " " + lNames[rand.Next(0, lNames.Count)];
                newCitizen.uec = rand.Next(1000, 50000);
                newCitizen.currentLocation = (Location)locations[rand.Next(0, locations.Count)];
                newCitizen.ownedShip = new Ship(ships[rand.Next(0, ships.Count)]); ;
                newCitizen.currentState = State.Idle;
                newCitizen.risk = rand.NextDouble();
                newCitizen.patience = rand.NextDouble();
                newCitizen.position = newCitizen.currentLocation.position;
                newCitizen.ownedShip.Owner = newCitizen;

                Thread.Sleep(25);
                newCitizen.patience = rand.NextDouble();
                Citizens.Add(newCitizen);
            }
        }

        public void ViewCitizens(object sender, RoutedEventArgs e)
        {
            DisplayCitizens = Visibility.Visible;
            DisplayLocations = Visibility.Hidden;
        }

        public void ViewLocations(object sender, RoutedEventArgs e)
        {
            DisplayCitizens = Visibility.Hidden;
            DisplayLocations = Visibility.Visible;
        }
    }

    public struct MarketInfo
    {
        public Commodity commodity;
        public double minSellPrice;
        public double maxBuyPrice;
    }
}