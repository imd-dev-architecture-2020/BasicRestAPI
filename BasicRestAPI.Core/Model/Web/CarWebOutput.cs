using System.ComponentModel.DataAnnotations;
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

        /// <summary>
        /// This is an identifier that is guaranteed to be unique within the context of a single instance of our API,
        /// </summary>
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public Brand Brand { get; set; }
    }
}