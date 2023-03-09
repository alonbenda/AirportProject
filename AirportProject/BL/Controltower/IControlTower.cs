using AirportProject.BL.Routes;
using Route = AirportProject.BL.Routes.Route;

namespace AirportProject.BL.Controltower
{
    public interface IControlTower
    {
        Route GetRoute();
    }
}