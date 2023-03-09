using AirportProject.Models;

namespace AirportProject.BL.Airport
{
    public interface IAirportLogic
    {
        bool Land(string airplaneName);
        bool TakeOff(string airplaneName);
    }
}
