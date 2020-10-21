using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicRestAPI.Database;
using BasicRestAPI.Model;
using BasicRestAPI.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace BasicRestAPI.Repositories
{
    // Repository pattern: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design#the-repository-pattern
    // This class will be further expanded in later lessons, when we are talking about interfacing with databases.
    public class GarageRepository : IGarageRepository
    {
        private readonly GarageDatabaseContext _context;

        public GarageRepository(GarageDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Garage>> GetAllGarages()
        {
            return await _context.Garages.ToListAsync();
        }

        public async Task<Garage> GetOneGarageById(int id)
        {
            return await _context.Garages.FindAsync(id);
        }

        public async Task Delete(int id)
        {
            var garage = await _context.Garages.FindAsync(id);
            if (garage == null)
            {
                throw new NotFoundException();
            }

            _context.Garages.Remove(garage);
            await _context.SaveChangesAsync();
        }

        public async Task<Garage> Insert(string name)
        {
            var garage = new Garage
            {
                Name = name
            };
            await _context.Garages.AddAsync(garage);
            await _context.SaveChangesAsync();
            return garage;
        }

        public async Task<Garage> Update(int id, string name)
        {
            var garage = await _context.Garages.FindAsync(id);
            if (garage == null)
            {
                throw new NotFoundException();
            }

            garage.Name = name;
            await _context.SaveChangesAsync();
            return garage;
        }
    }
}