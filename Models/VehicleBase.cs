namespace Parking.Models
{
    internal enum VehicleType
    {
        Car,
        Bus,
        Truck
    }
    internal abstract class VehicleBase
    {
        public VehicleType Type { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;

        public VehicleBase(string brand, string model)
        {
            Brand = brand;
            Model = model;
        }

        public static VehicleBase Create(VehicleType? type, string brand, string model)
        {
            return type switch
            {
                VehicleType.Car => new Car(brand, model),
                VehicleType.Bus => new Bus(brand, model),
                VehicleType.Truck => new Truck(brand, model),
                _ => throw new ArgumentException("Invalid vehicle type", nameof(type)),
            };
        }

        public bool Park(List<Parking> parkings)
        {
            Parking? parking = parkings.FirstOrDefault(p => p.HasFreeSpot(this));

            if (parking == null)
            {
                IO.NotifyUser(IOMessages.NoFreeSpots);
                return false;
            }

            parking.ParkedVehicles.Add(this);

            IO.NotifyUser(IOMessages.VehicleParked, parking);

            return true;
        }
    }
}
