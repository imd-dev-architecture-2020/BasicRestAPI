using System.Collections.Generic;
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
}