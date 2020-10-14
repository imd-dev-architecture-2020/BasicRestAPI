using BasicRestAPI.Model.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;

namespace BasicRestAPI.Database
{
    // This is a our gateway to the database.
    public class GarageDatabaseContext : DbContext
    {
        public GarageDatabaseContext(DbContextOptions<GarageDatabaseContext> ctx) : base(ctx)
        {
            
        }

        // A DbSet can be used to add/query items. It maps to a table.
        public DbSet<Garage> Garages { get; set; }
        public DbSet<Car> Cars { get; set; }
    }
}