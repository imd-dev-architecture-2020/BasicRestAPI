using BasicRestAPI.Model.Domain;
using BasicRestAPI.Model.Web;

namespace BasicRestAPI.Model
{
    public static class Mappers
    {
        // more extension method fun: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
        public static GarageWebOutput Convert(this Garage input)
        {
            return new GarageWebOutput(input.Id, input.Name);
        }
    }
}
