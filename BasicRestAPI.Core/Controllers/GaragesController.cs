using System.Collections.Generic;
using System.Linq;
using BasicRestAPI.Model;
using BasicRestAPI.Model.Web;
using BasicRestAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace BasicRestAPI.Controllers
{
    [ApiController]
    [Route("garages")]
    public class GarageController : ControllerBase
    {
        private readonly IGarageRepository _garageRepository;
        private readonly ILogger<GarageController> _logger;
        
        // notice that we pass in the repository instead of the database directly, this means you "abstract away" any database details.
        // this is useful when you want to replace your relational database with a document db for example.
        public GarageController(IGarageRepository garageRepository, ILogger<GarageController> logger)
        {
            _garageRepository = garageRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GarageWebOutput>), StatusCodes.Status200OK)]
        public IActionResult GetAllGarages()
        {
            _logger.LogInformation("Getting all garages");
            // This is a linq extension method: https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select?view=netcore-3.1
            var garages = _garageRepository.GetAllGarages().Select(x => x.Convert()).ToList();
            return Ok(garages);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GarageWebOutput), StatusCodes.Status200OK)]
        public IActionResult GarageById(int id)
        {
            _logger.LogInformation("Getting garage by id", id);
            var garage = _garageRepository.GetOneGarageById(id);
            // Ternary operator: https://en.wikipedia.org/wiki/%3F:
            return garage == null ? (IActionResult) NotFound() : Ok(garage.Convert());
        }

        [HttpPost]
        [ProducesResponseType(typeof(GarageWebOutput),StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public IActionResult CreateGarage(GarageUpsertInput input)
        {
            _logger.LogInformation("Creating a garage", input);
            var persistedGarage = _garageRepository.Insert(input.Name);
            return Created($"/garages/{persistedGarage.Id}", persistedGarage.Convert());
        }

        // this method went from a PUT to a PATCH. Why? (answer @ https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design#define-operations-in-terms-of-http-methods)
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateGarage(int id, GarageUpsertInput input)
        {
            _logger.LogInformation("Updating a garage", input);
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/try-catch
            try
            {
                _garageRepository.Update(id, input.Name);
                return Accepted();
            }
            catch (NotFoundException)
            {
                // we only catch the NotFoundException; only catch exceptions you explicitly want to behave different from the regular handling.
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult DeleteGarage(int id)
        {
            _logger.LogInformation("Deleting a garage", id);
            try
            {
                _garageRepository.Delete(id);
               return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
