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
        
        [Required]
        public string Color { get; set; }
    }
}