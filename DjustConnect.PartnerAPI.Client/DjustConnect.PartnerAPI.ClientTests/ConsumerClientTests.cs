using DjustConnect.PartnerAPI.Client;
using DjustConnect.PartnerAPI.Client.Filters;
using System;
using System.Threading;
using Xunit;

namespace DjustConnect.PartnerAPI.ClientTests
{
    public class ConsumerClientTests 
    {
        // GetFarmIdTypes
        // GetResources
        // GetResourceHealth
        // GetRarStatus
        // GetDarStatus
        // GetFarmStatus
        private ConsumerClient BuildClient()
        {
            string thumbprint = "E7A8C44F41EA5B0A62422C2E431F4D8B90EC208B";
            string subscriptionkey = "41d959b9f179424faa0c6f5a97b21c56";
            var client = new ConsumerClient(thumbprint, subscriptionkey); // beide te vinden online (ACC)
            client.BaseUrl = "https://partnerapi.acc.djustconnect.cegeka.com";
            return client;
        }

        [Fact]
        public void GetFarmStatus_ReturnsCorrectPaging()
        {
            // Arrange
            string farmNumberFilter = "0262172489";
           
            // Act
            var okResult = BuildClient().GetFarmStatusAsync(farmNumberFilter).Result;
            var expectedPageNumber = 1;
            var expectedTotalCount = 1;
            var expectedPageSize = 10;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(okResult.PageNumber, expectedPageNumber);
            Assert.Equal(okResult.TotalCount, expectedTotalCount);
            Assert.Equal(okResult.PageSize, expectedPageSize);
        }
        [Fact]
        public void GetFarmStatus_ReturnsCorrectResultContent()
        {
            // Arrange
            string farmNumberFilter = "0262172489";

            // Act
            var okResult = BuildClient().GetFarmStatusAsync(farmNumberFilter).Result;
            var expectedStatus = "HasUser";
            var expectedFarmNumber = "0262172489";

            // Assert
            Assert.Equal(okResult.Result[0].Status, expectedStatus);
            Assert.Equal(okResult.Result[0].FarmNumber, expectedFarmNumber);
        }

        [Fact]
        public void GetDarStatus_ReturnsCorrectPaging()
        {
            // Arrange
            string farmNumberFilter = "0262172489";

            // Act
            var okResult = BuildClient().GetDarStatusAsync(farmNumberFilter).Result;
            var expectedPageNumber = 1;
            var expectedTotalCount = 1;
            var expectedPageSize = 100;


            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(okResult.PageNumber, expectedPageNumber);
            Assert.Equal(okResult.TotalCount, expectedTotalCount);
            Assert.Equal(okResult.PageSize, expectedPageSize);
        }
        [Fact]
        public void GetDarStatus_ReturnsCorrectResultContent()
        {
            // Arrange
            string farmNumberFilter = "0123";

            // Act
            var okResult = BuildClient().GetDarStatusAsync(farmNumberFilter).Result;
            var expectedFarmNumber = "0123";
            var expectedResourceStatus = "Approved";
            var expectedDarStatus = "NotApplicable";

            // Assert
            Assert.Equal(okResult.Result[0].FarmNumber, expectedFarmNumber);
            Assert.Equal(okResult.Result[0].ResourceStatus, expectedResourceStatus);
            Assert.Equal(okResult.Result[0].DarStatus, expectedDarStatus);
        }
        [Fact]
        public void GetResourceHealthStatus_ReturnsNotNull()
        {
            // Arrange
            Guid? resourceId = Guid.Parse("4fbfa55c-e188-4e35-4089-08d8c2981168");

            // Act
            var okResult = BuildClient().GetResourceHealthAsync(resourceId).Result;

            // Assert
            Assert.NotNull(okResult);
        }

        [Fact]
        public void GetResourceHealthStatus_ReturnsCorrectResult()
        {
            // Arrange
            Guid? resourceId = Guid.Parse("4fbfa55c-e188-4e35-4089-08d8c2981168");

            // Act
            var okResult = BuildClient().GetResourceHealthAsync(resourceId).Result;
            var expectedResourceHealth = ResourceHealth.OK;
            var expectedResourceName = "prov-2-test";

            // Assert
            Assert.Equal(expectedResourceHealth, okResult[0].ResourceHealth);
            Assert.Equal(expectedResourceName, okResult[0].ResourceName);
        }

        [Fact]
        public void GetResources_ReturnsNotNull()
        {
            // Arrange

            // Act
            var okResult = BuildClient().GetResourcesAsync().Result;

            // Assert
            Assert.NotNull(okResult);
        }

        [Fact]
        public void GetFarmIdTypes_ReturnsNotNull()
        {
            // Arrange

            // Act
            var okResult = BuildClient().GetFarmIdTypesAsync().Result;

            // Assert
            Assert.NotNull(okResult);
        }
        [Fact]
        public void GetRarStatus_ReturnsNotNull()
        {
            // Arrange
            RarStatusFilter filter = new RarStatusFilter();
            filter.ResourceName = "prov-2-test";

            // Act
            var okResult = BuildClient().GetRarStatusAsync(filter, CancellationToken.None).Result;

            // Assert
            Assert.NotNull(okResult);
        }
    }
}
