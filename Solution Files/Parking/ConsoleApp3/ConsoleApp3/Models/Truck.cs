using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.Models
{
    internal class Truck : IParkable
    {
        public string Make { get; private set; }
        public string Model { get; private set; }

        public Truck(string make, string model)
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
