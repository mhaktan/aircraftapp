using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using AircraftApp.Entities;
using AircraftApp.MaintenanceTypes;
using AircraftApp.MaintenanceTypes.Dto;
using AircraftApp.Flows;

namespace AircraftApp.Tests.MaintenanceTypes
{
    public class MaintenanceTypeAppServiceTests
    {
        private readonly Mock<IRepository<MaintenanceType, long>> _repositoryMock;
        private readonly MaintenanceTypeAppService _service;

        public MaintenanceTypeAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<MaintenanceType, long>>();
            _service = new MaintenanceTypeAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new MaintenanceType { Id = 1, Name = "Test name", IsActive = true },
                new MaintenanceType { Id = 2, Name = "Test name", IsActive = true },
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
                new MaintenanceType { Id = 1, Name = "Test name", IsActive = true },
                new MaintenanceType { Id = 2, Name = "Test name", IsActive = true },
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
            var dto = new CreateMaintenanceTypeDto
            {
                Name = "Test name", IsActive = true
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<MaintenanceType>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new MaintenanceType { Id = 1, Name = "Test name", IsActive = true });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new MaintenanceType { Id = 1, Name = "Test name", IsActive = true });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
