using System.Linq;
using BasicRestAPI.Model;
using BasicRestAPI.Model.Web;
using BasicRestAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BasicRestAPI.Controllers
{
    [ApiController]
    [Route("garages")]
    public class CarController : ControllerBase
    {
        private readonly ILogger<GarageController> _logger;
        private readonly ICarRepository _carRepository;

        // notice that we pass in the repository instead of the database directly, this means you "abstract away" any database details.
        // this is useful when you want to replace your relational database with a document db for example.
        public CarController(ICarRepository carRepository, ILogger<GarageController> logger)
        {
            _logger = logger;
            _carRepository = carRepository;
        }


        [HttpGet("{garageId}/cars")]
        public IActionResult GetAllCarsForGarage(int garageId)
        {
            _logger.LogInformation($"Getting all cars for garage {garageId}");
            try
            {
                return Ok(_carRepository.GetAllCars(garageId).Select(x => x.Convert()).ToList());
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{garageId}/cars")]
        public IActionResult AddCarToGarage(int garageId, CarUpsertInput input)
        {
            _logger.LogInformation($"Creating a car for garage {garageId}");
            try
            {
                var persistedCar = _carRepository.Insert(id, input.Name, input.Brand);
                return Created($"/garages/{garageId}/cars/{persistedCar.Id}", persistedCar.Convert());
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPatch("{id}/cars/{carId}")]
        public IActionResult UpdateCarInGarage(int garageId, int carId, CarUpsertInput input)
        {
            _logger.LogInformation($"Updating car {carId} for garage {garageId}");
            try
            {
                _carRepository.Update(garageId, carId, input.Name, input.Brand);
                return Accepted();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}/cars/{carId}")]
        public IActionResult DeleteCarFromGarage(int garageId, int carId)
        {
            _logger.LogInformation($"Deleting car {carId} from garage {garageId}");
            try
            {
                _carRepository.Delete(garageId, carId);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}