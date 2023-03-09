using AirportProject.BL.Routes;
using AirportProject.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace AirportProject.Models
{
    public class Airplane
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool? IsLanding { get; set; }
        public bool IsOk { get; set; } = true;
        public bool IsInAirport { get; set; } = false;


        public bool CheckProblems()
        {
            Random random = new Random();
            int num = random.Next(20);
            if (num == 1)
            {
                return true;
            }
            return false;
        }
    }
}
