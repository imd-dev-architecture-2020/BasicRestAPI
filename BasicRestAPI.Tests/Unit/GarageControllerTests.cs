using System;
using System.Collections.Generic;
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
        public void TestGetAllGarages()
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
            _garageRepoMock.Setup(x => x.GetAllGarages()).Returns(returnSet).Verifiable();

            // Act
            var garageResponse = _garageController.GetAllGarages();

            // Assert
            garageResponse.Should().BeOfType<OkObjectResult>();

            // verify via a snapshot (https://swisslife-oss.github.io/snapshooter/)
            // used a lot in jest (for JS)
            Snapshot.Match(garageResponse);
        }

     
        [Fact]
        public void TestGetOneGarageHappyPath()
        {
            var garage = new Garage()
            {
                Id = 1,
                Name = "12"
            };
            _garageRepoMock.Setup(x => x.GetOneGarageById(1)).Returns(garage).Verifiable();
            var garageResponse = _garageController.GarageById(1);
            garageResponse.Should().BeOfType<OkObjectResult>();
            Snapshot.Match(garageResponse);
        }   

        [Fact]
        public void TestGetOneGarageNotFound()
        {
            _garageRepoMock.Setup(x => x.GetOneGarageById(1)).Returns(null as Garage).Verifiable();
            var garageResponse = _garageController.GarageById(1);
            garageResponse.Should().BeOfType<NotFoundResult>();
            Snapshot.Match(garageResponse);
        }

        [Fact]
        public void TestInsertOneGarage()
        {
            var garage = new Garage()
            {
                Id = 1,
                Name = "abcdef"
            };            
            _garageRepoMock.Setup(x => x.Insert("abcdef")).Returns(garage).Verifiable();
            var garageResponse = _garageController.CreateGarage(new GarageUpsertInput()
            {
                Name = "abcdef"
            });
            garageResponse.Should().BeOfType<CreatedResult>();
            Snapshot.Match(garageResponse);
        }

        [Fact]
        public void TestUpdateOneGarageHappyPath()
        {
            var garage = new Garage()
            {
                Id = 1,
                Name = "ghijkl"
            };            
            _garageRepoMock.Setup(x => x.Update(1, "ghijkl")).Returns(garage).Verifiable();
            var garageResponse = _garageController.UpdateGarage(1, new GarageUpsertInput()
            {
                Name = "ghijkl"
            });
            garageResponse.Should().BeOfType<AcceptedResult>();
            Snapshot.Match(garageResponse);
        }

        [Fact]
        public void TestUpdateOneGarageNotFound()
        {
   
            _garageRepoMock
                .Setup(x => x.Update(1, "ghijkl"))
                .Throws<NotFoundException>()
                .Verifiable();
            var garageResponse = _garageController.UpdateGarage(1, new GarageUpsertInput()
            {
                Name = "ghijkl"
            });
            garageResponse.Should().BeOfType<NotFoundResult>();
            Snapshot.Match(garageResponse);
        }
    }
}
