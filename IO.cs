namespace Parking
{
    internal static class IO
    {
        public static void Start()
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Паркинг мениджър [Версия 1.0.0-alpha]");
            Console.WriteLine("(i) Петър Николов, КН, 241кнр\n");

            Console.WriteLine("Възможни операции:\n");
            Console.WriteLine("     * Паркинг <име> <бр. места леки автомобили> <бр. места лекотоварни> <бр. места тежкотоварни>");
            Console.WriteLine("     * (за нов автомобил) <тип автомобил> <марка> <модел>");
            Console.WriteLine("     * Печат <име на паркинг>");
            Console.WriteLine("     * Край\n\n");
        }

        public static Command GetInitialInput()
        {
            Console.WriteLine("В момента няма налични паркинги. Създайте нов с командата \"Паркинг\"\n");

            Command initialCommand = GetInput();

            if (initialCommand.Target == ActionTarget.End)
            {
                Console.WriteLine("Няма създадени паркинги. Изключване...");
                return initialCommand;
            }

            if (initialCommand.Target != ActionTarget.Parking)
            {
                initialCommand.Target = ActionTarget.Invalid;
            }

            return initialCommand;
        }

        public static Command GetInput()
        {
            Console.Write("> ");

            Command command = new(ActionTarget.Invalid);

            string rawInput = Console.ReadLine();
            Console.WriteLine("");

            string[] rawInputArr;

            if (String.IsNullOrWhiteSpace(rawInput))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Невалидна операция!\n");
                Console.ResetColor();
                return command;
            }
            else
            {
                rawInputArr = rawInput.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            }

            switch (rawInputArr[0])
            {
                case "Паркинг":
                    command = ValidateParkingInput(rawInputArr);
                    break;
                case "Кола":
                    command = ValidateParkableInput(rawInputArr, ParkableType.Car);
                    break;
                case "Бус":
                    command = ValidateParkableInput(rawInputArr, ParkableType.Bus);
                    break;
                case "Камион":
                    command = ValidateParkableInput(rawInputArr, ParkableType.Truck);
                    break;
                case "Печат":
                    command.Target = ActionTarget.Print;
                    command.ParkingName = rawInputArr[1];
                    if (rawInputArr.Length < 2 || rawInputArr.Length > 2)
                    {
                        command.Target = ActionTarget.Invalid;
                    }
                    break;
                case "Край":
                    command.Target = ActionTarget.End;
                    break;
                default:
                    command.Target = ActionTarget.Invalid;
                    break;
            }

            if (command.Target == ActionTarget.Invalid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Невалидна операция!\n");
                Console.ResetColor();
            }

            return command;
        }

        public static Command ValidateParkingInput(string[] rawInputArr)
        {
            Command command = new(ActionTarget.Parking);

            if (rawInputArr.Length < 5 || rawInputArr.Length > 5)
            {
                return new Command(ActionTarget.Invalid);
            }

            bool carSpotsIsValid = int.TryParse(rawInputArr[2], out int carSpots);
            bool busSpotsIsValid = int.TryParse(rawInputArr[3], out int busSpots);
            bool truckSpotsIsValid = int.TryParse(rawInputArr[4], out int truckSpots);

            bool inputIsValid = carSpotsIsValid && busSpotsIsValid && truckSpotsIsValid;

            if (inputIsValid)
            {
                command.ParkingName = rawInputArr[1];
                command.ParkingSpots = new()
                {
                    CarSpots = carSpots,
                    BusSpots = busSpots,
                    TruckSpots = truckSpots
                };
            }
            else
            {
                return new Command(ActionTarget.Invalid);
            }

            return command;
        }

        public static Command ValidateParkableInput(string[] rawInputArr, ParkableType parkableType)
        {
            if (rawInputArr.Length < 3 || rawInputArr.Length > 3)
            {
                return new Command(ActionTarget.Invalid);
            }

            Command command = new(ActionTarget.Parkable)
            {
                ParkableType = parkableType,
                ParkableBrand = rawInputArr[1],
                ParkableModel = rawInputArr[2]
            };

            return command;
        }

        public static void NotifyParkingCreated(Parking parking)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Паркинг \"{parking.Name}\" е създаден успешно!");
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("| {0,-15} | {1,-15} | {2,-15} |", "Леки автомобили", "Лекотоварни", "Тежкотоварни");
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("| {0,-15} | {1,-15} | {2,-15} |\n", parking.Spots.CarSpots, parking.Spots.BusSpots, parking.Spots.TruckSpots);
            Console.ResetColor();
        }

        public static void PrintEndMessage(List<Parking> parkings)
        {
            foreach (Parking parking in parkings)
            {
                Console.WriteLine($"Паркинг \"{parking.Name}\" разполага със следните места:");
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("| Леки автомобили          | {0, -10} | {1, -10} |", parking.Spots.CarSpots, parking.Spots.CarOccupied);
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("| Лекотоварни автомобили   | {0, -10} | {1, -10} |", parking.BusSpots, parking.BusOccupied);
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("| Тежкотоварни автомобили  | {0, -10} | {1, -10} |\n", parking.TruckSpots, parking.TruckOccupied);
            }
        }
    }

    internal enum ActionTarget
    {
        Parking,
        Parkable,
        Print,
        End,
        Invalid
    }

    internal class Command
    {
        public ActionTarget Target { get; set; }
        public string? ParkingName { get; set; } = null;
        public ParkingSpots? ParkingSpots { get; set; } = null;
        public ParkableType? ParkableType { get; set; } = null;
        public string? ParkableBrand { get; set; } = null;
        public string? ParkableModel { get; set; } = null;

        public Command(ActionTarget target, string? parkingName = null, ParkingSpots? parkingSpots = null, ParkableType? parkableType = null, string? parkableBrand = null, string? parkableModel = null)
        {
            this.Target = target;

            switch (this.Target)
            {
                case ActionTarget.Parking:
                    this.ParkingName = parkingName;
                    this.ParkingSpots = parkingSpots;
                    break;
                case ActionTarget.Parkable:
                    this.ParkableType = parkableType;
                    this.ParkableBrand = parkableBrand;
                    this.ParkableModel = parkableModel;
                    break;
                case ActionTarget.Print:
                    this.ParkingName = parkingName;
                    break;
                case ActionTarget.End:
                    break;
                case ActionTarget.Invalid:
                    break;
                default:
                    throw new ArgumentException("Invalid action target.");
            }
        }

    }
}
