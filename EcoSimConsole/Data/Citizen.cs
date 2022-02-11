using EcoSimConsole.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Xaml.Schema;
using static EcoSimConsole.Data.Ship;

namespace EcoSimConsole.Data
{
    public class Citizen : Entity
    {
        public double risk;
        public double patience;
        public double uec;
        public Ship ownedShip;
        public Location currentLocation;
        public Location destination;

        public State currentState;
        public List<Location> possibleDestinations = new List<Location>();

        private double distanceLeft = 0;
        private double totalDistance = 0;

        public int waitingForTicks = 0;

        private bool hasInitialHangar = false;

        public Citizen()
        {
            if (CurrentLocation != null)
            {
                SetInitialHangar();
                hasInitialHangar = true;
            }
        }

        public State CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                if (currentState != value)
                {
                    currentState = value;
                    OnPropertyChanged("CurrentState");
                }
            }
        }

        public string GetUEC
        {
            get
            {
                return "UEC: " + uec.ToString("C");
            }
        }

        public string BoredValue
        {
            get
            {
                return (waitingForTicks * 0.05).ToString("0.##") + "/" + patience.ToString("0.##");
            }
        }

        public string Speed
        {
            get
            {
                return ownedShip.currentSpeed + "/" + ownedShip.quantumMaxSpeed;
            }
        }

        public string CurrentPosition
        {
            get
            {
                return "X: " + X + "\r\nY: " + Y + "\r\nZ: " + Z;
            }
        }

        public Location CurrentLocation
        {
            get
            {
                return currentLocation;
            }
            set
            {
                if (currentLocation != value)
                {
                    currentLocation = value;
                    OnPropertyChanged("CurrentLocation");
                }
            }
        }

        public Location Destination
        {
            get
            {
                return destination;
            }
            set
            {
                if (destination != value)
                {
                    destination = value;
                    OnPropertyChanged("Destination");
                }
            }
        }

        public double DistanceLeft
        {
            get
            {
                return distanceLeft;
            }
            set
            {
                if (distanceLeft != value)
                {
                    distanceLeft = value;
                    OnPropertyChanged("DistanceLeft");

                    OnPropertyChanged("TimeLeft");
                }
            }
        }

        public string TimeLeft
        {
            get
            {
                if (ownedShip != null)
                {
                    return (DistanceLeft / ownedShip.currentSpeed).ToString("{0}");
                }
                else
                {
                    return "0";
                }
            }
        }

        public string GetShipInfo
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Ship: " + ownedShip.Name).AppendLine();
                builder.Append("Cargo: " + ownedShip.GetFreeCargo(CargoType.SCU) + "/" + ownedShip.maxSCU).AppendLine();
                foreach (var manifest in ownedShip.cargoManifest)
                {
                    builder.Append(manifest.commodity.Name + ", Units: " + manifest.units + ", price: " + manifest.price.ToString("C") + ", " + manifest.cargoType + ", Hidden: " + manifest.hidden).AppendLine();
                }
                return builder.ToString();
            }
        }

        public override void OnUpdate()
        {
            //var selectedRoute = new TradeInfo();
            //if (uec > 10000000)
            //{
            //    uec = 1000000;
            //}

            //if (CurrentState == State.Idle)
            //{
            //    if (!hasInitialHangar)
            //    {
            //        SetInitialHangar();
            //    }
            //    CurrentState = State.PlanningTradeRoute;
            //}
            //if (CurrentState == State.PlanningTradeRoute)
            //{
            //    if (ownedShip.GetFreeCargo(CargoType.SCU) > 0)
            //    {
            //        FindNewTraderoute();
            //        var routes = GenerateTradeRoutes();

            //        if (routes != null)
            //        {
            //            selectedRoute = SelectTradeRoute(routes);
            //            if (selectedRoute.startLocation == CurrentLocation)
            //            {
            //                CurrentState = State.Buying;
            //            }
            //            else
            //            {
            //                if (CurrentLocation.RequestTakeoff(ownedShip))
            //                {
            //                    CurrentState = State.Traveling;
            //                    Destination = selectedRoute.startLocation;
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        var routes = FindBuyer();
            //        if (routes != null)
            //        {
            //            selectedRoute = SelectTradeRoute(routes);
            //            if (selectedRoute.endLocation != null)
            //            {
            //                if (CurrentLocation.RequestTakeoff(ownedShip))
            //                {
            //                    CurrentState = State.Traveling;
            //                    Destination = selectedRoute.endLocation;
            //                }
            //            }
            //        }
            //    }
            //}
            //if (CurrentState == State.Buying)
            //{
            //    if (selectedRoute.startLocation != null)
            //    {
            //        if (BuyCommodity(selectedRoute))
            //        {
            //            TakeOff(selectedRoute);
            //        }
            //    }
            //    else if (Bored())
            //    {
            //        CurrentState = State.PlanningTradeRoute;
            //    }
            //}
            //if (CurrentState == State.Traveling)
            //{
            //    if (Destination != null && DistanceLeft > 0)
            //    {
            //        TravelToDestination();
            //    }
            //    else if (Destination != null && DistanceLeft <= 0)
            //    {
            //        DistanceLeft = 0;
            //        CurrentState = State.Landing;
            //    }
            //    else if (Destination == null)
            //    {
            //        CurrentState = State.PlanningTradeRoute;
            //    }
            //}
            //if (CurrentState == State.Landing)
            //{
            //    if (RequestLanding())
            //    {
            //        CurrentState = State.Selling;
            //    }
            //    else if (Bored())
            //    {
            //        CurrentState = State.PlanningTradeRoute;
            //    }
            //}
            //if (CurrentState == State.Selling)
            //{
            //    var state = SellCommodity();
            //    if (state == TradeState.Finished)
            //    {
            //        CurrentState = State.PlanningTradeRoute;
            //    }
            //    else if (state == TradeState.Failed && Bored())
            //    {
            //        CurrentState = State.PlanningTradeRoute;
            //    }
            //}

            isUpdating = false;
        }

        public void TakeOff(TradeInfo selectedRoute)
        {
            if (selectedRoute.startLocation.RequestTakeoff(ownedShip))
            {
                CurrentState = State.Traveling;
                Destination = selectedRoute.endLocation;
                DistanceLeft = GetDistance();
                totalDistance = DistanceLeft;
            }
        }

        /// <summary>
        /// Initial check to give the citizen a hangar at startup
        /// </summary>
        public void SetInitialHangar()
        {
            if (!currentLocation.ShipQueue.Contains(ownedShip) && !CurrentLocation.Hangars.Contains(ownedShip))
            {
                CurrentLocation.RequestLanding(ownedShip);
                X = CurrentLocation.X;
                Y = CurrentLocation.Y;
            }
        }

        /// <summary>
        /// Find the traderouts from current location, or from another locations.
        /// need a ship, money and free cargo space.
        /// if this location has is producing commodities then check where you can sell them.
        /// </summary>
        private void FindNewTraderoute()
        {
            //if (ownedShip != null && uec > 0 && currentLocation != null)
            //{
            //    possibleDestinations.Clear();

            //    if (CurrentLocation.Productions.Count > 0)
            //    {
            //        foreach (var selling in CurrentLocation.Productions)
            //        {
            //            foreach (Location buyer in MainControl.main.Locations)
            //            {
            //                if (buyer.name != CurrentLocation.name && buyer.Productions.Count > 0)
            //                {
            //                    foreach (var production in buyer.Productions)
            //                    {
            //                        if (production.Requirements.Count > 0)
            //                        {
            //                            foreach (var bought in production.Requirements)
            //                            {
            //                                if (selling.commodity.Name == bought.commodity.Name && bought.Price > selling.Price)
            //                                {
            //                                    possibleDestinations.Add(buyer);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
                    //foreach (Production selling in CurrentLocation.Selling)
                    //{
                    //    foreach (Location potentialLocation in MainControl.main.Locations)
                    //    {
                    //        if (potentialLocation.name != CurrentLocation.name)
                    //        {
                    //            foreach (Production buying in potentialLocation.Buying)
                    //            {
                    //                if (selling.commodity.Name == buying.commodity.Name && buying.Price > selling.Price)
                    //                {
                    //                    possibleDestinations.Add(potentialLocation);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    FindSellersFromOther();
            //    }
            //    else
            //    {
            //        FindSellersFromOther();
            //        //TravelToOtherLocation();
            //    }
            //}
        }

        public void FindSellersFromOther()
        {
            //foreach (var potentialStart in MainControl.main.Locations)
            //{
            //    foreach (var potentialStop in MainControl.main.Locations)
            //    {
            //        if (potentialStop.name != potentialStart.name && potentialStart.Productions.Count > 0 && potentialStop.Productions.Count > 0)
            //        {
            //            foreach (var selling in potentialStart.Productions)
            //            {
            //                foreach (var produced in potentialStop.Productions)
            //                {
            //                    if (produced.Requirements.Count > 0)
            //                    {
            //                        foreach (var bought in produced.Requirements)
            //                        {
            //                            if (bought.Commodity != null && selling.Commodity != null && selling.commodity.Name == bought.commodity.Name && bought.Price > selling.Price)
            //                            {
            //                                possibleDestinations.Add(potentialStart);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        //public List<TradeInfo> FindBuyer()
        //{
        //    //List<TradeInfo> tradeRoutes = new List<TradeInfo>();

            //foreach (var potentialStop in MainControl.main.Locations)
            //{
            //    foreach (var manifest in ownedShip.cargoManifest)
            //    {
            //        if (potentialStop.name != CurrentLocation.name && potentialStop.Productions.Count > 0)
            //        {
            //            foreach (var produced in potentialStop.Productions)
            //            {
            //                if (produced.Requirements.Count > 0)
            //                {
            //                    foreach (var bought in produced.Requirements)
            //                    {
            //                        if (manifest.commodity.ID == bought.commodity.ID && bought.Price > manifest.price)
            //                        {
            //                            GenerateTradeInfo(manifest, bought, potentialStop, tradeRoutes);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //return tradeRoutes;
        //}

        /// <summary>
        /// Create a list of all possible traderoutes.
        /// if fromCurrent is false, check possible traderouts where origin is another location.
        /// </summary>
        /// <param name="fromCurrent"></param>
        /// <returns>the possible traderoutes</returns>
        //private List<TradeInfo> GenerateTradeRoutes()
        //{
            //List<TradeInfo> tradeRoutes = new List<TradeInfo>();
            //if (possibleDestinations.Count > 0)
            //{
                //    foreach (var selling in currentLocation.producedResource)
                //    {
                //        foreach (var possibleBuyer in possibleDestinations)
                //        {
                //            foreach (var buying in possibleBuyer.consumedResource)
                //            {
                //                GenerateTradeInfo(selling, buying, CurrentLocation, possibleBuyer, tradeRoutes);
                //            }
                //        }
                //    }
                //    return tradeRoutes;

                //else
                //{
            //    foreach (var possibleSeller in possibleDestinations)
            //    {
            //        foreach (var possibleBuyer in possibleDestinations)
            //        {
            //            if (possibleSeller.Name != possibleBuyer.Name && possibleSeller.Productions.Count > 0)
            //            {
            //                foreach (var selling in possibleSeller.Productions)
            //                {
            //                    foreach (var buyerProduced in possibleBuyer.Productions)
            //                    {
            //                        if (buyerProduced.Requirements.Count > 0)
            //                        {
            //                            foreach (var bought in buyerProduced.Requirements)
            //                            {
            //                                GenerateTradeInfo(selling, bought, possibleSeller, possibleBuyer, tradeRoutes);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    return tradeRoutes;
            //    //}
            //    //else if (CurrentState == State.DecidingTrade && Destination == null)
            //    //{
            //    //    CurrentState = State.PlanningTrade;
            //    //}
            //}
            //return null;
       // }

        //private bool GenerateTradeInfo(Production selling, Production buying, Location possibleSeller, Location possibleBuyer, List<TradeInfo> tradeRoutes)
        //{
            //if (selling.commodity.Name == buying.commodity.Name && selling.CurrentStorage > 0)
            //{
            //    TradeInfo route = new TradeInfo();
            //    route.endLocation = possibleBuyer;
            //    route.commodity = selling.commodity;
            //    route.profit = CalculateDecision(selling, buying);

            //    if (possibleSeller != null)
            //    {
            //        route.startLocation = possibleSeller;
            //    }

            //    if (tradeRoutes.Count > 0)
            //    {
            //        bool exists = false;
            //        foreach (var ti in tradeRoutes)
            //        {
            //            if (route.commodity == ti.commodity && route.endLocation == ti.endLocation)
            //            {
            //                exists = true;
            //                return false;
            //            }
            //            else
            //            {
            //                exists = false;
            //            }
            //        }
            //        if (!exists)
            //        {
            //            tradeRoutes.Add(route);
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        tradeRoutes.Add(route);
            //        return true;
            //    }
            //}

            //return false;
        //}

        //private bool GenerateTradeInfo(CargoInfo manifest, Production buying, Location possibleBuyer, List<TradeInfo> tradeRoutes)
        //{
            //if (manifest.commodity.ID == buying.commodity.ID && manifest.units > 0)
            //{
            //    TradeInfo route = new TradeInfo();
            //    route.startLocation = CurrentLocation;
            //    route.endLocation = possibleBuyer;
            //    route.commodity = manifest.commodity;
            //    route.profit = (buying.Price - manifest.price) * manifest.units;

            //    if (tradeRoutes.Count > 0)
            //    {
            //        bool exists = false;
            //        foreach (var ti in tradeRoutes)
            //        {
            //            if (route.commodity == ti.commodity && route.endLocation == ti.endLocation)
            //            {
            //                exists = true;
            //                return false;
            //            }
            //            else
            //            {
            //                exists = false;
            //            }
            //        }
            //        if (!exists)
            //        {
            //            tradeRoutes.Add(route);
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        tradeRoutes.Add(route);
            //        return true;
            //    }
            //}

            //return false;
       // }

        private TradeInfo SelectTradeRoute(List<TradeInfo> tradeRoutes)
        {
            TradeInfo selectedRoute = new TradeInfo();
            if (tradeRoutes.Count > 0)
            {
                selectedRoute = tradeRoutes[0];

                foreach (TradeInfo route in tradeRoutes)
                {
                    if (route.commodity.Name == "Diamond")
                    {
                        Console.WriteLine("Brek");
                    }
                    if (route.startLocation != null && route.endLocation != null)
                    {
                        if (selectedRoute.startLocation != route.endLocation)
                        {
                            if (selectedRoute.endLocation == route.endLocation && selectedRoute.commodity != route.commodity)
                            {
                                if (selectedRoute.profit < route.profit)
                                {
                                    selectedRoute = route;
                                }
                            }
                            else if (selectedRoute.endLocation != route.endLocation)
                            {
                                if (selectedRoute.profit < route.profit)
                                {
                                    selectedRoute = route;
                                }
                            }
                        }
                    }
                }

                return selectedRoute;
            }
            return selectedRoute;
        }

        public string Cargo
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (ownedShip.cargoManifest.Count > 0)
                {
                    foreach (var manifest in ownedShip.cargoManifest)
                    {
                        sb.Append(manifest.commodity.Name + " : " + manifest.units).AppendLine();
                    }
                }
                return sb.ToString();
            }
        }

        //public double CalculateDecision(Production selling, Production buying)
        //{
        //    int maxBuyableUnits = 0;
        //    int maxAvailable = selling.CurrentStorage;
        //    int maxRiskedUnits = 0;
        //    double max = uec * risk / selling.Price;
        //    if (max > maxAvailable)
        //    {
        //        maxRiskedUnits = maxAvailable;
        //    }
        //    else
        //    {
        //        maxRiskedUnits = Convert.ToInt32(max);
        //    }

        //    if (maxAvailable > ownedShip.GetFreeCargo(CargoType.SCU))
        //    {
        //        maxBuyableUnits = ownedShip.GetFreeCargo(CargoType.SCU);
        //    }
        //    if (maxAvailable < ownedShip.GetFreeCargo(CargoType.SCU))
        //    {
        //        maxBuyableUnits = maxAvailable;
        //    }
        //    if (maxBuyableUnits > maxRiskedUnits)
        //    {
        //        maxBuyableUnits = maxRiskedUnits;
        //    }
        //    return (maxBuyableUnits * buying.Price) - (maxBuyableUnits * selling.Price);
        //}

        private void TravelToDestination()
        {
            if (ownedShip.currentSpeed == 0 && DistanceLeft > 0)
            {
                Accelerate();
            }
            if (DistanceLeft / totalDistance > 0.5 && DistanceLeft > 0)
            {
                Accelerate();
            }
            else if (DistanceLeft / totalDistance < 0.5 && DistanceLeft > 0)
            {
                Decelerate();
            }

            DistanceLeft -= ownedShip.currentSpeed;
            OnPropertyChanged("CurrentPosition");
            OnPropertyChanged("Speed");
        }

        private void Decelerate()
        {
            if (Destination.IsOnSurface && DistanceLeft < Destination.orbiting.radius + 25)
            {
                ownedShip.currentSpeed = ownedShip.scmSpeed * 60;
            }
            else if (DistanceLeft < 100000 && distanceLeft < ownedShip.currentSpeed)
            {
                ownedShip.currentSpeed -= ownedShip.quantum1Speed;
            }
            else if (DistanceLeft > 100000 && distanceLeft < ownedShip.currentSpeed)
            {
                ownedShip.currentSpeed -= ownedShip.quantum2Speed;
            }
            if (ownedShip.currentSpeed < 0)
            {
                ownedShip.currentSpeed = 0;
            }
        }

        private void Accelerate()
        {
            if (Destination.IsOnSurface && totalDistance < 100)
            {
                ownedShip.currentSpeed = 20;
            }
            else if (totalDistance < 100000 && ownedShip.currentSpeed < ownedShip.quantumMaxSpeed)
            {
                ownedShip.currentSpeed += ownedShip.quantum1Speed * 60;
            }
            else if (totalDistance > 100000 && ownedShip.currentSpeed < ownedShip.quantumMaxSpeed)
            {
                ownedShip.currentSpeed += ownedShip.quantum2Speed;
            }
            if (ownedShip.currentSpeed > ownedShip.quantumMaxSpeed)
            {
                ownedShip.currentSpeed = ownedShip.quantumMaxSpeed * 60;
            }
        }

        public double GetDistance()
        {
            return Math.Sqrt(Math.Pow(Destination.X - X, 2) + Math.Pow(Destination.Y - Y, 2));
        }

        private bool RequestLanding()
        {
            if (Destination.RequestLanding(ownedShip) || CurrentLocation.Hangars.Contains(ownedShip))
            {
                CurrentLocation = Destination;
                X = CurrentLocation.X;
                Y = CurrentLocation.Y;
                OnPropertyChanged("CurrentPosition");
                return true;
            }

            return false;
        }

        //private TradeState SellCommodity()
        //{
            //var transaction = new TradeState();
            //bool canBuy = false;
            //if (ownedShip.cargoManifest.Count > 0)
            //{
            //    if (CurrentLocation.Productions.Count > 0)
            //    {
            //        foreach (var produced in currentLocation.Productions)
            //        {
            //            if (produced.Requirements.Count > 0)
            //            {
            //                foreach (var bought in produced.Requirements)
            //                {
            //                    for (int i = 0; i < ownedShip.cargoManifest.Count; i++)
            //                    {
            //                        var manifest = ownedShip.cargoManifest[i];

            //                        if (bought.commodity.ID == manifest.commodity.ID)
            //                        {
            //                            canBuy = true;
            //                            //how much can the buyer buy?
            //                            int totalBuyable = bought.MaxStorage - bought.CurrentStorage;

            //                            //can the buyer buy everythign that the ship is trading?
            //                            if (totalBuyable < manifest.units && manifest.units > 0 && totalBuyable > 0)
            //                            {
            //                                totalBuyable = manifest.units - totalBuyable;
            //                            }
            //                            else if (totalBuyable < 0)
            //                            {
            //                                totalBuyable = 0;
            //                            }

            //                            if (totalBuyable >= manifest.units)
            //                            {
            //                                if (manifest.units > 10000)
            //                                {
            //                                    totalBuyable = 10000;
            //                                }
            //                                else
            //                                {
            //                                    totalBuyable = manifest.units;
            //                                }
            //                            }
            //                            if (totalBuyable > 0)
            //                            {
            //                                uec += totalBuyable * bought.Price;
            //                                bought.CurrentStorage += totalBuyable;
            //                                manifest.units -= totalBuyable;

            //                                bought.transactions += 0.01;
            //                                ownedShip.UpdateManifest(ownedShip.cargoManifest[i], manifest);
            //                                transaction = TradeState.Successfull;
            //                            }
            //                            else
            //                            {
            //                                transaction = TradeState.Failed;
            //                            }

            //                            if (ownedShip.cargoManifest.Count > 0 && ownedShip.cargoManifest.Contains(manifest) && manifest.units <= 0)
            //                            {
            //                                ownedShip.cargoManifest.Remove(manifest);
            //                            }
            //                        }
            //                        else
            //                        {
            //                            canBuy = false;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //if (ownedShip.cargoManifest.Count == 0 || canBuy == false)
            //{
            //    transaction = TradeState.Finished;
            //}
            //OnPropertyChanged("GetShipInfo");
            //OnPropertyChanged("GetUEC");
            //return transaction;
        //}

        //private bool BuyCommodity(TradeInfo route)
        //{
        //    Production produced = route.startLocation.Productions.First(p => p.commodity.ID == route.commodity.ID);

        //    if (produced != null)
        //    {
        //        int totalAvailable = produced.CurrentStorage;
        //        int freeStorage = ownedShip.GetFreeCargo(CargoType.SCU);
        //        int buyableUnits = 0;
        //        double totalPrice = freeStorage * produced.Price;

        //        int maxRiskedUnits = 0;
        //        double max = uec * risk / produced.Price;
        //        if (max > totalAvailable)
        //        {
        //            maxRiskedUnits = totalAvailable;
        //        }
        //        else
        //        {
        //            maxRiskedUnits = Convert.ToInt32(max);
        //        }

        //        if (totalPrice > uec)
        //        {
        //            totalPrice = 0;

        //            while (totalPrice + produced.price < uec && buyableUnits + 1 < freeStorage && buyableUnits + 1 <= totalAvailable)
        //            {
        //                buyableUnits++;
        //                totalPrice = produced.price * buyableUnits;
        //            }
        //        }
        //        else
        //        {
        //            if (totalAvailable > freeStorage)
        //            {
        //                buyableUnits = freeStorage;
        //            }
        //            else if (totalAvailable < freeStorage)
        //            {
        //                buyableUnits = totalAvailable;
        //            }
        //            else if (buyableUnits > maxRiskedUnits)
        //            {
        //                buyableUnits = maxRiskedUnits;
        //            }
        //        }
        //        if (buyableUnits > 0)
        //        {
        //            if (buyableUnits <= freeStorage && freeStorage > 0)
        //            {
        //                uec -= totalPrice;
        //                produced.transactions += 0.01;
        //                produced.CurrentStorage -= buyableUnits;
        //                var newCargo = new CargoInfo()
        //                {
        //                    commodity = produced.commodity,
        //                    units = buyableUnits,
        //                    price = produced.price,
        //                    cargoType = CargoType.SCU,
        //                    hidden = false
        //                };
        //                ownedShip.cargoManifest.Add(newCargo);

        //                OnPropertyChanged("GetShipInfo");
        //                OnPropertyChanged("GetUEC");
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        public bool Bored()
        {
            waitingForTicks++;
            if (waitingForTicks * 0.05 > patience)
            {
                waitingForTicks = 0;
                OnPropertyChanged("BoredValue");
                return true;
                //TravelToOtherLocation();
            }
            OnPropertyChanged("BoredValue");
            return false;
        }
    }

    public enum State
    {
        Idle,
        PlanningTradeRoute,
        Buying,
        Selling,
        Landing,
        PrepareTakeoff,
        Traveling,
    }

    public enum TradeState
    {
        Successfull,
        Finished,
        Failed
    }

    public struct TradeInfo
    {
        public Location startLocation;
        public Location endLocation;
        public Commodity commodity;
        public double profit;

        public string start
        {
            get
            {
                return startLocation.name;
            }
        }

        public string end
        {
            get
            {
                return endLocation.name;
            }
        }

        public string Cargo
        {
            get
            {
                return commodity.Name;
            }
        }
    }

    public struct TravelInfo
    {
        public Location location;
        public double distance;
    }
}