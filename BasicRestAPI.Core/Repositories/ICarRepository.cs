using System.Collections.Generic;
using BasicRestAPI.Model.Domain;

namespace BasicRestAPI.Repositories
{
    public interface ICarRepository
    {
        IEnumerable<Car> GetAllCars(int garageId);
        Car GetOneCarById(int garageId, int carId);
        void Delete(int garageId, int carId);
        Car Insert(int garageId, string name, Brand brand, string color);
        Car Update(int garageId, int carId, string name, Brand brand);
    }
}