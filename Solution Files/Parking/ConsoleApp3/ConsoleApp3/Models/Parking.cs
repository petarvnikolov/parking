using System.Reflection;
using System.Runtime.CompilerServices;

namespace ConsoleApp3.Models
{
    internal class Parking
    {
        private List<IParkable> vehicles;
        public string ParkingName { get; private set; }
        public int Car { get; private set; }
        public int Bus { get; private set; }
        public int Truck { get; private set; }

        public Parking(string parkingName, int cars, int buses, int trucks)
        {
            this.vehicles = new List<IParkable>();
            this.ParkingName = parkingName;
            this.Car = cars;
            this.Bus = buses;
            this.Truck = trucks;
        }

        public bool AddVehicle(IParkable vehicle)
        {
            
            int parked = this.vehicles.Where(x => x.GetType().Name == vehicle.GetType().Name).ToList().Count;
            int available = (int)this.GetType().GetProperty(vehicle.GetType().Name).GetValue(this, null);

            if(parked < available)
            {
                this.vehicles.Add(vehicle);
                return true;
            }
            return false;
        }

        public List<IParkable> GetVehicles()
        {
            return new List<IParkable>(this.vehicles);
        }

        public override string ToString()
        {
            int carsParked = this.vehicles.Count(x => x.GetType().Name == "Car");
            //int carsParked = this.vehicles.Where(x => x.GetType().Name == "Car").Count;
            int busesParked = this.vehicles.Count(x => x.GetType().Name == "Bus");
            int trucksParked = this.vehicles.Count(x => x.GetType().Name == "Truck");
            string result = $"Паркинг {this.ParkingName} разполага със следните места:\r\n";
            result += $"Леки автомобили {this.Car}, заети {carsParked}\r\n";
            result += $"Лекотоварни автомобили {this.Bus}, заети {busesParked}\r\n";
            result += $"Тежкотоварни автомобили {this.Truck}, заети {trucksParked}";
            return result;
        }
    }
}