using Parking.Models;

namespace Parking
{
    internal class ParkingSpots
    {
        public int CarSpots { get; set; }
        public int BusSpots { get; set; }
        public int TruckSpots { get; set; }
    }
    internal class Parking
    {
        public string Name { get; set; }
        public ParkingSpots TotalSpots { get; set; }
        public ParkingSpots OccupiedSpots => GetOccupiedSpots();
        public List<VehicleBase> ParkedVehicles { get; set; }

        private Parking(string name, ParkingSpots total)
        {
            Name = name;
            TotalSpots = total;
            ParkedVehicles = new();
        }
        public static Parking? Create(string name, ParkingSpots total, List<Parking> parkings)
        {
            if (Parking.Exists(name, parkings))
            {
                IO.NotifyUser(IOMessages.ParkingAlreadyExists);
                return null;
            }

            Parking newParking = new(name, total);

            IO.NotifyUser(IOMessages.ParkingCreated, newParking);

            return newParking;
        }

        public static bool Exists(string name, List<Parking> parkings)
        {
            return parkings.Any(p => p.Name.ToLower() == name.ToLower());
        }

        public static Parking? Get(string name, List<Parking> parkings)
        {
            Parking? parking = parkings.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());

            if (parking == null)
            {
                IO.NotifyUser(IOMessages.ParkingDoesNotExist);
                return null;
            }

            return parking;
        }

        public bool HasFreeSpot(VehicleBase vehicle)
        {
            return vehicle.Type switch
            {
                VehicleType.Car => TotalSpots.CarSpots - OccupiedSpots.CarSpots > 0,
                VehicleType.Bus => TotalSpots.BusSpots - OccupiedSpots.BusSpots > 0,
                VehicleType.Truck => TotalSpots.TruckSpots - OccupiedSpots.TruckSpots > 0,
                _ => false
            };
        }

        public ParkingSpots GetOccupiedSpots()
        {
            return new ParkingSpots
            {
                CarSpots = ParkedVehicles.Count(v => v.Type == VehicleType.Car),
                BusSpots = ParkedVehicles.Count(v => v.Type == VehicleType.Bus),
                TruckSpots = ParkedVehicles.Count(v => v.Type == VehicleType.Truck)
            };
        }
    }
}
