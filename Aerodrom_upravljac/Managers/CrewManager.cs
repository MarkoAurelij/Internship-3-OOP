using System;
using System.Collections.Generic;
using System.Linq;
using AirportManagement.Models;
using AirportManagement.AppData;

namespace AirportManagement.Managers
{
    public class CrewManager
    {
        private readonly Database _db;

        public CrewManager(Database db)
        {
            _db = db;
        }

        public void AddCrewMember(CrewMember member)
        {
            _db.CrewMembers.Add(member);
        }

        public List<CrewMember> GetAllCrewMembers()
        {
            return _db.CrewMembers;
        }

        public CrewMember GetCrewMemberById(Guid id)
        {
            return _db.CrewMembers.FirstOrDefault(c => c.Id == id);
        }
    }
}
