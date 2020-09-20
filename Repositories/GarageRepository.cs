using System.Collections.Generic;
using System.Linq;
using BasicRestAPI.Database;
using BasicRestAPI.Model;
using BasicRestAPI.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace BasicRestAPI.Repositories
{
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

        public Car Insert(int garageId, string name, Brand brand)
        {
            CheckGarageExists(garageId);
            var car = new Car()
            {
                Brand = brand,
                Name = name,
                GarageId = garageId
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

    // Repository pattern: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design#the-repository-pattern
    // This class will be further expanded in later lessons, when we are talking about interfacing with databases.
    public class GarageRepository : IGarageRepository
    {
        private readonly GarageDatabaseContext _context;

        public GarageRepository(GarageDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Garage> GetAllGarages()
        {
            return _context.Garages.ToList();
        }

        public Garage GetOneGarageById(int id)
        {
            return _context.Garages.Find(id);
        }

        public void Delete(int id)
        {
            var garage = _context.Garages.Find(id);
            if (garage == null)
            {
                throw new NotFoundException();
            }

            _context.Garages.Remove(garage);
            _context.SaveChanges();
        }

        public Garage Insert(string name)
        {
            var garage = new Garage
            {
                Name = name
            };
            _context.Garages.Add(garage);
            _context.SaveChanges();
            return garage;
        }

        public Garage Update(int id, string name)
        {
            var garage = _context.Garages.Find(id);
            if (garage == null)
            {
                throw new NotFoundException();
            }

            garage.Name = name;
            _context.SaveChanges();
            return garage;
        }
    }
}