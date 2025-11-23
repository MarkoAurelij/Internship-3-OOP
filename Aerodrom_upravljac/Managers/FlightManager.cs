using System;
using System.Collections.Generic;
using System.Linq;
using AirportManagement.Models;
using AirportManagement.AppData;

namespace AirportManagement.Managers
{
    public class FlightManager
    {
        private readonly Database _db;

        public FlightManager() : this(new Database()) { }

        public FlightManager(Database db)
        {
            _db = db;
        }

        public void AddFlight(Flight flight)
        {
            _db.Flights.Add(flight);
        }

        public List<Flight> GetAllFlights()
        {
            return _db.Flights;
        }

        public Flight GetFlightById(Guid id)
        {
            return _db.Flights.FirstOrDefault(f => f.Id == id);
        }
    }
}
