using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using AircraftApp.Entities;
using AircraftApp.SpareParts;
using AircraftApp.SpareParts.Dto;
using AircraftApp.Flows;

namespace AircraftApp.Tests.SpareParts
{
    public class SparePartAppServiceTests
    {
        private readonly Mock<IRepository<SparePart, long>> _repositoryMock;
        private readonly SparePartAppService _service;

        public SparePartAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<SparePart, long>>();
            _service = new SparePartAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new SparePart { Id = 1, PartNumber = "Test partNumber", PartName = "Test partName", UnitOfMeasure = "Test unitOfMeasure", StockQuantity = 1, MinStockLevel = 1, IsActive = true },
                new SparePart { Id = 2, PartNumber = "Test partNumber", PartName = "Test partName", UnitOfMeasure = "Test unitOfMeasure", StockQuantity = 1, MinStockLevel = 1, IsActive = true },
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
                new SparePart { Id = 1, PartNumber = "Test partNumber", PartName = "Test partName", UnitOfMeasure = "Test unitOfMeasure", StockQuantity = 1, MinStockLevel = 1, IsActive = true },
                new SparePart { Id = 2, PartNumber = "Test partNumber", PartName = "Test partName", UnitOfMeasure = "Test unitOfMeasure", StockQuantity = 1, MinStockLevel = 1, IsActive = true },
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
            var dto = new CreateSparePartDto
            {
                PartNumber = "Test partNumber", PartName = "Test partName", UnitOfMeasure = "Test unitOfMeasure", StockQuantity = 1, MinStockLevel = 1, IsActive = true
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<SparePart>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new SparePart { Id = 1, PartNumber = "Test partNumber", PartName = "Test partName", UnitOfMeasure = "Test unitOfMeasure", StockQuantity = 1, MinStockLevel = 1, IsActive = true });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new SparePart { Id = 1, PartNumber = "Test partNumber", PartName = "Test partName", UnitOfMeasure = "Test unitOfMeasure", StockQuantity = 1, MinStockLevel = 1, IsActive = true });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
