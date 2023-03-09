using AirportProject.BL.Routes;
using Route = AirportProject.BL.Routes.Route;

namespace AirportProject.BL.Controltower
{
    public class ControlTower : IControlTower
    {
        readonly AllRoutes _allRoutes;
        public ControlTower(AllRoutes allRoutes)
        {
            _allRoutes = allRoutes;
        }

        public Route GetRoute()
        {
            return new Route(_allRoutes);
        }
    }
}
