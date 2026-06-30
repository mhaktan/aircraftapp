using System;
using Xunit;
using FluentAssertions;
using AircraftApp.Entities;

namespace AircraftApp.Tests.MaintenanceTypes
{
    public class MaintenanceTypeEntityTests
    {
        [Fact]
        public void MaintenanceType_ShouldBeCreatable()
        {
            // Act
            var entity = new MaintenanceType();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void MaintenanceType_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new MaintenanceType();

            // Assert
            entity.Id.Should().Be(default(long));
            entity.IsActive.Should().Be(false);
        }

        [Fact]
        public void MaintenanceType_Name_ShouldAcceptValue()
        {
            var entity = new MaintenanceType { Name = "Test Value" };
            entity.Name.Should().Be("Test Value");
        }

    }
}
