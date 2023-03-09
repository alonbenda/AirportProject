using AirportProject.Hubs;
using AirportProject.Models;
using Microsoft.AspNetCore.SignalR;
using static System.Collections.Specialized.BitVector32;

namespace AirportProject.BL.Routes
{
    public class AllRoutes
    {
        static Station firstStation;
        static Station secondStation;
        static Station thirdStation;
        static Station forthStation;
        static Station fifthStation;
        static Station sixthStation;
        static Station seventhStation;
        static Station eighthStation;
        static Station ninthStation;

        static Station emergancyStation;

        public AllRoutes()
        {
            firstStation = new Station(1);
            secondStation = new Station(2);
            thirdStation = new Station(3);
            forthStation = new Station(4);
            fifthStation = new Station(5);
            sixthStation = new Station(6);
            seventhStation = new Station(7);
            eighthStation = new Station(8);
            ninthStation = new Station(9);

            emergancyStation = new Station(0);

            landingRoute = new List<Station> { firstStation, secondStation, thirdStation, forthStation, fifthStation };

            takeoffRoute = new List<Station> { eighthStation, forthStation, ninthStation };
        }

        private List<Station> landingRoute;

        public List<Station> LandingRoute
        {
            get { return landingRoute; }
        }

        private List<Station> takeoffRoute;

        public List<Station> TakeoffRoute
        {
            get { return takeoffRoute; }
        }

        public static async Task Emergancy(Airplane airplane, IHubContext<AirportHub, IAirportHub> hub)
        {
            await emergancyStation.Enter(airplane);
            Console.WriteLine($"{airplane.Name}'s ENTER EMERGANCY STATION!");
            await hub.Clients.All.UpdateAirport(new List<Station> { emergancyStation });
            await Task.Delay(5000);

            emergancyStation.Exit(airplane);
            Console.WriteLine($"{airplane.Name}'s EXIT EMERGANCY STATION!");
            await hub.Clients.All.UpdateAirport(new List<Station> { emergancyStation });
            airplane.IsOk = true;
        }

        static async Task<Station> lll(Airplane airplane, SemaphoreSlim sem)
        {
            Station station1 = null;
            await Task.Run(async () =>
            {
                if (airplane.IsLanding.Value)
                {
                    while (true)
                    {
                        await sem.WaitAsync();
                        if (sixthStation.State && seventhStation.State)
                        {
                            sem.Release();
                            continue;
                        }
                        else if (sixthStation.Plane != null && seventhStation.Plane == null)
                        {
                            station1 = seventhStation;
                        }
                        else if (seventhStation.Plane != null && sixthStation.Plane == null)
                        {
                            station1 = sixthStation;
                        }
                        else
                        {
                            station1 = sixthStation;
                        }
                        await station1.Enter(airplane);
                        sem.Release();
                        break;
                    }
                }
                else if (!airplane.IsLanding.Value)
                {
                    while (true)
                    {
                        await sem.WaitAsync();
                        if (sixthStation.State && seventhStation.State)
                        {
                            sem.Release();
                            continue;
                        }
                        else if (sixthStation.Plane != null && seventhStation.Plane == null)
                        {
                            if (sixthStation.Plane.IsLanding!.Value)
                            {
                                station1 = seventhStation;
                                await station1.Enter(airplane);
                                sem.Release();
                                break;
                            }
                        }
                        else if (seventhStation.Plane != null && sixthStation.Plane == null)
                        {
                            if (seventhStation.Plane.IsLanding!.Value)
                            {
                                station1 = sixthStation;
                                await station1.Enter(airplane);
                                sem.Release();
                                break;
                            }
                        }
                        else
                        {
                            station1 = sixthStation;
                            await station1.Enter(airplane);
                            sem.Release();
                            break;
                        }
                        sem.Release();
                    }
                }
            });
            return station1;
        }

        public static Station LoadOrUnloadPassengers(Airplane airplane, SemaphoreSlim sem)
        {
            Station station = null;
            if (airplane == null || !airplane.IsLanding.HasValue)
            {
                return station;
            }
            station = lll(airplane, sem).Result;
            return station;
        }
    }
}
