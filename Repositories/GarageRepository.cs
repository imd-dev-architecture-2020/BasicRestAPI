using System.Collections.Generic;
using System.Linq;
using BasicRestAPI.Database;
using BasicRestAPI.Model;
using BasicRestAPI.Model.Domain;

namespace BasicRestAPI.Repositories
{

    public interface IGarageRepository
    {
        IEnumerable<Garage> GetAllGarages();
        Garage GetOneGarageById(int id);
        void Delete(int id);
        Garage Insert(string name);
        Garage Update(int id, string name);
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