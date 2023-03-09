using AirportProject.BL.Routes;
using AirportProject.Hubs;
using AirportProject.Models;
using Microsoft.AspNetCore.SignalR;

namespace AirportProject.BL.TakeOffs
{
    public class TakingOff : ITakingOff
    {
        private readonly Airplane _airplane;
        private readonly List<Station> _route;
        private readonly IHubContext<AirportHub, IAirportHub> _hub;

        SemaphoreSlim _sem;
        static SemaphoreSlim _takeOffSem = new(1);

        public TakingOff(Airplane airplane, List<Station> route, IHubContext<AirportHub, IAirportHub> hub, SemaphoreSlim sem)
        {
            _airplane = airplane;
            _route = route;
            _hub = hub;
            _sem = sem;
        }

        public async Task TakeOff()
        {
            await Task.Run(async () =>
            {
                Station? prevStation = null;

                await _takeOffSem.WaitAsync();
                prevStation = AllRoutes.LoadOrUnloadPassengers(_airplane, _sem);
                _takeOffSem.Release();

                Console.WriteLine($"{_airplane.Name}, taking off, ENTER station={prevStation.Id}");
                await _hub.Clients.All.UpdateAirport(new List<Station> { prevStation });
                await Task.Delay(5000);

                foreach (var station in _route)
                {
                    if (_airplane.CheckProblems())
                    {
                        _airplane.IsOk = false;
                        Console.WriteLine($"{_airplane.Name}'s has a problem!");
                        await _hub.Clients.All.UpdateAirport(new List<Station> { prevStation });
                        await Task.Delay(10000);

                        if (prevStation.Id == 6 || prevStation.Id == 7)
                        {
                            await _sem.WaitAsync();
                            prevStation.Exit(_airplane);
                            Console.WriteLine($"{_airplane.Name}, taking off, ENTER station={prevStation.Id}");
                            _sem.Release();
                        }
                        else
                        {
                            prevStation.Exit(_airplane);
                            Console.WriteLine($"{_airplane.Name}, taking off, ENTER station={prevStation.Id}");
                        }
                        await _hub.Clients.All.UpdateAirport(new List<Station> { prevStation });

                        await AllRoutes.Emergancy(_airplane, _hub);
                        _airplane.IsLanding = null;
                        return;
                    }

                    await station!.Enter(_airplane);
                    Console.WriteLine($"{_airplane.Name}, taking off, ENTER station={station.Id}");

                    if (prevStation != null)
                    {
                        if (prevStation.Id == 6 || prevStation.Id == 7)
                        {
                            await _sem.WaitAsync();
                            prevStation.Exit(_airplane);
                            Console.WriteLine($"{_airplane.Name}, taking off, EXIT station={prevStation.Id}");
                            _sem.Release();
                        }
                        prevStation.Exit(_airplane);
                        Console.WriteLine($"{_airplane.Name}, taking off, EXIT station={prevStation.Id}");
                    }

                    await _hub.Clients.All.UpdateAirport(new List<Station> { station, prevStation });
                    await Task.Delay(3000);
                    prevStation = station;
                }
                prevStation?.Exit(_airplane);
                Console.WriteLine($"{_airplane.Name}, taking off, EXIT station={prevStation!.Id}");
                await _hub.Clients.All.UpdateAirport(new List<Station> { prevStation });
                _airplane.IsLanding = null;
                _airplane.IsInAirport = false;
            });
        }
    }
}
