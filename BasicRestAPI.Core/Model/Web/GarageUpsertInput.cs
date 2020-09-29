using System.ComponentModel.DataAnnotations;

namespace BasicRestAPI.Model.Web
{
    // This is incoming data
    // Debatable whether or not this is a good idea; normally you should split the POST/PUT messages.
    // However, since name is the only data we can change we reuse the class everywhere.
    public class GarageUpsertInput
    {
        // This is called "Model validation"
        // you can find more info @ https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-3.1#validation-attributes
        // and https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.1#automatic-http-400-responses
        [Required]
        [StringLength(1000)]
        public string Name { get; set; }
    }
}