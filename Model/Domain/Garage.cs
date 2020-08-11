using System.Collections.Generic;

namespace BasicRestAPI.Models.Domain
{
    public class Garage : BaseDatabaseClass
    {
        public string Name { get; set; }
        public IEnumerable<Car> Cars { get; set; }
    }
}