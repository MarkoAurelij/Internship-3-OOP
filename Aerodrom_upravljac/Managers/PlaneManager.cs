using System;
using System.Collections.Generic;
using System.Linq;
using AirportManagement.Models;
using AirportManagement.AppData;

namespace AirportManagement.Managers
{
    public class PlaneManager
    {
        private readonly Database _db;

        public PlaneManager(Database db)
        {
            _db = db;
        }

        public void AddPlane(Plane plane)
        {
            _db.Planes.Add(plane);
        }

        public List<Plane> GetAllPlanes()
        {
            return _db.Planes;
        }

        public Plane GetPlaneById(Guid id)
        {
            return _db.Planes.FirstOrDefault(p => p.Id == id);
        }
    }
}
