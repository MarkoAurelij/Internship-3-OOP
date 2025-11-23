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
                Console.WriteLine("=== MAIN MENU ===");
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
                        new PassengerMenu(_passengerManager).Show();
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
        public PassengerMenu(PassengerManager manager) { _manager = manager; }
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

        public void Show()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("PASSENGER MENU");
                Console.WriteLine("1 - Register Passenger");
                Console.WriteLine("2 - List Passengers");
                Console.WriteLine("3 - Back");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterPassenger();
                        break;
                    case "2":
                        ListPassengers();
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
                Console.WriteLine("Select a Plane (Use ← / → keys, Enter to confirm):");
                int visibleCount = Math.Min(3, planes.Count);
                int start = Math.Max(0, planeIndex - 1);
                int end = Math.Min(planes.Count, start + visibleCount);

                for (int i = start; i < end; i++)
                {
                    Console.Write(i == planeIndex ? "> " : "  ");
                    var p = planes[i];
                    Console.WriteLine($"{i + 1} - {p.Name} ({p.YearOfProduction}) - Seats: {string.Join(", ", p.Seats.Keys)}");
                }

                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.RightArrow) planeIndex = (planeIndex + 1) % planes.Count;
                if (key == ConsoleKey.LeftArrow) planeIndex = (planeIndex - 1 + planes.Count) % planes.Count;
            } while (key != ConsoleKey.Enter);

            Plane selectedPlane = planes[planeIndex];

            var allCrew = _crewManager.GetAllCrewMembers();
            if (allCrew.Count == 0)
            {
                Console.WriteLine("No crew members available. Cannot create flight.");
                MenuHelpers.Wait();
                return;
            }

            List<CrewMember> selectedCrew = new List<CrewMember>();
            int crewStartIndex = 0;
            const int crewVisible = 5;
            bool crewSelecting = true;

            while (crewSelecting)
            {
                Console.Clear();
                Console.WriteLine("Select Crew Members (Space to select/deselect, Enter to confirm, arrows to scroll):");
                int crewEndIndex = Math.Min(allCrew.Count, crewStartIndex + crewVisible);

                for (int i = crewStartIndex; i < crewEndIndex; i++)
                {
                    var c = allCrew[i];
                    string marker = selectedCrew.Contains(c) ? "[X]" : "[ ]";
                    Console.WriteLine($"{i + 1} {marker} {c.FirstName} {c.LastName} ({c.CrewPosition})");
                }

                var cKey = Console.ReadKey(true).Key;
                if (cKey == ConsoleKey.DownArrow && crewStartIndex + crewVisible < allCrew.Count) crewStartIndex++;
                if (cKey == ConsoleKey.UpArrow && crewStartIndex > 0) crewStartIndex--;
                if (cKey == ConsoleKey.Enter) crewSelecting = false;

                if (cKey == ConsoleKey.Spacebar)
                {
                    var firstVisible = allCrew[crewStartIndex];
                    if (selectedCrew.Contains(firstVisible)) selectedCrew.Remove(firstVisible);
                    else selectedCrew.Add(firstVisible);
                }
            }

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
                    Console.WriteLine($"[{f.Id}] {f.Name}: {f.Origin} -> {f.Destination}, Departure: {f.Departure}, Arrival: {f.Arrival}, Distance: {f.Distance} km, Plane: {planeName}, Crew: {f.Crew.Count}");
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

                if (!int.TryParse(input, out year) || year < 1900 || year > DateTime.Now.Year)
                {
                    Console.WriteLine("Invalid year. Must be between 1900 and current year.");
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
                        Console.WriteLine("Invalid number of seats. Must be 0 or positive.");
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
                    Console.WriteLine($"[{p.Id}] {p.Name} ({p.YearOfProduction}), Seats: Standard-{p.Seats["Standard"]}, Business-{p.Seats["Business"]}, VIP-{p.Seats["VIP"]}");
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
                Console.WriteLine("PLANE MENU");
                Console.WriteLine("1 - Add Plane");
                Console.WriteLine("2 - List Planes");
                Console.WriteLine("3 - Back");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddPlane();
                        break;
                    case "2":
                        ListPlanes();
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

    public class CrewMenu
    {
        private readonly CrewManager _manager;
        public CrewMenu(CrewManager manager) { _manager = manager; }
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
                    Console.WriteLine($"{c.FirstName} {c.LastName}, {c.CrewPosition}, Born: {c.YearOfBirth}, Gender: {c.Gender}");
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
                Console.WriteLine("3 - Back");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCrewMember();
                        break;
                    case "2":
                        ListCrewMembers();
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
}
