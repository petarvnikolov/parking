using Parking.Models;

namespace Parking
{
    internal enum IOMessages
    {
        InvalidOperation,
        InvalidArguments,
        AvailableCommands,
        NoParkings,
        ParkingAlreadyExists,
        ParkingDoesNotExist,
        ParkingCreated,
        VehicleParked,
        NoFreeSpots
    }
    internal static class IO
    {
        public static void Start()
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Паркинг мениджър [Версия 1.0.0-alpha]");
            Console.WriteLine("(c) Петър Николов, КН, 241кнр\n");

            IO.NotifyUser(IOMessages.AvailableCommands);
        }

        public static void NotifyUser(IOMessages ioMessageType, Parking? selectedParking = null, List<Parking>? allParkings = null, string? parkingName = null)
        {
            switch (ioMessageType)
            {
                case IOMessages.InvalidOperation:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Невалидна операция.\n");
                    Console.ResetColor();
                    break;
                case IOMessages.AvailableCommands:
                    Console.WriteLine("Възможни операции:\n");
                    Console.WriteLine("     * Паркинг <име> <бр. места леки автомобили> <бр. места лекотоварни> <бр. места тежкотоварни>");
                    Console.WriteLine("     * (за нов автомобил) <тип автомобил> <марка> <модел>");
                    Console.WriteLine("     * Печат <име на паркинг>");
                    Console.WriteLine("     * Помощ");
                    Console.WriteLine("     * Край\n");
                    break;
                case IOMessages.InvalidArguments:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Невалидни аргументи.\n");
                    Console.ResetColor();
                    break;
                case IOMessages.NoParkings:
                    Console.WriteLine("В момента няма налични паркинги. Създайте нов с командата \"Паркинг\"\n");
                    break;
                case IOMessages.ParkingCreated when selectedParking != null:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("------------------------------------------------------");
                    Console.WriteLine($"Паркинг \"{selectedParking.Name}\" е създаден успешно!");
                    Console.WriteLine("------------------------------------------------------");
                    Console.WriteLine("| {0,-15} | {1,-15} | {2,-15} |", "Леки автомобили", "Лекотоварни", "Тежкотоварни");
                    Console.WriteLine("------------------------------------------------------");
                    Console.WriteLine("| {0,-15} | {1,-15} | {2,-15} |", selectedParking.TotalSpots.CarSpots, selectedParking.TotalSpots.BusSpots, selectedParking.TotalSpots.TruckSpots);
                    Console.WriteLine("------------------------------------------------------\n");
                    Console.ResetColor();
                    break;
                case IOMessages.ParkingCreated when selectedParking == null:
                    throw new ArgumentNullException(nameof(selectedParking), "ERROR: Created parking object is null.");
                case IOMessages.ParkingAlreadyExists:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Паркинг с това име вече съществува.\n");
                    Console.ResetColor();
                    break;
                case IOMessages.ParkingDoesNotExist:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Паркинг с това име не съществува.\n");
                    Console.ResetColor();
                    break;
                case IOMessages.VehicleParked when selectedParking != null:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Превозното средство е паркирано успешно в паркинг \"{selectedParking.Name}\"!\n");
                    Console.ResetColor();
                    break;
                case IOMessages.VehicleParked when selectedParking == null:
                    throw new ArgumentNullException(nameof(selectedParking), "ERROR: Selected parking object is null.");
                case IOMessages.NoFreeSpots:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Превозното средство не е паркирано. Няма свободни места в паркингите.\n");
                    Console.ResetColor();
                    break;
            }
        }

        public static Command GetInput(List<Parking>? parkings = null)
        {
            bool isInitial = parkings == null || parkings.Count < 1;
            if (isInitial)
            {
                IO.NotifyUser(IOMessages.NoParkings);
            }

            Console.Write("> ");

            Command command = new(CommandTargets.Invalid);

            string? rawInput = Console.ReadLine();
            string[] rawInputArray;

            Console.WriteLine("");

            if (String.IsNullOrWhiteSpace(rawInput))
            {
                IO.NotifyUser(IOMessages.InvalidOperation);
                IO.NotifyUser(IOMessages.AvailableCommands);
                return command;
            }

            rawInputArray = rawInput.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (isInitial && rawInputArray[0].ToLower() != "паркинг")
            {
                return command;
            }

            switch (rawInputArray[0].ToLower())
            {
                case "паркинг":
                    command = ValidateParkingCommand(rawInputArray);
                    break;
                case "кола":
                    command = ValidateVehicleInput(rawInputArray, VehicleType.Car);
                    break;
                case "бус":
                    command = ValidateVehicleInput(rawInputArray, VehicleType.Bus);
                    break;
                case "камион":
                    command = ValidateVehicleInput(rawInputArray, VehicleType.Truck);
                    break;
                case "печат":
                    command = ValidatePrintCommand(rawInputArray, parkings!);
                    break;
                case "край":
                    command.Target = CommandTargets.End;
                    break;
                case "помощ":
                    IO.NotifyUser(IOMessages.AvailableCommands);
                    break;
                default:
                    IO.NotifyUser(IOMessages.InvalidOperation);
                    IO.NotifyUser(IOMessages.AvailableCommands);
                    break;
            }

            return command;
        }

        public static Command ValidateParkingCommand(string[] rawInputArray)
        {
            Command command;

            if (rawInputArray.Length < 5 || rawInputArray.Length > 5)
            {
                IO.NotifyUser(IOMessages.InvalidArguments);
                IO.NotifyUser(IOMessages.AvailableCommands);
                return new Command(CommandTargets.Invalid);
            }

            bool carSpotsIsValid = int.TryParse(rawInputArray[2], out int carSpots);
            bool busSpotsIsValid = int.TryParse(rawInputArray[3], out int busSpots);
            bool truckSpotsIsValid = int.TryParse(rawInputArray[4], out int truckSpots);

            bool inputIsValid = carSpotsIsValid && busSpotsIsValid && truckSpotsIsValid;

            if (inputIsValid)
            {
                command = new(CommandTargets.Parking, rawInputArray[1], new ParkingSpots() { CarSpots = carSpots, BusSpots = busSpots, TruckSpots = truckSpots });
            }
            else
            {
                IO.NotifyUser(IOMessages.InvalidArguments);
                return new Command(CommandTargets.Invalid);
            }

            return command;
        }

        public static Command ValidateVehicleInput(string[] rawInputArray, VehicleType vehicleType)
        {
            if (rawInputArray.Length < 3 || rawInputArray.Length > 3)
            {
                IO.NotifyUser(IOMessages.InvalidArguments);
                IO.NotifyUser(IOMessages.AvailableCommands);
                return new Command(CommandTargets.Invalid);
            }

            Command command = new(CommandTargets.Vehicle, vehicleType: vehicleType, vehicleBrand: rawInputArray[1], vehicleModel: rawInputArray[2]);

            return command;
        }

        public static Command ValidatePrintCommand(string[] rawInputArray, List<Parking> parkings)
        {
            if (rawInputArray.Length < 2 || rawInputArray.Length > 2)
            {
                IO.NotifyUser(IOMessages.InvalidArguments);
                IO.NotifyUser(IOMessages.AvailableCommands);
                return new Command(CommandTargets.Invalid);
            }
            else if (!Parking.Exists(rawInputArray[1], parkings))
            {
                IO.NotifyUser(IOMessages.ParkingDoesNotExist);
                return new Command(CommandTargets.Invalid);
            }

            return new Command(CommandTargets.Print, rawInputArray[1]);
        }

        public static void PrintParking(Parking parking)
        {
            List<VehicleBase> sortedVehicles = parking.ParkedVehicles.OrderBy(v => v.Type == VehicleType.Car ? 0 : v.Type == VehicleType.Bus ? 1 : 2).ToList();

            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine($"Паркинг \"{parking.Name}\" разполага със следните места:");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine($"{" ",-27}| {"Общо",-10} | {"Заети",-10} |");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("| Леки автомобили          | {0, -10} | {1, -10} |", parking.TotalSpots.CarSpots, parking.OccupiedSpots.CarSpots);
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("| Лекотоварни автомобили   | {0, -10} | {1, -10} |", parking.TotalSpots.BusSpots, parking.OccupiedSpots.BusSpots);
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("| Тежкотоварни автомобили  | {0, -10} | {1, -10} |", parking.TotalSpots.TruckSpots, parking.OccupiedSpots.TruckSpots);
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("Aвтомобили в паркинга:");
            Console.WriteLine("------------------------------------------------------");
            foreach (VehicleBase vehicle in sortedVehicles)
            {
                Console.WriteLine($"| Марка: {vehicle.Brand,-17} | Модел: {vehicle.Model,-16} |");
                Console.WriteLine("------------------------------------------------------");
            }
            Console.WriteLine("");
        }

        public static void PrintEndMessage(List<Parking> parkings)
        {
            if (parkings == null || parkings.Count < 1)
            {
                Console.WriteLine("Няма създадени паркинги. Изключване...");
                return;
            }

            Console.WriteLine("------------------------------------------------------");
            foreach (Parking parking in parkings)
            {
                Console.WriteLine($"Паркинг \"{parking.Name}\" разполага със следните места:");
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine($"{" ",-27}| {"Общо",-10} | {"Заети",-10} |");
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("| Леки автомобили          | {0, -10} | {1, -10} |", parking.TotalSpots.CarSpots, parking.OccupiedSpots.CarSpots);
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("| Лекотоварни автомобили   | {0, -10} | {1, -10} |", parking.TotalSpots.BusSpots, parking.OccupiedSpots.BusSpots);
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("| Тежкотоварни автомобили  | {0, -10} | {1, -10} |", parking.TotalSpots.TruckSpots, parking.OccupiedSpots.TruckSpots);
                Console.WriteLine("------------------------------------------------------");
            }
        }
    }

    internal enum CommandTargets
    {
        Parking,
        Vehicle,
        Print,
        End,
        Invalid
    }

    internal class Command
    {
        public CommandTargets Target { get; set; }
        public string? ParkingName { get; set; } = null;
        public ParkingSpots? ParkingSpots { get; set; } = null;
        public VehicleType? VehicleType { get; set; } = null;
        public string? VehicleBrand { get; set; } = null;
        public string? VehicleModel { get; set; } = null;

        public Command(CommandTargets target, string? parkingName = null, ParkingSpots? parkingSpots = null, VehicleType? vehicleType = null, string? vehicleBrand = null, string? vehicleModel = null)
        {
            this.Target = target;

            switch (this.Target)
            {
                case CommandTargets.Parking:
                    if (parkingName == null || parkingSpots == null)
                    {
                        throw new ArgumentNullException("parkingName, parkingSpots", "Parking name or parking spots are null.");
                    }
                    this.ParkingName = parkingName;
                    this.ParkingSpots = parkingSpots;
                    break;
                case CommandTargets.Vehicle:
                    if (vehicleType == null || vehicleBrand == null || vehicleModel == null)
                    {
                        throw new ArgumentNullException("vehicleType, vehicleBrand, vehicleModel", "Vehicle type, brand or model are null.");
                    }
                    this.VehicleType = vehicleType;
                    this.VehicleBrand = vehicleBrand;
                    this.VehicleModel = vehicleModel;
                    break;
                case CommandTargets.Print:
                    if (parkingName == null)
                    {
                        throw new ArgumentNullException(nameof(parkingName), "Parking name is null.");
                    }
                    this.ParkingName = parkingName;
                    break;
                case CommandTargets.End:
                    break;
                case CommandTargets.Invalid:
                    break;
                default:
                    throw new ArgumentException("Invalid action target.");
            }
        }

    }
}
