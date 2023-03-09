using AirportProject.BL.Controltower;
using AirportProject.BL.Landings;
using AirportProject.BL.TakeOffs;
using AirportProject.Hubs;
using AirportProject.Models;
using Microsoft.AspNetCore.SignalR;
using Route = AirportProject.BL.Routes.Route;

namespace AirportProject.BL.Airport
{
    public class AirportLogic : IAirportLogic
    {
        private readonly IControlTower _controlTower;
        private readonly IHubContext<AirportHub, IAirportHub> _hub;

        readonly List<Airplane> landeds = new List<Airplane>();
        readonly List<Airplane> tookOffs = new List<Airplane>();
        readonly Route routes;

        static readonly object loadStationsLocker = new object();
        static SemaphoreSlim _sem = new SemaphoreSlim(1);

        public AirportLogic(IControlTower controlTower, IHubContext<AirportHub, IAirportHub> hub)
        {
            _controlTower = controlTower;
            _hub = hub;
            routes = _controlTower.GetRoute();
        }

        public bool Land(string airplaneName)
        {
            Landing(airplaneName);
            return true;
        }

        void Landing(string airplaneName)
        {
            Task.Run(async () =>
            {
                if (tookOffs.Count > 0)
                {
                    var p = tookOffs[0];
                    if (p.IsLanding == null)
                    {
                        p.IsLanding = true;
                        var l = new Landing(p, routes.GetLandingRoute(), _hub, _sem);

                        tookOffs.Remove(p);
                        await l.Land();
                        landeds.Add(p);
                    }
                }
                else
                {
                    var plane = new Airplane() { Name = airplaneName, IsLanding = true, IsInAirport = false };
                    var landing = new Landing(plane, routes.GetLandingRoute(), _hub, _sem);

                    await landing.Land();
                    landeds.Add(plane);
                }
            });
        }

        public bool TakeOff(string airplaneName)
        {
            TakingOff(airplaneName);
            return true;
        }

        void TakingOff(string airplaneName)
        {
            Task.Run(async () =>
            {
                if (landeds.Count + tookOffs.Count >= 10)
                {
                    if (landeds.Count > 0)
                    {
                        var p = landeds[0];
                        if (landeds[0].IsLanding == null)
                        {
                            p.IsLanding = false;
                            var t = new TakingOff(p, routes.GetTakeoffRoute(), _hub, _sem);

                            landeds.Remove(p);
                            await t.TakeOff();
                            if (!p.IsInAirport)
                            {
                                tookOffs.Add(p);
                            }
                            else
                            {
                                landeds.Add(p);
                            }
                        }
                    }
                }
                else
                {
                    var plane = new Airplane() { Name = airplaneName, IsLanding = false, IsInAirport = true };
                    var takeoff = new TakingOff(plane, routes.GetTakeoffRoute(), _hub, _sem);

                    await takeoff.TakeOff();
                    tookOffs.Add(plane);
                }
            });
        }
    }
}
