using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSimConsole.Data
{
    public class Ship : Entity
    {
        private Citizen _owner;
        public int distanceToDestination;

        public double currentSpeed;
        public double scmSpeed;
        public double quantum1Speed;
        public double quantum2Speed;
        public double quantumMaxSpeed;
        public int maxSCU;
        public int maxPassengeers;
        public int maxData;
        public double fuel;
        public double dps;
        public double HP;
        public double ShieldsHP;
        public double Mass;
        public double ManuveringMOD;
        public double TargetingMOD;

        public List<CargoInfo> cargoManifest = new List<CargoInfo>();

        public Ship()
        {
        }

        public Ship(Ship orgShip)
        {
            name = orgShip.Name;
            maxSCU = orgShip.maxSCU;
            maxPassengeers = orgShip.maxPassengeers;
            scmSpeed = orgShip.scmSpeed;
            quantum1Speed = orgShip.quantum1Speed;
            quantum2Speed = orgShip.quantum2Speed;
            quantumMaxSpeed = orgShip.quantumMaxSpeed;
            HP = orgShip.HP;
            ShieldsHP = orgShip.ShieldsHP;
            fuel = orgShip.fuel;
        }

        public override void OnUpdate()
        {
            throw new NotImplementedException();
        }

        public Citizen Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                if (_owner != value)
                {
                    _owner = value;
                    OnPropertyChanged("Owner");
                }
            }
        }

        public int GetFreeCargo(CargoType cargoType)
        {
            int usedCargo = 0;
            if (cargoManifest.Count > 0)
            {
                foreach (var manifest in cargoManifest)
                {
                    if (manifest.cargoType == cargoType)
                    {
                        usedCargo += manifest.units;
                    }
                }
            }
            return maxSCU - usedCargo;
        }

        public void UpdateManifest(CargoInfo oldManifest, CargoInfo newManifest)
        {
            if (cargoManifest.Contains(oldManifest))
            {
                cargoManifest.Remove(oldManifest);
            }
            if (!cargoManifest.Contains(newManifest))
            {
                cargoManifest.Add(newManifest);
            }
        }
    }

    public struct CargoInfo
    {
        public Commodity commodity;
        public int units;
        public CargoType cargoType;
        public bool hidden;
        public double price;
    }

    public enum CargoType
    {
        SCU,
        Data,
        Passangers
    }
}