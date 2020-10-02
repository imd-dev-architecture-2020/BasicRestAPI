using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using BasicRestAPI.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BasicRestAPI.Tests.Integration.Utils
{
    public static class ContentHelper
    {
        public static StringContent GetStringContent(object obj)
            => new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
    }

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

                services.AddDbContext<GarageDatabaseContext>(options =>
                {
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

        public void ResetAndSeedDatabase(Action<GarageDatabaseContext> contextFiller)
        {
            using var scope = Services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var db = scopedServices.GetRequiredService<GarageDatabaseContext>();

            db.Cars.RemoveRange(db.Cars.ToList());
            db.Garages.RemoveRange(db.Garages.ToList());
            db.SaveChanges();

            contextFiller(db);

            db.SaveChanges();
        }
    }
}
