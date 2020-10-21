using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicRestAPI.Model;
using BasicRestAPI.Model.Web;
using BasicRestAPI.Repositories;
using Microsoft.AspNetCore.Http;
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


        /// <summary>
        /// Gets you a list of all the cars inside a garage. If the garage does not exist, you get a 404. An empty list means "no cars available".
        /// </summary>
        /// <param name="garageId">The unique identifier of the garage</param>
        /// <returns>A list of cars</returns>
        [HttpGet("{garageId}/cars")]
        [ProducesResponseType(typeof(IEnumerable<CarWebOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // this one is needed b/c we only handle NotFoundExceptions explicitly. Other errors just go through and throw a 400/500.
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllCarsForGarage(int garageId)
        {
            _logger.LogInformation($"Getting all cars for garage {garageId}");
            try
            {
                return Ok((await _carRepository.GetAllCars(garageId)).Select(x => x.Convert()).ToList());
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates a new car inside a garage. A 404 means that the garage with said id does not exist.
        /// </summary>
        /// <param name="garageId">The unique identifier of the garage</param>
        /// <param name="input">The body of the garage</param>
        /// <returns></returns>
        [HttpPost("{garageId}/cars")]
        [ProducesResponseType(typeof(CarWebOutput),StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddCarToGarage(int garageId, CarUpsertInput input)
        {
            _logger.LogInformation($"Creating a car for garage {garageId}");
            try
            {
                var persistedCar = await _carRepository.Insert(garageId, input.Name, input.Brand, input.Color);
                return Created($"/garages/{garageId}/cars/{persistedCar.Id}", persistedCar.Convert());
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPatch("{id}/cars/{carId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult DeleteCarFromGarage(int garageId, int carId)
        {
            _logger.LogInformation($"Deleting car {carId} from garage {garageId}");
            try
            {
                _carRepository.Delete(garageId, carId);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}