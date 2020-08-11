namespace BasicRestAPI.Models.Web
{
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
}