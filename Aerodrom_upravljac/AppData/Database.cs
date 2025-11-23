using System.Collections.Generic;
using AirportManagement.Models;

namespace AirportManagement.AppData
{
    public class Database
    {
        public List<Passenger> Passengers { get; set; } = new();
        public List<Flight> Flights { get; set; } = new();
        public List<Plane> Planes { get; set; } = new();
        public List<CrewMember> CrewMembers { get; set; } = new();
    }
}
