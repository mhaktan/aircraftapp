using System;
using Xunit;
using FluentAssertions;
using AircraftApp.Entities;

namespace AircraftApp.Tests.SpareParts
{
    public class SparePartEntityTests
    {
        [Fact]
        public void SparePart_ShouldBeCreatable()
        {
            // Act
            var entity = new SparePart();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void SparePart_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new SparePart();

            // Assert
            entity.Id.Should().Be(default(long));
            entity.IsActive.Should().Be(false);
        }

        [Fact]
        public void SparePart_PartNumber_ShouldAcceptValue()
        {
            var entity = new SparePart { PartNumber = "Test Value" };
            entity.PartNumber.Should().Be("Test Value");
        }

        [Fact]
        public void SparePart_PartName_ShouldAcceptValue()
        {
            var entity = new SparePart { PartName = "Test Value" };
            entity.PartName.Should().Be("Test Value");
        }

        [Fact]
        public void SparePart_UnitOfMeasure_ShouldAcceptValue()
        {
            var entity = new SparePart { UnitOfMeasure = "Test Value" };
            entity.UnitOfMeasure.Should().Be("Test Value");
        }

    }
}
