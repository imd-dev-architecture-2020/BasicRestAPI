using BasicRestAPI.Model.Domain;

namespace BasicRestAPI.Model.Web
{
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