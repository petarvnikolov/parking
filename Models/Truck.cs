using Parking.Models;

namespace Parking
{
    internal class Truck : VehicleBase
    {
        public Truck(string brand, string model) : base(brand, model)
        {
            Type = VehicleType.Truck;
        }
    }
}
