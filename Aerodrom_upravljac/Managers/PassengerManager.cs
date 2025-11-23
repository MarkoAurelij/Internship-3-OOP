using AirportManagement.AppData;
using AirportManagement.Models;

public class PassengerManager
{
    private readonly Database _db;

    public PassengerManager(Database db)
    {
        _db = db;
    }

    public PassengerManager() : this(new Database()) { }

    public void AddPassenger(Passenger passenger)
    {
        _db.Passengers.Add(passenger);
    }

    public List<Passenger> GetAllPassengers()
    {
        return _db.Passengers;
    }

    public Passenger Login(string email, string password)
    {
        return _db.Passengers.FirstOrDefault(p =>
            p.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
            p.Password == password);
    }
}
