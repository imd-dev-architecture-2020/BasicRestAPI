namespace BasicRestAPI.Model.Domain
{
    public class Car : BaseDatabaseClass
    {
        public int GarageId { get; set; }
        public string Name { get; set; }
        public Brand Brand { get; set; }
    }
}