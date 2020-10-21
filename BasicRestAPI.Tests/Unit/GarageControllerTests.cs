using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicRestAPI.Controllers;
using BasicRestAPI.Model;
using BasicRestAPI.Model.Domain;
using BasicRestAPI.Model.Web;
using BasicRestAPI.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Snapshooter.Xunit;
using Xunit;

namespace BasicRestAPI.Tests.Unit
{
    public class GarageControllerTests : IDisposable
    {
        // Mocking, the concept: https://stackoverflow.com/questions/2665812/what-is-mocking
        // Mocking, the library: https://github.com/Moq/moq4/wiki/Quickstart
        private readonly Mock<ILogger<GarageController>> _loggerMock;
        private readonly Mock<IGarageRepository> _garageRepoMock;
        private readonly GarageController _garageController;

        public GarageControllerTests()
        {
            // In our tests we choose to ignore whatever logging is being done. We still need to mock it to avoid 
            // null reference exceptions; loose mocks just handle whatever you throw at them.
            _loggerMock = new Mock<ILogger<GarageController>>(MockBehavior.Loose);
            _garageRepoMock = new Mock<IGarageRepository>(MockBehavior.Strict);
            _garageController = new GarageController(_garageRepoMock.Object, _loggerMock.Object);
        }
        public void Dispose()
        {
            _loggerMock.VerifyAll();
            _garageRepoMock.VerifyAll();

            _loggerMock.Reset();
            _garageRepoMock.Reset();
        }


        [Fact]
        public async Task TestGetAllGarages()
        {
            var returnSet = new[]
            {
                new Garage
                {
                    Cars = new List<Car>(200),
                    Id = 1,
                    Name = "test garage 1"
                },
                new Garage
                {
                    Cars = new List<Car>(200),
                    Id = 2,
                    Name = "test garage 2"
                },
                new Garage
                {
                    Cars = new List<Car>(200),
                    Id = 3,
                    Name = "test garage 3"
                },
            };
            // Arrange
            _garageRepoMock.Setup(x => x.GetAllGarages()).Returns(Task.FromResult((IEnumerable<Garage>)returnSet)).Verifiable();

            // Act
            var garageResponse = await _garageController.GetAllGarages();

            // Assert
            garageResponse.Should().BeOfType<OkObjectResult>();

            // verify via a snapshot (https://swisslife-oss.github.io/snapshooter/)
            // used a lot in jest (for JS)
            Snapshot.Match(garageResponse);
        }

     
        [Fact]
        public async Task TestGetOneGarageHappyPath()
        {
            var garage = new Garage()
            {
                Id = 1,
                Name = "12"
            };
            _garageRepoMock.Setup(x => x.GetOneGarageById(1)).Returns(Task.FromResult(garage)).Verifiable();
            var garageResponse = await _garageController.GarageById(1);
            garageResponse.Should().BeOfType<OkObjectResult>();
            Snapshot.Match(garageResponse);
        }   

        [Fact]
        public async Task TestGetOneGarageNotFound()
        {
            _garageRepoMock.Setup(x => x.GetOneGarageById(1)).Returns(Task.FromResult(null as Garage)).Verifiable();
            var garageResponse = await _garageController.GarageById(1);
            garageResponse.Should().BeOfType<NotFoundResult>();
            Snapshot.Match(garageResponse);
        }

        [Fact]
        public async Task TestInsertOneGarage()
        {
            var garage = new Garage()
            {
                Id = 1,
                Name = "abcdef"
            };            
            _garageRepoMock.Setup(x => x.Insert("abcdef")).Returns(Task.FromResult(garage)).Verifiable();
            var garageResponse = await _garageController.CreateGarage(new GarageUpsertInput()
            {
                Name = "abcdef"
            });
            garageResponse.Should().BeOfType<CreatedResult>();
            Snapshot.Match(garageResponse);
        }

        [Fact]
        public async Task TestUpdateOneGarageHappyPath()
        {
            var garage = new Garage()
            {
                Id = 1,
                Name = "ghijkl"
            };            
            _garageRepoMock.Setup(x => x.Update(1, "ghijkl")).Returns(Task.FromResult(garage)).Verifiable();
            var garageResponse = await _garageController.UpdateGarage(1, new GarageUpsertInput()
            {
                Name = "ghijkl"
            });
            garageResponse.Should().BeOfType<AcceptedResult>();
            Snapshot.Match(garageResponse);
        }

        [Fact]
        public async Task TestUpdateOneGarageNotFound()
        {
   
            _garageRepoMock
                .Setup(x => x.Update(1, "ghijkl"))
                .Throws<NotFoundException>()
                .Verifiable();
            var garageResponse = await _garageController.UpdateGarage(1, new GarageUpsertInput()
            {
                Name = "ghijkl"
            });
            garageResponse.Should().BeOfType<NotFoundResult>();
            Snapshot.Match(garageResponse);
        }
    }
}
