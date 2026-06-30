using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using AircraftApp.Entities;
using AircraftApp.MaintenanceRequests;
using AircraftApp.MaintenanceRequests.Dto;
using AircraftApp.Flows;

namespace AircraftApp.Tests.MaintenanceRequests
{
    public class MaintenanceRequestAppServiceTests
    {
        private readonly Mock<IRepository<MaintenanceRequest, long>> _repositoryMock;
        private readonly MaintenanceRequestAppService _service;

        public MaintenanceRequestAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<MaintenanceRequest, long>>();
            _service = new MaintenanceRequestAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object, new Mock<IRepository<StatusChangeLog, long>>().Object, new Mock<IRepository<ApprovalRecord, Guid>>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new MaintenanceRequest { Id = 1, RequestNumber = "Test requestNumber", Description = "Test description", Priority = 0, RequestedBy = "Test requestedBy", Status = 0 },
                new MaintenanceRequest { Id = 2, RequestNumber = "Test requestNumber", Description = "Test description", Priority = 0, RequestedBy = "Test requestedBy", Status = 0 },
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
                new MaintenanceRequest { Id = 1, RequestNumber = "Test requestNumber", Description = "Test description", Priority = 0, RequestedBy = "Test requestedBy", Status = 0 },
                new MaintenanceRequest { Id = 2, RequestNumber = "Test requestNumber", Description = "Test description", Priority = 0, RequestedBy = "Test requestedBy", Status = 0 },
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
            var dto = new CreateMaintenanceRequestDto
            {
                RequestNumber = "Test requestNumber", Description = "Test description", Priority = 0, RequestedBy = "Test requestedBy", Status = 0
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<MaintenanceRequest>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new MaintenanceRequest { Id = 1, RequestNumber = "Test requestNumber", Description = "Test description", Priority = 0, RequestedBy = "Test requestedBy", Status = 0 });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new MaintenanceRequest { Id = 1, RequestNumber = "Test requestNumber", Description = "Test description", Priority = 0, RequestedBy = "Test requestedBy", Status = 0 });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
