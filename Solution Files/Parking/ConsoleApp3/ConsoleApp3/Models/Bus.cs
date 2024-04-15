namespace ConsoleApp3.Models
{
    internal class Bus : IParkable
    {
        public string Make { get; private set; }
        public string Model { get; private set; }

        public Bus(string make, string model)
        {
            this.Make = make;
            this.Model = model;
        }

        public bool Park(List<Parking> parkings)
        {
            foreach (var parking in parkings)
            {
                if (parking.AddVehicle(this))
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"Марка {this.Make}, модел {this.Model}";
        }
    }
}