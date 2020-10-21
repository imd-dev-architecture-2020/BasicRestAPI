using System.ComponentModel.DataAnnotations;
using BasicRestAPI.Model.Domain;

namespace BasicRestAPI.Model.Web
{
    public class CarUpsertInput
    {
        [Required]
        [StringLength(1000)]
        public string Name { get; set; }

        [Required]
        public Brand Brand { get; set; }
        
        // Don't write comments like this -- this is an example.
        /// <summary>
        /// The color of the car.
        /// </summary>
        [Required]
        public string Color { get; set; }
    }
}