using System;
using Xunit;
using FluentAssertions;
using AircraftApp.Entities;

namespace AircraftApp.Tests.MaintenanceLogs
{
    public class MaintenanceLogEntityTests
    {
        [Fact]
        public void MaintenanceLog_ShouldBeCreatable()
        {
            // Act
            var entity = new MaintenanceLog();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void MaintenanceLog_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new MaintenanceLog();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void MaintenanceLog_PerformedBy_ShouldAcceptValue()
        {
            var entity = new MaintenanceLog { PerformedBy = "Test Value" };
            entity.PerformedBy.Should().Be("Test Value");
        }

        [Fact]
        public void MaintenanceLog_WorkDescription_ShouldAcceptValue()
        {
            var entity = new MaintenanceLog { WorkDescription = "Test Value" };
            entity.WorkDescription.Should().Be("Test Value");
        }

    }
}
