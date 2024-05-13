using Parking.Models;

namespace Parking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool programRunning = true;

            List<Parking> parkings = new();

            IO.Start();

            while (programRunning)
            {
                Command command = IO.GetInput(parkings);

                if (command.Target == CommandTargets.Parking)
                {
                    Parking? createdParking = Parking.Create(command.ParkingName!, command.ParkingSpots!, parkings);

                    if (createdParking == null)
                    {
                        continue;
                    }

                    parkings.Add(createdParking);
                }

                if (command.Target == CommandTargets.Vehicle)
                {
                    VehicleBase vehicle = VehicleBase.Create(command.VehicleType!, command.VehicleBrand!, command.VehicleModel!);

                    bool vehicleParked = vehicle.Park(parkings);

                    if (!vehicleParked)
                    {
                        continue;
                    }
                }

                if (command.Target == CommandTargets.Print)
                {
                    Parking? parking = Parking.Get(command.ParkingName!, parkings);

                    if (parking == null)
                    {
                        continue;
                    }

                    IO.PrintParking(parking);
                }

                if (command.Target == CommandTargets.End)
                {
                    programRunning = false;

                    IO.PrintEndMessage(parkings);
                }
            }
        }
    }
}