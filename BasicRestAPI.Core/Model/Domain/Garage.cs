using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BasicRestAPI.Model.Domain
{
    // this class inherits from BaseDatabaseClass, containing an ID and other shared properties
    public class Garage : BaseDatabaseClass
    {
        // this or put them underneath each other like Car.Name
        [Required, MaxLength(2048)]
        public string Name { get; set; }
        
        public IEnumerable<Car> Cars { get; set; }
    }
}