using System.ComponentModel.DataAnnotations;

namespace BasicRestAPI.Model.Domain
{
    public abstract class BaseDatabaseClass
    {   
        [Key]
        public int Id { get; set; }
    }
}