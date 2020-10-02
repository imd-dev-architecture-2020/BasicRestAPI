using BasicRestAPI.Model.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;

namespace BasicRestAPI.Database
{
    // This is a our gateway to the database. This will be further developed/explained during the lessons on Entity Framework.
    public class GarageDatabaseContext : DbContext
    {
        public GarageDatabaseContext(DbContextOptions<GarageDatabaseContext> ctx) : base(ctx)
        {
            
        }

        public DbSet<Garage> Garages { get; set; }
        public DbSet<Car> Cars { get; set; }
    }
}