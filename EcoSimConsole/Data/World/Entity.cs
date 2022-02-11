using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Entity : INotifyPropertyChanged
    {
        [JsonProperty]
        public string name;

        [JsonProperty]
        public Position position;
        public bool isUpdating = false;
        public double X
        {
            get
            {
                return position.x;
            }
            set
            {
                if (position.x != value)
                    position.x = value;
            }
        }

        public double Y
        {
            get
            {
                return position.y;
            }
            set
            {
                if (position.y != value)
                    position.y = value;
            }
        }

        public double Z
        {
            get
            {
                return position.z;
            }
            set
            {
                if (position.z != value)
                    position.z = value;
            }
        }

        public string Name { get { return name; } }

        public abstract void OnUpdate();

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

    public struct Position
    {
        public double x;
        public double y;
        public double z;
    }
}