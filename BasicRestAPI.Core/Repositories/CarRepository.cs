using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicRestAPI.Database;
using BasicRestAPI.Model;
using BasicRestAPI.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace BasicRestAPI.Repositories
{
    // This class will be further expanded in later lessons, when we are talking about interfacing with databases.
    public class CarRepository : ICarRepository
    {
        private readonly GarageDatabaseContext _context;

        public CarRepository(GarageDatabaseContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Car>> GetAllCars(int garageId)
        {
            var garageWithCars = await _context.Garages
                .Include(x => x.Cars)
                .FirstOrDefaultAsync(x => x.Id == garageId);
            if (garageWithCars == null)
            {
                throw new NotFoundException();
            }

            return garageWithCars.Cars;
        }

        public async Task<Car> GetOneCarById(int garageId, int carId)
        {
            await CheckGarageExists(garageId);

            var car = await _context.Cars.FirstOrDefaultAsync(x => x.GarageId == garageId && x.Id == carId);
            if (car == null)
            {
                throw new NotFoundException();
            }

            return car;
        }


        public async Task Delete(int garageId, int carId)
        {
            var car = await GetOneCarById(garageId, carId);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
        }

        public async Task<Car> Insert(int garageId, string name, Brand brand, string color)
        {
            await CheckGarageExists(garageId);
            var car = new Car()
            {
                Brand = brand,
                Name = name,
                GarageId = garageId,
                Color = color
            };
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
            return car;

        }

        public async Task<Car> Update(int garageId, int carId, string name, Brand brand)
        {
            var car = await GetOneCarById(garageId, carId);
            car.Brand = brand;
            car.Name = name;
            await _context.SaveChangesAsync();
            return car;
        }

        private async Task CheckGarageExists(int garageId)
        {
            var garageCheck = await _context.Garages.FindAsync(garageId);
            if (garageCheck == null)
            {
                throw new NotFoundException();
            }
        }
    }
}