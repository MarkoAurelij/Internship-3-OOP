using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportManagement.Models
{
    public enum Position
    {
        Pilot,
        CoPilot,
        Steward,
        Stewardess
    }
    public class Crew
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public CrewMember Pilot { get; set; }
        public CrewMember CoPilot { get; set; }
        public CrewMember Steward1 { get; set; }
        public CrewMember Steward2 { get; set; }

        public string Name => $"{Pilot.LastName}/{CoPilot.LastName} Crew";
    }


    public class CrewMember
    {
        public Guid Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int YearOfBirth { get; set; }
        public string Gender { get; set; }
        public Position CrewPosition { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastModified { get; private set; }

        public CrewMember(string firstName, string lastName, int yearOfBirth, string gender, Position crewPosition)
        {
            FirstName = firstName;
            LastName = lastName;
            YearOfBirth = yearOfBirth;
            Gender = gender;
            CrewPosition = crewPosition;
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            LastModified = DateTime.Now;
        }

        public void UpdateModification()
        {
            LastModified = DateTime.Now;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"{FirstName} {LastName} ({CrewPosition}), Born: {YearOfBirth}");
        }
    }

    public class Passenger
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int YearOfBirth { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }

        public Passenger(string firstName, string lastName, int yearOfBirth, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            YearOfBirth = yearOfBirth;
            Email = email;
            Password = password;
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            LastModified = DateTime.Now;
        }

        public void UpdateModification()
        {
            LastModified = DateTime.Now;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"{FirstName} {LastName}, Born: {YearOfBirth}, Email: {Email}");
        }
    }


    public class Plane
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public int YearOfProduction { get; set; }
        public Dictionary<string, int> Seats { get; set; } = new Dictionary<string, int>();
        public DateTime CreatedAt { get; private set; }
        public DateTime LastModified { get; private set; }

        public Plane(string name, int yearOfProduction, Dictionary<string, int> seats)
        {
            Name = name;
            YearOfProduction = yearOfProduction;
            Seats = seats;
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            LastModified = DateTime.Now;
        }

        public void UpdateModification()
        {
            LastModified = DateTime.Now;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"{Name} ({YearOfProduction})");
            foreach (var seat in Seats)
            {
                Console.WriteLine($"  {seat.Key}: {seat.Value} seats");
            }
        }
    }


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

        public Flight(string name, string origin, string destination,
              DateTime departure, DateTime arrival,
              double distance, Plane plane, Crew crew)
        {
            Name = name;
            Origin = origin;
            Destination = destination;
            Departure = departure;
            Arrival = arrival;
            Distance = distance;
            Plane = plane;
            Crew = crew;
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
            string crewInfo = Crew != null ? Crew.Name : "No crew assigned";
            int totalSeats = Plane?.Seats.Values.Sum() ?? 0;
            Console.WriteLine($"[{Id}] {Name}: {Origin} -> {Destination}, Departure: {Departure}, Arrival: {Arrival}, Distance: {Distance} km, Passengers: {Passengers.Count}/{totalSeats}, Crew: {crewInfo}");
        }

    }
}
