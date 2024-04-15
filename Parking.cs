namespace Parking
{
    internal class ParkingSpots
    {
        public int CarSpots { get; set; }
        public int BusSpots { get; set; }
        public int TruckSpots { get; set; }
    }
    internal class Parking
    {
        public string Name { get; set; }
        public ParkingSpots Spots { get; set; }
        public List<IParkable> ParkedVehicles { get; set; }

        public Parking(string name, ParkingSpots spots)
        {
            Name = name;
            Spots = spots;
            ParkedVehicles = new();
        }
    }
}
