using System.Collections.Generic;
using BasicRestAPI.Models.Domain;

namespace BasicRestAPI.Database
{
    // It's always a good idea to program to interfaces
    // https://blog.ndepend.com/programming-interface-simple-explanation/
    public interface IInMemoryDatabase
    {
        ICollection<Garage> Garages { get; }
    }
}