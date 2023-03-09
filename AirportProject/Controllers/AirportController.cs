using AirportProject.BL.Airport;
using Microsoft.AspNetCore.Mvc;

namespace AirportProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly IAirportLogic _airportLogic;

        public AirportController(IAirportLogic airportLogic)
        {
            _airportLogic = airportLogic;
        }

        [HttpGet("/land/{airplaneName}")]
        public bool Land(string airplaneName)
        {
            return _airportLogic.Land(airplaneName);
        }

        [HttpGet("/takeoff/{airplaneName}")]
        public bool TakeOff(string airplaneName)
        {
            return _airportLogic.TakeOff(airplaneName);
        }
    }
}
