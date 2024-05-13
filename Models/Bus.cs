using Parking.Models;

namespace Parking
{
    internal class Bus : VehicleBase
    {
        public Bus(string brand, string model) : base(brand, model)
        {
            Type = VehicleType.Bus;
        }
    }
}
