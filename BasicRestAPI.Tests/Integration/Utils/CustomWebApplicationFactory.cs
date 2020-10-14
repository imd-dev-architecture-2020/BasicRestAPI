using System;
using System.Linq;
using BasicRestAPI.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BasicRestAPI.Tests.Integration.Utils
{
    // Used for integration testing, based on https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<GarageDatabaseContext>));

                services.Remove(descriptor);

                services.AddDbContextPool<GarageDatabaseContext>(options =>
                {
                    // notice that we use "InMemoryDatabase" here.
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<GarageDatabaseContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                db.Database.EnsureCreated();
            });
        }

        // You can think of Action<...> as a reference to a method that is being passed.
        public void ResetAndSeedDatabase(Action<GarageDatabaseContext> contextFiller)
        {
            // Retrieve a service scope and a database-context instance.
            using var scope = Services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var db = scopedServices.GetRequiredService<GarageDatabaseContext>();
            // Clear the database
            db.Cars.RemoveRange(db.Cars.ToList());
            db.Garages.RemoveRange(db.Garages.ToList());
            db.SaveChanges();

            // execute the method using retrieved database as parameter
            contextFiller(db);

            db.SaveChanges();
        }
    }
}
