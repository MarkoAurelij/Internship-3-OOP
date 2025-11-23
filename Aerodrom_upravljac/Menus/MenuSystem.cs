using System;
using AirportManagement.Managers;

namespace AirportManagement.Menus
{
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
                        var passengerMenu = new PassengerMenu(_passengerManager);
                        passengerMenu.Show();
                        break;
                    case "2":
                        var flightMenu = new FlightMenu(_flightManager);
                        flightMenu.Show();
                        break;
                    case "3":
                        var planeMenu = new PlaneMenu(_planeManager);
                        planeMenu.Show();
                        break;
                    case "4":
                        var crewMenu = new CrewMenu(_crewManager);
                        crewMenu.Show();
                        break;
                    case "5":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }
            }
        }
    }