using System.ComponentModel.DataAnnotations;

namespace BasicRestAPI.Model.Domain
{
    public class Car : BaseDatabaseClass
    {
        [Required]
        public Garage Garage { get; set; }

        public int GarageId { get; set; }
        
        [Required]
        [MaxLength(2048)]
        public string Name { get; set; }
        
        public Brand Brand { get; set; }

        [MaxLength(1024)]
        [Required]
        public string Color { get; set; }
    }
}