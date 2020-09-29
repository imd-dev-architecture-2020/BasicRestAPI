using BasicRestAPI.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace BasicRestAPI.Database
{
    // This is a our gateway to the database. This will be further developed/explained during the lessons on Entity Framework.
    public class GarageDatabaseContext : DbContext
    {
        // this is, obviously, wrong. A full, dynamic configuration will be added late
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=garages.db");

        public DbSet<Garage> Garages { get; set; }
        public DbSet<Car> Cars { get; set; }
    }
}