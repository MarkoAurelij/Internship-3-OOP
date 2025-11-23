using AirportManagement.Managers;
using AirportManagement.Models;
using System;

namespace AirportManagement.Menus
{
    public static class MenuHelpers
    {
        public static void Wait()
        {
            Console.WriteLine("\nPritisnite ENTER za nastavak...");
            Console.ReadLine();
        }
    }

    public class MainMenu
    {
        private readonly PassengerManager _passengerManager;
        private readonly FlightManager _flightManager;
        private readonly PlaneManager _planeManager;
        private readonly CrewManager _crewManager;

        public MainMenu(PassengerManager pm, FlightManager fm, PlaneManager plm, CrewManager cm)
        {
            _passengerManager = pm;
            _flightManager = fm;
            _planeManager = plm;
            _crewManager = cm;
        }

        public void Show()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("1 - Passengers");
                Console.WriteLine("2 - Flights");
                Console.WriteLine("3 - Planes");
                Console.WriteLine("4 - Crew");
                Console.WriteLine("5 - Exit");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        new PassengerMenu(_passengerManager, _flightManager).Show();
                        break;
                    case "2":
                        new FlightMenu(_flightManager, _planeManager, _crewManager).Show();
                        break;
                    case "3":
                        new PlaneMenu(_planeManager).Show();
                        break;
                    case "4":
                        new CrewMenu(_crewManager).Show();
                        break;
                    case "5":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        MenuHelpers.Wait();
                        break;
                }
            }
        }
    }

    public class PassengerMenu
    {
        private readonly PassengerManager _manager;
        private readonly FlightManager _flightManager;
        private Passenger _currentPassenger;

        public PassengerMenu(PassengerManager manager, FlightManager flightManager)
        {
            _manager = manager;
            _flightManager = flightManager;
        }

        private void RegisterPassenger()
        {
            Console.Clear();
            Console.WriteLine("REGISTER PASSENGER");

            Console.Write("First Name (Enter to cancel): ");
            string firstName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstName)) return;

            Console.Write("Last Name (Enter to cancel): ");
            string lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName)) return;

            int yearOfBirth = 0;
            while (true)
            {
                Console.Write("Year of Birth (Enter to cancel): ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return;

                if (!int.TryParse(input, out yearOfBirth))
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }

                int age = DateTime.Now.Year - yearOfBirth;
                if (age < 18 || age > 100)
                {
                    Console.WriteLine("Passenger must be between 18 and 100 years old.");
                    continue;
                }
                break;
            }

            Console.Write("Email (Enter to cancel): ");
            string email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email)) return;

            Console.Write("Password (Enter to cancel): ");
            string password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password)) return;

            var passenger = new Passenger(firstName, lastName, yearOfBirth, email, password);
            _manager.AddPassenger(passenger);

            Console.WriteLine($"\nPassenger '{firstName} {lastName}' registered successfully!");
            MenuHelpers.Wait();
        }

        private void ListPassengers()
        {
            Console.Clear();
            Console.WriteLine("LIST OF PASSENGERS");

            var passengers = _manager.GetAllPassengers();

            if (passengers.Count == 0)
            {
                Console.WriteLine("No passengers found.");
            }
            else
            {
                foreach (var p in passengers)
                {
                    Console.WriteLine($"{p.FirstName} {p.LastName} (Born: {p.YearOfBirth}, Email: {p.Email})");
                }
            }

            MenuHelpers.Wait();
        }

        private void LoginPassenger()
        {
            Console.Clear();
            Console.WriteLine("PASSENGER LOGIN");

            Console.Write("Email: ");
            string email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email)) return;

            Console.Write("Password: ");
            string password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password)) return;

            var passenger = _manager.Login(email, password);
            if (passenger != null)
            {
                _currentPassenger = passenger;
                Console.WriteLine($"\nWelcome {passenger.FirstName} {passenger.LastName}!");
                MenuHelpers.Wait();
            }
            else
            {
                Console.WriteLine("\nInvalid email or password. Try again.");
                MenuHelpers.Wait();
            }
        }

        private void BookFlight()
        {
            if (_currentPassenger == null)
            {
                Console.WriteLine("You must be logged in to book a flight.");
                MenuHelpers.Wait();
                return;
            }

            var flights = _flightManager.GetAllFlights();
            if (flights.Count == 0)
            {
                Console.WriteLine("No flights available.");
                MenuHelpers.Wait();
                return;
            }

            Console.Clear();
            Console.WriteLine("AVAILABLE FLIGHTS");

            for (int i = 0; i < flights.Count; i++)
            {
                var f = flights[i];
                int totalSeats = f.Plane?.Seats.Values.Sum() ?? 0;
                Console.WriteLine($"{i + 1} - {f.Name}: {f.Origin} -> {f.Destination}, Seats: {f.Passengers.Count}/{totalSeats}");
            }

            Console.Write("\nChoose flight number to book (Enter to cancel): ");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return;

            if (!int.TryParse(input, out int flightIndex) || flightIndex < 1 || flightIndex > flights.Count)
            {
                Console.WriteLine("Invalid selection.");
                MenuHelpers.Wait();
                return;
            }

            var selectedFlight = flights[flightIndex - 1];
            if (!selectedFlight.HasCapacity())
            {
                Console.WriteLine("Selected flight is fully booked.");
                MenuHelpers.Wait();
                return;
            }

            selectedFlight.Passengers.Add(_currentPassenger);
            Console.WriteLine($"Successfully booked flight '{selectedFlight.Name}' for {_currentPassenger.FirstName} {_currentPassenger.LastName}!");
            MenuHelpers.Wait();
        }

        public void Show()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("PASSENGER MENU");
                Console.WriteLine("1 - Register Passenger");
                Console.WriteLine("2 - Login Passenger");
                Console.WriteLine("3 - List Passengers");
                Console.WriteLine("4 - Book Flight");
                Console.WriteLine("5 - Back");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": RegisterPassenger(); break;
                    case "2": LoginPassenger(); break;
                    case "3": ListPassengers(); break;
                    case "4": BookFlight(); break;
                    case "5": running = false; break;
                    default:
                        Console.WriteLine("Invalid option.");
                        MenuHelpers.Wait();
                        break;
                }
            }
        }
    }



    public class FlightMenu
    {
        private readonly FlightManager _manager;
        private readonly PlaneManager _planeManager;
        private readonly CrewManager _crewManager;

        public FlightMenu(FlightManager flightManager, PlaneManager planeManager, CrewManager crewManager)
        {
            _manager = flightManager;
            _planeManager = planeManager;
            _crewManager = crewManager;
        }

        private void AddFlight()
        {
            Console.Clear();
            Console.WriteLine("ADD FLIGHT");

            Console.Write("Flight Name (Enter to cancel): ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) return;

            Console.Write("Origin (Enter to cancel): ");
            string origin = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(origin)) return;

            Console.Write("Destination (Enter to cancel): ");
            string destination = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(destination)) return;

            DateTime departure;
            while (true)
            {
                Console.Write("Departure Date & Time (yyyy-MM-dd HH:mm) (Enter to cancel): ");
                string depInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(depInput)) return;
                if (!DateTime.TryParse(depInput, out departure))
                {
                    Console.WriteLine("Invalid format. Try again.");
                    continue;
                }
                break;
            }

            DateTime arrival;
            while (true)
            {
                Console.Write("Arrival Date & Time (yyyy-MM-dd HH:mm) (Enter to cancel): ");
                string arrInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(arrInput)) return;
                if (!DateTime.TryParse(arrInput, out arrival) || arrival <= departure)
                {
                    Console.WriteLine("Arrival must be after departure and in correct format.");
                    continue;
                }
                break;
            }

            double distance;
            while (true)
            {
                Console.Write("Distance in km (Enter to cancel): ");
                string distInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(distInput)) return;
                if (!double.TryParse(distInput, out distance) || distance <= 0)
                {
                    Console.WriteLine("Invalid distance. Must be positive number.");
                    continue;
                }
                break;
            }

            var planes = _planeManager.GetAllPlanes();
            if (planes.Count == 0)
            {
                Console.WriteLine("No planes available. Cannot create flight.");
                MenuHelpers.Wait();
                return;
            }

            int planeIndex = 0;
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("Select a Plane (Use Up/Down keys, Enter to confirm):");
                for (int i = 0; i < planes.Count; i++)
                {
                    var prefix = i == planeIndex ? "> " : "  ";
                    var p = planes[i];
                    Console.WriteLine($"{prefix}{i + 1} - {p.Name} ({p.YearOfProduction}) - Seats: {string.Join(", ", p.Seats.Keys)}");
                }

                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.DownArrow) planeIndex = (planeIndex + 1) % planes.Count;
                if (key == ConsoleKey.UpArrow) planeIndex = (planeIndex - 1 + planes.Count) % planes.Count;

            } while (key != ConsoleKey.Enter);

            Plane selectedPlane = planes[planeIndex];

            var allCrews = _crewManager.GetAllCrews();
            if (allCrews.Count == 0)
            {
                Console.WriteLine("No crews available. Cannot create flight.");
                MenuHelpers.Wait();
                return;
            }

            int crewIndex = 0;
            bool selectingCrew = true;
            do
            {
                Console.Clear();
                Console.WriteLine("Select Crew (Use Up/Down keys, Enter to confirm):");
                for (int i = 0; i < allCrews.Count; i++)
                {
                    var prefix = i == crewIndex ? "> " : "  ";
                    var crew = allCrews[i];
                    Console.WriteLine($"{prefix}{i + 1} - {crew.Name}");
                }

                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.DownArrow && crewIndex < allCrews.Count - 1) crewIndex++;
                if (key == ConsoleKey.UpArrow && crewIndex > 0) crewIndex--;
                if (key == ConsoleKey.Enter) selectingCrew = false;

            } while (selectingCrew);

            Crew selectedCrew = allCrews[crewIndex];

            var flight = new Flight(name, origin, destination, departure, arrival, distance, selectedPlane, selectedCrew);
            _manager.AddFlight(flight);

            Console.WriteLine($"\nFlight '{name}' added successfully!");
            MenuHelpers.Wait();
        }

        private void ListFlights()
        {
            Console.Clear();
            Console.WriteLine("LIST OF FLIGHTS");

            var flights = _manager.GetAllFlights();
            if (flights.Count == 0)
            {
                Console.WriteLine("No flights found.");
            }
            else
            {
                foreach (var f in flights)
                {
                    string planeName = f.Plane != null ? f.Plane.Name : "No Plane";
                    string crewName = f.Crew != null ? f.Crew.Name : "No Crew";
                    int totalSeats = f.Plane?.Seats.Values.Sum() ?? 0;
                    Console.WriteLine($"[{f.Id}] {f.Name}: {f.Origin} -> {f.Destination}, Departure: {f.Departure}, Arrival: {f.Arrival}, Distance: {f.Distance} km, Plane: {planeName}, Crew: {crewName}, Passengers: {f.Passengers.Count}/{totalSeats}");
                }
            }

            MenuHelpers.Wait();
        }

        public void Show()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("FLIGHT MENU");
                Console.WriteLine("1 - Add Flight");
                Console.WriteLine("2 - List Flights");
                Console.WriteLine("3 - Back");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddFlight();
                        break;
                    case "2":
                        ListFlights();
                        break;
                    case "3":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        MenuHelpers.Wait();
                        break;
                }
            }
        }
    }



    public class PlaneMenu
    {
        private readonly PlaneManager _manager;
        public PlaneMenu(PlaneManager manager) { _manager = manager; }

        private void AddPlane()
        {
            Console.Clear();
            Console.WriteLine("ADD PLANE");

            Console.Write("Plane Name (Enter to cancel): ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) return;

            int year;
            while (true)
            {
                Console.Write("Year of Production (Enter to cancel): ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return;

                if (!int.TryParse(input, out year) ||
                    year < 1900 ||
                    year > DateTime.Now.Year)
                {
                    Console.WriteLine("Invalid year. Try again.");
                    continue;
                }
                break;
            }

            var seats = new Dictionary<string, int>();
            string[] categories = { "Standard", "Business", "VIP" };

            foreach (var category in categories)
            {
                int count;
                while (true)
                {
                    Console.Write($"Number of {category} seats (Enter to cancel): ");
                    string seatInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(seatInput)) return;

                    if (!int.TryParse(seatInput, out count) || count < 0)
                    {
                        Console.WriteLine("Invalid seat number.");
                        continue;
                    }

                    break;
                }
                seats[category] = count;
            }

            var plane = new Plane(name, year, seats);
            _manager.AddPlane(plane);

            Console.WriteLine($"\nPlane '{name}' added successfully!");
            MenuHelpers.Wait();
        }

        private void ListPlanes()
        {
            Console.Clear();
            Console.WriteLine("LIST OF PLANES");

            var planes = _manager.GetAllPlanes();
            if (planes.Count == 0)
            {
                Console.WriteLine("No planes found.");
            }
            else
            {
                foreach (var p in planes)
                {
                    Console.WriteLine(
                        $"[{p.Id}] {p.Name} ({p.YearOfProduction}), " +
                        $"Seats: Standard-{p.Seats["Standard"]}, " +
                        $"Business-{p.Seats["Business"]}, VIP-{p.Seats["VIP"]}"
                    );
                }
            }

            MenuHelpers.Wait();
        }

        private void SearchPlane()
        {
            Console.Clear();
            Console.WriteLine("SEARCH PLANE");
            Console.WriteLine("1 - By ID");
            Console.WriteLine("2 - By Name");
            Console.Write("Choice: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter ID: ");
                string idInput = Console.ReadLine();

                if (Guid.TryParse(idInput, out Guid id))
                {
                    var plane = _manager.GetPlaneById(id);
                    if (plane == null)
                    {
                        Console.WriteLine("Plane not found.");
                    }
                    else
                    {
                        PrintPlaneDetails(plane);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid ID format.");
                }
            }
            else if (choice == "2")
            {
                Console.Write("Enter name: ");
                string name = Console.ReadLine();

                var result = _manager.GetAllPlanes()
                                     .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                                     .ToList();

                if (result.Count == 0)
                    Console.WriteLine("No planes match the search criteria.");
                else
                    result.ForEach(PrintPlaneDetails);
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }

            MenuHelpers.Wait();
        }

        private void PrintPlaneDetails(Plane p)
        {
            Console.WriteLine($"\n[{p.Id}] {p.Name} ({p.YearOfProduction})");
            Console.WriteLine($"Seats:");
            foreach (var kvp in p.Seats)
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            Console.WriteLine();
        }


        private void DeletePlane()
        {
            Console.Clear();
            Console.WriteLine("DELETE PLANE");
            Console.WriteLine("1 - By ID");
            Console.WriteLine("2 - By Name");
            Console.Write("Choice: ");

            string choice = Console.ReadLine();

            Plane planeToDelete = null;

            if (choice == "1")
            {
                Console.Write("Enter ID: ");
                string idInput = Console.ReadLine();

                if (Guid.TryParse(idInput, out Guid id))
                {
                    planeToDelete = _manager.GetPlaneById(id);
                }
            }
            else if (choice == "2")
            {
                Console.Write("Enter Name: ");
                string name = Console.ReadLine();
                planeToDelete = _manager.GetAllPlanes()
                    .FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                Console.WriteLine("Invalid choice.");
                MenuHelpers.Wait();
                return;
            }

            if (planeToDelete == null)
            {
                Console.WriteLine("Plane not found.");
                MenuHelpers.Wait();
                return;
            }

            _manager.RemovePlane(planeToDelete);

            Console.WriteLine($"Plane '{planeToDelete.Name}' deleted successfully.");
            MenuHelpers.Wait();
        }

        public void Show()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("PLANE MENU");
                Console.WriteLine("1 - Add Plane");
                Console.WriteLine("2 - List Planes");
                Console.WriteLine("3 - Search Plane");
                Console.WriteLine("4 - Delete Plane");
                Console.WriteLine("5 - Back");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddPlane(); break;
                    case "2": ListPlanes(); break;
                    case "3": SearchPlane(); break;
                    case "4": DeletePlane(); break;
                    case "5": running = false; break;
                    default:
                        Console.WriteLine("Invalid option.");
                        MenuHelpers.Wait();
                        break;
                }
            }
        }
    }


    public class CrewMenu
    {
        private readonly CrewManager _manager;

        public CrewMenu(CrewManager manager)
        {
            _manager = manager;
        }

        private void AddCrewMember()
        {
            Console.Clear();
            Console.WriteLine("ADD CREW MEMBER");

            Console.Write("First Name (Enter to cancel): ");
            string firstName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstName)) return;

            Console.Write("Last Name (Enter to cancel): ");
            string lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName)) return;

            int year;
            while (true)
            {
                Console.Write("Year of Birth (Enter to cancel): ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return;

                if (!int.TryParse(input, out year))
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }

                int age = DateTime.Now.Year - year;
                if (age < 18 || age > 100)
                {
                    Console.WriteLine("Crew member must be between 18 and 100 years old.");
                    continue;
                }
                break;
            }

            Console.Write("Gender (Enter to cancel): ");
            string gender = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(gender)) return;

            Position position;
            while (true)
            {
                Console.WriteLine("Position options: Pilot, CoPilot, Steward, Stewardess");
                Console.Write("Position: ");
                string posInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(posInput)) return;

                if (!Enum.TryParse(posInput, true, out position))
                {
                    Console.WriteLine("Invalid position. Try again.");
                    continue;
                }
                break;
            }

            var member = new CrewMember(firstName, lastName, year, gender, position);
            _manager.AddCrewMember(member);

            Console.WriteLine($"\nCrew member '{firstName} {lastName}' added successfully!");
            MenuHelpers.Wait();
        }

        private void ListCrewMembers()
        {
            Console.Clear();
            Console.WriteLine("LIST OF CREW MEMBERS");

            var crew = _manager.GetAllCrewMembers();
            if (crew.Count == 0)
            {
                Console.WriteLine("No crew members found.");
            }
            else
            {
                foreach (var c in crew)
                {
                    Console.WriteLine($"{c.Id} | {c.FirstName} {c.LastName}, {c.CrewPosition}, Born: {c.YearOfBirth}, Gender: {c.Gender}");
                }
            }

            MenuHelpers.Wait();
        }

        private CrewMember SelectCrewMember(List<CrewMember> available, string role)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"SELECT {role.ToUpper()}");

                var filtered = available
                    .Where(c =>
                        (role == "Pilot" && c.CrewPosition == Position.Pilot) ||
                        (role == "CoPilot" && c.CrewPosition == Position.CoPilot) ||
                        (role == "Steward" &&
                            (c.CrewPosition == Position.Steward || c.CrewPosition == Position.Stewardess)))
                    .ToList();

                if (filtered.Count == 0)
                {
                    Console.WriteLine($"No available {role}s!");
                    return null;
                }

                for (int i = 0; i < filtered.Count; i++)
                {
                    var c = filtered[i];
                    Console.WriteLine($"{i + 1} - {c.FirstName} {c.LastName} ({c.CrewPosition})");
                }

                Console.Write("Choose number (Enter to cancel): ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return null;

                if (!int.TryParse(input, out int index) || index < 1 || index > filtered.Count)
                {
                    Console.WriteLine("Invalid selection.");
                    MenuHelpers.Wait();
                    continue;
                }

                return filtered[index - 1];
            }
        }

        private void CreateCrew()
        {
            Console.Clear();
            Console.WriteLine("CREATE CREW");

            var allMembers = _manager.GetAllCrewMembers();
            var existingCrews = _manager.GetAllCrews();

            var unavailable = existingCrews
                .SelectMany(c => new[] { c.Pilot, c.CoPilot, c.Steward1, c.Steward2 })
                .Where(m => m != null);

            var available = allMembers.Except(unavailable).ToList();

            if (available.Count == 0)
            {
                Console.WriteLine("No available crew members to form a crew!");
                MenuHelpers.Wait();
                return;
            }

            var pilot = SelectCrewMember(available, "Pilot");
            if (pilot == null) return;
            available.Remove(pilot);

            var copilot = SelectCrewMember(available, "CoPilot");
            if (copilot == null) return;
            available.Remove(copilot);

            var steward1 = SelectCrewMember(available, "Steward");
            if (steward1 == null) return;
            available.Remove(steward1);

            var steward2 = SelectCrewMember(available, "Steward");
            if (steward2 == null) return;
            available.Remove(steward2);

            var crew = new Crew
            {
                Pilot = pilot,
                CoPilot = copilot,
                Steward1 = steward1,
                Steward2 = steward2
            };

            _manager.AddCrew(crew);

            Console.WriteLine($"\nCrew '{crew.Name}' created successfully!");
            MenuHelpers.Wait();
        }

        private void ListCrews()
        {
            Console.Clear();
            Console.WriteLine("LIST OF CREWS");

            var crews = _manager.GetAllCrews();

            if (crews.Count == 0)
            {
                Console.WriteLine("No crews found.");
            }
            else
            {
                foreach (var c in crews)
                {
                    Console.WriteLine($"Crew ID: {c.Id} | Name: {c.Name}");
                    Console.WriteLine($"  Pilot    : {c.Pilot.FirstName} {c.Pilot.LastName}");
                    Console.WriteLine($"  CoPilot  : {c.CoPilot.FirstName} {c.CoPilot.LastName}");
                    Console.WriteLine($"  Steward1 : {c.Steward1.FirstName} {c.Steward1.LastName}");
                    Console.WriteLine($"  Steward2 : {c.Steward2.FirstName} {c.Steward2.LastName}");
                    Console.WriteLine("---------------------------------------");
                }
            }

            MenuHelpers.Wait();
        }

        public void Show()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("CREW MENU");
                Console.WriteLine("1 - Add Crew Member");
                Console.WriteLine("2 - List Crew Members");
                Console.WriteLine("3 - Create Crew");
                Console.WriteLine("4 - List Crews");
                Console.WriteLine("5 - Back");
                Console.Write("Choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddCrewMember();
                        break;
                    case "2":
                        ListCrewMembers();
                        break;
                    case "3":
                        CreateCrew();
                        break;
                    case "4":
                        ListCrews();
                        break;
                    case "5":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        MenuHelpers.Wait();
                        break;
                }
            }
        }
    }

}
