using System;

namespace AirportManagement.Models
{
    enum Position
    {
        Pilot,
        CoPilot,
        Steward,
        Stewardess
    }

    public class CrewMember
    {
        public Guid Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int YearOfBirth { get; set; }
        public string Gender { get; set; }
        public Position Position { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastModified { get; private set; }

        public CrewMember(string firstName, string lastName, int yearOfBirth, string gender, Position position)
        {
            FirstName = firstName;
            LastName = lastName;
            YearOfBirth = yearOfBirth;
            Gender = gender;
            Position = position;
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
            Console.WriteLine($"{FirstName} {LastName} ({Position}), Born: {YearOfBirth}");
        }
    }
}
