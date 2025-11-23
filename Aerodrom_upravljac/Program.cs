using System;
using AirportManagement.Managers;
using AirportManagement.Menus;

namespace AirportManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize managers
            var passengerManager = new PassengerManager();
            var flightManager = new FlightManager();
            var planeManager = new PlaneManager();
            var crewManager = new CrewManager();


            var mainMenu = new MainMenu(passengerManager, flightManager, planeManager, crewManager);
            mainMenu.Show();

            Console.WriteLine("\nExiting program. Press any key to close...");
            Console.ReadKey();
        }
    }
}
