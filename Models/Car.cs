using Parking.Models;

namespace Parking
{
    internal class Car : VehicleBase
    {
        public Car(string brand, string model) : base(brand, model)
        {
            Type = VehicleType.Car;
        }
    }
}
