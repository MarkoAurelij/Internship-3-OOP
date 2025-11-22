using System;
using System.Collections.Generic;

namespace AirportManagement.Models
{
    class Plane
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public int YearOfManufacture { get; set; }
        public Dictionary<string, int> Seats { get; set; } 
        public int FlightsCompleted { get; private set; } = 0;
        public DateTime CreatedAt { get; private set; }
        public DateTime LastModified { get; private set; }

        public Plane(string name, int yearOfManufacture, Dictionary<string, int> seats)
        {
            Name = name;
            YearOfManufacture = yearOfManufacture;
            Seats = seats;
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            LastModified = DateTime.Now;
        }

        public void UpdateModification()
        {
            LastModified = DateTime.Now;
        }

        public void IncrementFlights()
        {
            FlightsCompleted++;
            UpdateModification();
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"{Name} ({YearOfManufacture}) - Seats: {string.Join(", ", Seats)} - Flights completed: {FlightsCompleted}");
        }
    }
}
