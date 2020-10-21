using System.Collections.Generic;
using System.Threading.Tasks;
using BasicRestAPI.Model.Domain;

namespace BasicRestAPI.Repositories
{
    public interface IGarageRepository
    {
        Task<IEnumerable<Garage>> GetAllGarages();
        Task<Garage> GetOneGarageById(int id);
        Task Delete(int id);
        Task<Garage> Insert(string name);
        Task<Garage> Update(int id, string name);
    }
}