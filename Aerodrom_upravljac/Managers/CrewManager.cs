using AirportManagement.AppData;
using AirportManagement.Models;

public class CrewManager
{
    private readonly Database _db;

    public CrewManager() : this(new Database()) { }

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


    public void AddCrew(Crew crew)
    {
        _db.Crews.Add(crew);
    }

    public List<Crew> GetAllCrews()
    {
        return _db.Crews;
    }

    public List<CrewMember> GetAvailableByPosition(Position position)
    {
        return _db.CrewMembers
            .Where(m => m.CrewPosition == position &&
                   !_db.Crews.Any(cr =>
                       cr.Pilot?.Id == m.Id ||
                       cr.CoPilot?.Id == m.Id ||
                       cr.Steward1?.Id == m.Id ||
                       cr.Steward2?.Id == m.Id))
            .ToList();
    }
}
