using System;
using System.Collections.Generic;
using System.Linq;
using AirportManagement.Models;
using AirportManagement.AppData;

namespace AirportManagement.Managers
{
    public class PassengerManager
    {
        private readonly Database _db;

        public PassengerManager(Database db)
        {
            _db = db;
        }

        public void AddPassenger(Passenger passenger)
        {
            _db.Passengers.Add(passenger);
        }

        public List<Passenger> GetAllPassengers()
        {
            return _db.Passengers;
        }

        public Passenger GetPassengerByEmail(string email)
        {
            return _db.Passengers
                .FirstOrDefault(p => p.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
