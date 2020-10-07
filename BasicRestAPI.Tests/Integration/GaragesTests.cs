using System.Threading.Tasks;
using BasicRestAPI.Model.Domain;
using BasicRestAPI.Model.Web;
using BasicRestAPI.Tests.Integration.Utils;
using FluentAssertions;
using Newtonsoft.Json;
using Snapshooter;
using Snapshooter.Xunit;
using Xunit;

namespace BasicRestAPI.Tests.Integration
{
    // In this test we test all the methods of the garages controller.
    // This is both pretty artificial (as the API doesn't know things as "controllers", only endpoints)
    // and easy for our examples. An alternative is splitting the tests over "retrieval" and "crud" tests,
    // or even a single test per method.
    public class GaragesTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GaragesTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        // A Task<T> is about the same as an Promise<T>, and we will talk about that in later lessons.
        // For now it's sufficient to know (but not entirely correct) ...
        //  - a method is async when you mark it as such (public async ... )
        //  - a method is async if you have an await in there somewhere
        //  - an async method always returns Task or Task<T> and needs to be awaited "further up the chain"
        [Fact]
        public async Task GetGaragesEndPointReturnsNoDataWhenDbIsEmpty()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });
            var response = await client.GetAsync("/garages");
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task GetGaragesEndPointReturnsSomeDataWhenDbIsNotEmpty()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) =>
            {
                db.Garages.Add(new Garage() {Id = 1, Name = "abc"});
                db.Garages.Add(new Garage() {Id = 2, Name = "def"});
            });
            var response = await client.GetAsync("/garages");
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task GetGarageById404IfDoesntExist()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });
            var response = await client.GetAsync("/garages/1");
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetGarageByIdReturnGarageIfExists()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) =>
            {
                db.Garages.Add(new Garage() {Id = 1, Name = "abcdef"});
            });
            var response = await client.GetAsync("/garages/1");
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task DeleteGarageByIdReturns404IfDoesntExist()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) =>
            {
                db.Garages.Add(new Garage() {Id = 1, Name = "abcdef"});
            });
            var response = await client.DeleteAsync("/garages/2");
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task DeleteGarageByIdReturnsDeletesIfExists()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) =>
            {
                db.Garages.Add(new Garage() {Id = 1, Name = "abcdef"});
            });
            var beforeDeleteResponse = await client.GetAsync("/garages/1");
            beforeDeleteResponse.EnsureSuccessStatusCode();
            var deleteResponse = await client.DeleteAsync("/garages/1");
            deleteResponse.EnsureSuccessStatusCode();
            var afterDeleteResponse = await client.GetAsync("/garages/1");
            afterDeleteResponse.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task InsertGarageReturnsCorrectData()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });

            var request = new
            {
                Body = new GarageUpsertInput
                {
                    Name = "hey"
                }
            };
            var createResponse = await client.PostAsync("/garages", ContentHelper.GetStringContent(request.Body));
            createResponse.EnsureSuccessStatusCode();
            var body = JsonConvert.DeserializeObject<GarageWebOutput>(await createResponse.Content.ReadAsStringAsync());
            body.Should().NotBeNull();
            body.Name.Should().Be("hey");
            var getResponse = await client.GetAsync($"/garages/{body.Id}");
            getResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task InsertGarageThrowsErrorOnEmptyName()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });

            var request = new
            {
                Body = new GarageUpsertInput
                {
                    Name = string.Empty
                }
            };
            var createResponse = await client.PostAsync("/garages", ContentHelper.GetStringContent(request.Body));
            createResponse.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task InsertGarageThrowsErrorOnGiganticName()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });

            var request = new
            {
                Body = new GarageUpsertInput
                {
                    Name = new string('c', 10001)
                }
            };
            var createResponse = await client.PostAsync("/garages", ContentHelper.GetStringContent(request.Body));
            createResponse.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task UpdateGaragesReturns404NonExisting()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });

            var request = new
            {
                Body = new GarageUpsertInput
                {
                    Name = "hey"
                }
            };
            var patchResponse = await client.PatchAsync("/garages/1", ContentHelper.GetStringContent(request.Body));
            patchResponse.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task UpdateGaragesReturnsAnUpdatedResult()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) =>
            {
                db.Garages.Add(new Garage() {Id = 1, Name = "abcdef"});
            });
            var request = new
            {
                Body = new GarageUpsertInput
                {
                    Name = "hey"
                }
            };
            var patchResponse = await client.PatchAsync("/garages/1", ContentHelper.GetStringContent(request.Body));
            patchResponse.EnsureSuccessStatusCode();
            var body = JsonConvert.DeserializeObject<GarageWebOutput>(await patchResponse.Content.ReadAsStringAsync());
            body.Should().NotBeNull();
            body.Name.Should().Be("hey");
            var getResponse = await client.GetAsync($"/garages/{body.Id}");
            getResponse.EnsureSuccessStatusCode();
            Snapshot.Match(getResponse.Content.ReadAsStringAsync(), new SnapshotNameExtension("_Content"));
            Snapshot.Match(getResponse, new SnapshotNameExtension("_Full"));
        }
    }
}
