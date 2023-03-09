using AirportProject.BL.Routes;
using AirportProject.Hubs;
using AirportProject.Models;
using Microsoft.AspNetCore.SignalR;

namespace AirportProject.BL.Landings
{
    public class Landing : ILanding
    {
        private readonly Airplane _airplane;
        private readonly List<Station> _route;
        private readonly IHubContext<AirportHub, IAirportHub> _hub;

        SemaphoreSlim _sem;

        public Landing(Airplane airplane, List<Station> route, IHubContext<AirportHub, IAirportHub> hub, SemaphoreSlim sem)
        {
            _airplane = airplane;
            _route = route;
            _hub = hub;
            _sem = sem;
        }

        public async Task Land()
        {
            await Task.Run(async () =>
            {
                _airplane.IsInAirport = true;
                Station? prevStation = null;
                foreach (var station in _route)
                {
                    await station!.Enter(_airplane);
                    Console.WriteLine($"{_airplane.Name}, landing, ENTER station={station.Id}");

                    if (prevStation != null)
                    {
                        prevStation.Exit(_airplane);
                        Console.WriteLine($"{_airplane.Name}, landing, EXIT station={prevStation.Id}");
                    }
                    await _hub.Clients.All.UpdateAirport(new List<Station> { station, prevStation });
                    await Task.Delay(3000);
                    prevStation = station;

                    if (_airplane.CheckProblems())
                    {
                        _airplane.IsOk = false;
                        Console.WriteLine($"{_airplane.Name}'s has a problem!");
                        await _hub.Clients.All.UpdateAirport(new List<Station> { station });
                        await Task.Delay(10000);

                        station.Exit(_airplane);
                        Console.WriteLine($"{_airplane.Name}, landing, EXIT station={station.Id}");
                        await _hub.Clients.All.UpdateAirport(new List<Station> { station });

                        await AllRoutes.Emergancy(_airplane, _hub);
                        _airplane.IsLanding = null;
                        return;
                    }
                }

                Station currentStation = null;
                if (prevStation?.Id == 5)
                {
                    currentStation = AllRoutes.LoadOrUnloadPassengers(_airplane, _sem);

                    prevStation?.Exit(_airplane);

                    Console.WriteLine($"{_airplane.Name}, landing, ENTER station={currentStation.Id}");
                    Console.WriteLine($"{_airplane.Name}, landing, EXIT station={prevStation!.Id}");

                    await _hub.Clients.All.UpdateAirport(new List<Station> { prevStation, currentStation });
                    await Task.Delay(5000);
                }

                currentStation.Exit(_airplane);
                Console.WriteLine($"{_airplane.Name}, landing, EXIT station={currentStation.Id}");
                await _hub.Clients.All.UpdateAirport(new List<Station> { currentStation });
                _airplane.IsLanding = null;
            });
        }
    }
}