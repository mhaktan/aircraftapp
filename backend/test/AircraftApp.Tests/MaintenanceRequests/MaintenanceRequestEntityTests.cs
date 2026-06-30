using System;
using Xunit;
using FluentAssertions;
using AircraftApp.Entities;

namespace AircraftApp.Tests.MaintenanceRequests
{
    public class MaintenanceRequestEntityTests
    {
        [Fact]
        public void MaintenanceRequest_ShouldBeCreatable()
        {
            // Act
            var entity = new MaintenanceRequest();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void MaintenanceRequest_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new MaintenanceRequest();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void MaintenanceRequest_RequestNumber_ShouldAcceptValue()
        {
            var entity = new MaintenanceRequest { RequestNumber = "Test Value" };
            entity.RequestNumber.Should().Be("Test Value");
        }

        [Fact]
        public void MaintenanceRequest_Description_ShouldAcceptValue()
        {
            var entity = new MaintenanceRequest { Description = "Test Value" };
            entity.Description.Should().Be("Test Value");
        }

        [Fact]
        public void MaintenanceRequest_RequestedBy_ShouldAcceptValue()
        {
            var entity = new MaintenanceRequest { RequestedBy = "Test Value" };
            entity.RequestedBy.Should().Be("Test Value");
        }

    }
}
