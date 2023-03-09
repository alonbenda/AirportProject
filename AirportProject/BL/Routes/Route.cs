using AirportProject.Models;

namespace AirportProject.BL.Routes
{
    public class Route
    {
        readonly AllRoutes _allRoutes;

        public Route(AllRoutes allRoutes)
        {
            _allRoutes = allRoutes;
        }

        public List<Station> GetLandingRoute()
        {
            return _allRoutes.LandingRoute;
        }

        public List<Station> GetTakeoffRoute()
        {
            return _allRoutes.TakeoffRoute;
        }
    }
}
