using System.Collections.Generic;
using BasicRestAPI.Models.Domain;

namespace BasicRestAPI.Database
{
    // Obviously not a good idea to keep everything in memory but this will have to do for now.
    public class InMemoryDatabase : IInMemoryDatabase
    {
        public ICollection<Garage> Garages { get; private set; }

        public InMemoryDatabase()
        {
            Garages = new List<Garage> {
                new Garage { Name = "Garage 001",
                Id = 1,
                Cars = new [] {
                    new Car { Brand = Brand.Opel, Name = "Car 001.001" },
                    new Car { Brand = Brand.Opel, Name = "Car 001.002" },
                    new Car { Brand = Brand.Opel, Name = "Car 001.003" },
                }},
                new Garage { Name = "Garage 002",
                Id = 2,
                Cars = new [] {
                    new Car { Brand = Brand.Volkswagen, Name = "Car 002.001" },
                    new Car { Brand = Brand.Volkswagen, Name = "Car 002.002" },
                    new Car { Brand = Brand.Volkswagen, Name = "Car 002.003" },
                }},
                new Garage { Name = "Garage 003" ,
                Id = 3,
                Cars = new [] {
                    new Car { Brand = Brand.Volkswagen, Name = "Car 003.001" },
                    new Car { Brand = Brand.Opel, Name = "Car 003.002" },
                    new Car { Brand = Brand.Volkswagen, Name = "Car 003.003" },
                }}};
        }
    }
}