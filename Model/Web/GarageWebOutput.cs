using BasicRestAPI.Model.Domain;

namespace BasicRestAPI.Model.Web
{
    // Notice that "cars" is absent. When you want to view a garage you only want to view the garage info, not the related cars.
    // Those cars will have their own DTOs (https://en.wikipedia.org/wiki/Data_transfer_object)
    public class GarageWebOutput
    {
        public GarageWebOutput(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CarWebOutput
    {
        public CarWebOutput(int id, string name, Brand brand)
        {
            Id = id;
            Name = name;
            Brand = brand;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Brand Brand { get; set; }
    }
}