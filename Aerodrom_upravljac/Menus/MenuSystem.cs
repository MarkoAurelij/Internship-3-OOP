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
                        new FlightMenu(_flightManager).Show();
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
            Console.WriteLine("=== REGISTER PASSENGER ===");

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
            Console.WriteLine("=== LIST OF PASSENGERS ===");

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
                Console.WriteLine("=== PASSENGER MENU ===");
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
        public FlightMenu(FlightManager manager) { _manager = manager; }
        private void AddFlight()
        {
            Console.Clear();
            Console.WriteLine("=== ADD FLIGHT ===");

            // Flight Name
            Console.Write("Flight Name (Enter to cancel): ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) return;

            // Origin
            Console.Write("Origin (Enter to cancel): ");
            string origin = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(origin)) return;

            // Destination
            Console.Write("Destination (Enter to cancel): ");
            string destination = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(destination)) return;

            // Departure
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

                if (!DateTime.TryParse(arrInput, out arrival))
                {
                    Console.WriteLine("Invalid format. Try again.");
                    continue;
                }

                if (arrival <= departure)
                {
                    Console.WriteLine("Arrival must be after departure.");
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

            Plane plane = null;
            Console.WriteLine("Plane selection not implemented yet.");
            MenuHelpers.Wait();

            List<CrewMember> crew = new List<CrewMember>();
            Console.WriteLine("Crew selection not implemented yet.");
            MenuHelpers.Wait();

            var flight = new Flight(name, origin, destination, departure, arrival, distance, plane, crew);
            _manager.AddFlight(flight);

            Console.WriteLine($"\nFlight '{name}' added successfully!");
            MenuHelpers.Wait();
        }
        public void Show()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== FLIGHT MENU ===");
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
                        MenuHelpers.Wait();
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

        public void Show()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== PLANE MENU ===");
                Console.WriteLine("1 - Add Plane");
                Console.WriteLine("2 - List Planes");
                Console.WriteLine("3 - Back");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                    case "2":
                        MenuHelpers.Wait();
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

        public void Show()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== CREW MENU ===");
                Console.WriteLine("1 - Add Crew Member");
                Console.WriteLine("2 - List Crew Members");
                Console.WriteLine("3 - Back");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                    case "2":
                        MenuHelpers.Wait();
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
