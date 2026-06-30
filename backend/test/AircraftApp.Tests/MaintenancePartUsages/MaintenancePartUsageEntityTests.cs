using System;
using Xunit;
using FluentAssertions;
using AircraftApp.Entities;

namespace AircraftApp.Tests.MaintenancePartUsages
{
    public class MaintenancePartUsageEntityTests
    {
        [Fact]
        public void MaintenancePartUsage_ShouldBeCreatable()
        {
            // Act
            var entity = new MaintenancePartUsage();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void MaintenancePartUsage_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new MaintenancePartUsage();

            // Assert
            entity.Id.Should().Be(default(long));

        }


    }
}
