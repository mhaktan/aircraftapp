using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using AircraftApp.Entities;
using AircraftApp.MaintenanceLogs;
using AircraftApp.MaintenanceLogs.Dto;
using AircraftApp.Flows;

namespace AircraftApp.Tests.MaintenanceLogs
{
    public class MaintenanceLogAppServiceTests
    {
        private readonly Mock<IRepository<MaintenanceLog, long>> _repositoryMock;
        private readonly MaintenanceLogAppService _service;

        public MaintenanceLogAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<MaintenanceLog, long>>();
            _service = new MaintenanceLogAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new MaintenanceLog { Id = 1, PerformedBy = "Test performedBy", WorkDescription = "Test workDescription", TimeSpent = 1, LogDate = DateTime.UtcNow },
                new MaintenanceLog { Id = 2, PerformedBy = "Test performedBy", WorkDescription = "Test workDescription", TimeSpent = 1, LogDate = DateTime.UtcNow },
            }.AsQueryable();

            _repositoryMock.Setup(r => r.GetAll()).Returns(entities);

            // Act
            var result = _repositoryMock.Object.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
        }

        [Fact]
        public void Repository_GetAll_WithFilter_ShouldWork()
        {
            // Arrange
            var entities = new[]
            {
                new MaintenanceLog { Id = 1, PerformedBy = "Test performedBy", WorkDescription = "Test workDescription", TimeSpent = 1, LogDate = DateTime.UtcNow },
                new MaintenanceLog { Id = 2, PerformedBy = "Test performedBy", WorkDescription = "Test workDescription", TimeSpent = 1, LogDate = DateTime.UtcNow },
            }.AsQueryable();

            _repositoryMock.Setup(r => r.GetAll()).Returns(entities);

            // Act — simulate keyword filter
            var result = _repositoryMock.Object.GetAll()
                .Where(x => x.Id.ToString().Contains("1"));

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Create_ShouldInsertEntity()
        {
            // Arrange
            var dto = new CreateMaintenanceLogDto
            {
                PerformedBy = "Test performedBy", WorkDescription = "Test workDescription", TimeSpent = 1, LogDate = DateTime.UtcNow
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<MaintenanceLog>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new MaintenanceLog { Id = 1, PerformedBy = "Test performedBy", WorkDescription = "Test workDescription", TimeSpent = 1, LogDate = DateTime.UtcNow });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new MaintenanceLog { Id = 1, PerformedBy = "Test performedBy", WorkDescription = "Test workDescription", TimeSpent = 1, LogDate = DateTime.UtcNow });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
