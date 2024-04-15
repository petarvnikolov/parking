namespace Parking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool programRunning = true;

            List<Parking> parkings = new();

            IO.Start();

            Command initialCommand;

            do
            {
                initialCommand = IO.GetInitialInput();

                if (initialCommand.Target == ActionTarget.End)
                {
                    break;
                }
                else if (initialCommand.Target == ActionTarget.Parking)
                {
                    Parking parking = new(initialCommand.ParkingName, initialCommand.ParkingSpots);
                    parkings.Add(parking);
                    IO.NotifyParkingCreated(parking);

                    break;
                }
            } while (initialCommand.Target != ActionTarget.Parking);

            while (programRunning)
            {
                Command input = IO.GetInput();

                if (input.Target == ActionTarget.End)
                {
                    programRunning = false;
                    IO.PrintEndMessage(parkings);
                }
            }
        }
    }
}