using System;
using Xunit;
using FluentAssertions;
using AircraftApp.Entities;

namespace AircraftApp.Tests.Aircrafts
{
    public class AircraftEntityTests
    {
        [Fact]
        public void Aircraft_ShouldBeCreatable()
        {
            // Act
            var entity = new Aircraft();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void Aircraft_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new Aircraft();

            // Assert
            entity.Id.Should().Be(default(long));
            entity.IsActive.Should().Be(false);
        }

        [Fact]
        public void Aircraft_RegistrationNumber_ShouldAcceptValue()
        {
            var entity = new Aircraft { RegistrationNumber = "Test Value" };
            entity.RegistrationNumber.Should().Be("Test Value");
        }

        [Fact]
        public void Aircraft_AircraftType_ShouldAcceptValue()
        {
            var entity = new Aircraft { AircraftType = "Test Value" };
            entity.AircraftType.Should().Be("Test Value");
        }

        [Fact]
        public void Aircraft_Manufacturer_ShouldAcceptValue()
        {
            var entity = new Aircraft { Manufacturer = "Test Value" };
            entity.Manufacturer.Should().Be("Test Value");
        }

        [Fact]
        public void Aircraft_Model_ShouldAcceptValue()
        {
            var entity = new Aircraft { Model = "Test Value" };
            entity.Model.Should().Be("Test Value");
        }

        [Fact]
        public void Aircraft_SerialNumber_ShouldAcceptValue()
        {
            var entity = new Aircraft { SerialNumber = "Test Value" };
            entity.SerialNumber.Should().Be("Test Value");
        }

    }
}
