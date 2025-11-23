using System;
using System.Collections.Generic;

namespace AirportManagement.Models
{
    public class Passenger
    {
        public Guid Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int YearOfBirth { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Flight> BookedFlights { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastModified { get; private set; }

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
            BookedFlights = new List<Flight>();
        }

        public void UpdateModification()
        {
            LastModified = DateTime.Now;
        }
    }
}
