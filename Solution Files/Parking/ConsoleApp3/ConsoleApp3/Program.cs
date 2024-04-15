using ConsoleApp3.Models;

namespace ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Parking> parkings = new List<Parking>();
            bool programRunning = true;

            do
            {
                string input = Console.ReadLine();
                string[] inputArray = input
                                        .Split(new[] { ' '}, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(x => x.Trim())
                                        .ToArray();

                if(inputArray.Length <= 0)
                {
                    continue;
                }

                switch (inputArray[0].ToLower())
                {
                    case "паркинг":
                        if (inputArray.Length != 5)
                        {
                            continue;
                        }
                        string parkingName = inputArray[1];
                        int cars = int.Parse(inputArray[2]);
                        int buses = int.Parse(inputArray[3]);
                        int trucks = int.Parse(inputArray[4]);

                        Parking p = new Parking(parkingName, cars, buses, trucks);

                        parkings.Add(p);

                        break;
                    case "кола":
                        if (inputArray.Length != 3)
                        {
                            continue;
                        }
                        string make = inputArray[1];
                        string model = inputArray[2];

                        Car c = new Car(make, model);

                        if (!c.Park(parkings))
                        {
                            Console.WriteLine($"Няма свободни паркоместа за {c.Make} {c.Model}!");
                        }
                        break;
                    case "бус":
                        if (inputArray.Length != 3)
                        {
                            continue;
                        }
                        string bMake = inputArray[1];
                        string bModel = inputArray[2];

                        Bus b = new Bus(bMake, bModel);

                        if (!b.Park(parkings))
                        {
                            Console.WriteLine($"Няма свободни паркоместа за {b.Make} {b.Model}!");
                        }
                        break;
                    case "камион":
                        if (inputArray.Length != 3)
                        {
                            continue;
                        }
                        string tMake = inputArray[1];
                        string tModel = inputArray[2];

                        Truck t = new Truck(tMake, tModel);

                        if (!t.Park(parkings))
                        {
                            Console.WriteLine($"Няма свободни паркоместа за {t.Make} {t.Model}!");
                        }
                        break;
                    case "печат":
                        if (inputArray.Length != 2)
                        {
                            continue;
                        }

                        Parking pPrint = parkings.FirstOrDefault(x => x.ParkingName == inputArray[1]);
                        var vehicles = pPrint.GetVehicles();
                        Console.WriteLine($"Паркирани автомобили в паркинг {pPrint.ParkingName}:");
                        foreach ( var vehicle in vehicles ) 
                        {
                            Console.WriteLine(vehicle);
                        }
                        break;
                    case "край":
                        programRunning = false;
                        foreach(var parking in parkings)
                        {
                            Console.WriteLine(parking);
                        }
                        break;
                }
            }
            while (programRunning);

            Console.WriteLine();
        }
    }
}