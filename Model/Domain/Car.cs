namespace BasicRestAPI.Models.Domain
{
    public class Car : BaseDatabaseClass
    {
        public string Name { get; set; }
        public Brand Brand { get; set; }
    }
}