using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<Car> GetAllCars(int garageId)
        {
            var garageWithCars = _context.Garages
                .Include(x => x.Cars)
                .FirstOrDefault(x => x.Id == garageId);
            if (garageWithCars == null)
            {
                throw new NotFoundException();
            }

            return garageWithCars.Cars;
        }

        public Car GetOneCarById(int garageId, int carId)
        {
            CheckGarageExists(garageId);

            var car = _context.Cars.FirstOrDefault(x => x.GarageId == garageId && x.Id == carId);
            if (car == null)
            {
                throw new NotFoundException();
            }

            return car;
        }


        public void Delete(int garageId, int carId)
        {
            var car = GetOneCarById(garageId, carId);
            _context.Cars.Remove(car);
            _context.SaveChanges();
        }

        public Car Insert(int garageId, string name, Brand brand, string color)
        {
            CheckGarageExists(garageId);
            var car = new Car()
            {
                Brand = brand,
                Name = name,
                GarageId = garageId,
                Color = color
            };
            _context.Cars.Add(car);
            _context.SaveChanges();
            return car;

        }

        public Car Update(int garageId, int carId, string name, Brand brand)
        {
            var car = GetOneCarById(garageId, carId);
            car.Brand = brand;
            car.Name = name;
            _context.SaveChanges();
            return car;
        }

        private void CheckGarageExists(int garageId)
        {
            var garageCheck = _context.Garages.Find(garageId);
            if (garageCheck == null)
            {
                throw new NotFoundException();
            }
        }
    }
}