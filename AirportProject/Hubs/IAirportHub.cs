using AirportProject.Models;

namespace AirportProject.Hubs
{
    public interface IAirportHub
    {
        Task UpdateAirport(List<Station> stations);
    }
}
