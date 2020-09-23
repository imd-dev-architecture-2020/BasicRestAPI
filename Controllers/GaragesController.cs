using System;
using System.Linq;
using BasicRestAPI.Database;
using BasicRestAPI.Models.Domain;
using BasicRestAPI.Models.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BasicRestAPI.Controllers
{
    [ApiController]
    [Route("garages")]
    public class GarageController : ControllerBase
    {
        private readonly IInMemoryDatabase _database;
        private readonly ILogger<GarageController> _logger;

        public GarageController(IInMemoryDatabase database, ILogger<GarageController> logger)
        {
            _database = database;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllGarages()
        {
            _logger.LogInformation("Getting all garages");
            // This is a linq extension method: https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select?view=netcore-3.1
            var garages = _database.Garages.Select(x => new GarageWebOutput(x.Id, x.Name)).ToArray();
            return Ok(garages);
        }

        [HttpGet("{id}")]
        public IActionResult GarageById(int id)
        {
            _logger.LogInformation("Getting garage by id", id);
            // This is, for example, an method that is waiting to be extracted. 
            // Notice that the logic "retrieve a garage or return a 404" occurs 
            // in a lot of methods.
            var garage = _database.Garages.FirstOrDefault(x => x.Id == id);
            if (garage == null)
            {
                return NotFound();
            }
            return Ok(new GarageWebOutput(garage.Id, garage.Name));
        }

        [HttpPost]
        public IActionResult CreateGarage(GarageUpsertInput input)
        {
            _logger.LogInformation("Creating a garage", input);
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                return BadRequest(new Exception("Invalid input: name should be filled in"));
            }
            var garage = new Garage
            {
                // this is a bad idea, why?
                Id = _database.Garages.Select(x => x.Id).Max() + 1,
                Cars = Array.Empty<Car>(),
                Name = input.Name,
            };
            _database.Garages.Add(garage);
            return Created($"/garages/{garage.Id}", new GarageWebOutput(garage.Id, garage.Name));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGarage(int id, GarageUpsertInput input)
        {
            _logger.LogInformation("Updating a garage", input);
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                return BadRequest(new Exception("Invalid input: name should be filled in"));
            }
            if (id == 0)
            {
                return BadRequest(new Exception("Invalid input: id should not be equal to zero"));
            }
            // The next line is bad; why?
            var garage = _database.Garages.FirstOrDefault(x => x.Id == id);
            if (garage == null)
            {
                return NotFound();
            }
            garage.Name = input.Name;
            return Accepted(new GarageWebOutput(garage.Id, garage.Name));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGarage(int id)
        {
            _logger.LogInformation("Deleting a garage", id);
            var garage = _database.Garages.FirstOrDefault(x => x.Id == id);
            if (garage == null)
            {
                return NotFound();
            }
            _database.Garages.Remove(garage);
            return NoContent();
        }
    }
}
