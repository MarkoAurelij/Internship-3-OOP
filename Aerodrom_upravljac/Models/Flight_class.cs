using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AirportManagement.Models
{
    public class Flight
    {

        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public double Distance { get; set; } 
        public double Duration => (Arrival - Departure).TotalHours;
        public Plane Plane { get; set; }
        public Crew Crew { get; set; }
        public List<Passenger> Passengers { get; private set; } = new List<Passenger>();
        public DateTime CreatedAt { get; private set; }
        public DateTime LastModified { get; private set; }


        public Flight(string name, string origin, string destination, DateTime departure, DateTime arrival, double distance, Plane plane, Crew crew)
        {
            Name = name;
            Origin = origin;
            Destination = destination;
            Departure = departure;
            Arrival = arrival;
            Distance = distance;
            Plane = plane;
            Crew = crew;
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            LastModified = DateTime.Now;
        }


        public void UpdateModification()
        {
            LastModified = DateTime.Now;
        }

        public bool CanBeCancelled()
        {
            return Departure > DateTime.Now.AddHours(24);
        }


        public bool HasCapacity()
        {
            int totalSeats = Plane.Seats.Values.Sum(); 
            return Passengers.Count < totalSeats;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"[{Id}] {Name}: {Origin} -> {Destination}, Departure: {Departure}, Arrival: {Arrival}, Distance: {Distance} km, Passengers: {Passengers.Count}/{Plane.Seats.Values.Sum()}");
        }
    }
}
