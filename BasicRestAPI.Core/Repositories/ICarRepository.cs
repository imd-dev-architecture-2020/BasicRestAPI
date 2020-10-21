using System.Collections.Generic;
using System.Threading.Tasks;
using BasicRestAPI.Model.Domain;

namespace BasicRestAPI.Repositories
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCars(int garageId);
        Task<Car> GetOneCarById(int garageId, int carId);
        Task Delete(int garageId, int carId);
        Task<Car> Insert(int garageId, string name, Brand brand, string color);
        Task<Car> Update(int garageId, int carId, string name, Brand brand);
    }
}