using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Data
{
    public class StellarObject : Entity
    {
        [JsonProperty]
        public double radius;

        [JsonProperty]
        public StellarObject orbiting;

        [JsonProperty]
        public ObservableCollection<StellarObject> satellites = new ObservableCollection<StellarObject>();

        [JsonProperty]
        public double distance;

        [JsonProperty]
        public double degree;

        public List<Location> locations = new List<Location>();

        [JsonProperty]
        public LocationType type;

        public StellarObject()
        {
            this.satellites = new ObservableCollection<StellarObject>();
        }

        public StellarObject(Location location)
        {
            radius = location.radius;
            name = location.name;
            type = location.type;
            orbiting = location.orbiting;
        }

        public string Orbiting
        {
            get
            {
                return orbiting.Name;
            }
            
        }
        public ObservableCollection<StellarObject> Satellites
        {
            get 
            {
                return satellites;
            } 
            set 
            {
                satellites = value;
            } 
        }
        public override void OnUpdate()
        {
            throw new NotImplementedException();
        }

        public void SetPosition(double distance, double degree)
        {
            X = distance * Math.Cos((Math.PI / 180) * degree);
            Y = distance * Math.Sin((Math.PI / 180) * degree);
            if (orbiting != null && orbiting.type != LocationType.Star)
            {
                X = orbiting.X - X;
                Y = orbiting.Y - Y;
            }
        }
    }

    public enum LocationType
    {
        Star,
        Planet,
        Moon,
        Asteroid,
        Location,
        Jumppoint
    }
}