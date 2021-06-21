using DjustConnect.PartnerAPI.Client;
using DjustConnect.PartnerAPI.Client.DTOs;
using DjustConnect.PartnerAPI.Client.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        // ConsumerAccess GET -> POST -> GET testen of de data wel degelijk correct wordt opgeslagen
        // FarmMapping
        private ConsumerClient BuildClient()
        {
            // client cert thumbprint
            string thumbprint = "E7A8C44F41EA5B0A62422C2E431F4D8B90EC208B";
            // DjustConnect subscriptionkey
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
            var expectedPageSize = 100;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(expectedPageNumber, okResult.PageNumber);
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
            string resourceName = "prov-2-test";

            // Act
            var okResult = BuildClient().GetRarStatusAsync(resourceName).Result;

            // Assert
            Assert.NotNull(okResult);
        }
        [Fact]
        public void GetRarStatusWithFilter_ReturnsNotNull()
        {
            // Arrange
            RarStatusFilter filter = new RarStatusFilter();
            filter.ResourceName = "prov-2-test";

            // Act
            var okResult = BuildClient().GetRarStatusAsyncWithFilter(filter, CancellationToken.None).Result;

            // Assert
            Assert.NotNull(okResult);
        }
        [Fact]
        public void GetDarStatusWithFilter_ReturnsNotNull()
        {
            // Arrange
            DarStatusFilter filter = new DarStatusFilter();
            filter.ResourceName = "prov-2-test";

            // Act
            var okResult = BuildClient().GetDarStatusAsyncWithFilter(filter, CancellationToken.None).Result;

            // Assert
            Assert.NotNull(okResult);
        }
        [Fact]
        public void GetFarmStatusWithFilter_ReturnsNotNull()
        {
            // Arrange
            FarmStatusFilter filter = new FarmStatusFilter();
            filter.FarmNumber = "0262172489";

            // Act
            var okResult = BuildClient().GetFarmStatusAsyncWithFilter(filter, CancellationToken.None).Result;

            // Assert
            Assert.NotNull(okResult);
        }
        [Fact]
        public void GetDarStatusWithFilter_ReturnsCorrectResultContent()
        {
            // Arrange
            DarStatusFilter filter = new DarStatusFilter();
            filter.ResourceName = "prov-2-test";

            // Act
            var okResult = BuildClient().GetDarStatusAsyncWithFilter(filter, CancellationToken.None).Result;
            var expectedFarmNumber = "0123";
            var expectedResourceStatus = "Approved";
            var expectedDarStatus = "NotApplicable";

            // Assert
            Assert.Equal(okResult.Result[0].FarmNumber, expectedFarmNumber);
            Assert.Equal(okResult.Result[0].ResourceStatus, expectedResourceStatus);
            Assert.Equal(okResult.Result[0].DarStatus, expectedDarStatus);
        }

        [Fact]
        public async Task GetConsumerAccessAsync_ReturnsCorrectData()
        {
            // Arrange

            //Act
            var okResult = await BuildClient().GetConsumerAccessAsync(CancellationToken.None);
            var expectedFarmIdType = "KBO";
            var expectedFarmId = "0262172489";
            var expectedFarmIdTwo = "0123";
            var expectedResourceName = "prov-2-test";

            //Assert
            Assert.NotNull(okResult);
            Assert.Contains(expectedFarmId, okResult.FarmsIds);
            Assert.Contains(expectedFarmIdTwo, okResult.FarmsIds);
            // Error: FarmIdType name =  PE nummer, not KBO , is this a mistake in DC or in the test?
            Assert.Equal(expectedFarmIdType, okResult.FarmIdType.Name);
            Assert.Contains(expectedResourceName, okResult.Resources.Select(r => r.Name));
        }
        [Fact]
        public async Task GetConsumerAccessAsync_ThenPostConsumerAccess_UpdatesConsumerAccess()
        {

            // Arrange
            var client = BuildClient();
            var newId = "test-ilvo" + Guid.NewGuid();
            // Act
            // Get
            var consumerAccess = await client.GetConsumerAccessAsync();
            var farmIdsCount = consumerAccess.FarmsIds.Count;
            consumerAccess.FarmsIds.Add(newId);
            // Post
            await client.PostConsumerAccessAsync(consumerAccess);
            // Get again
            var okResult = await client.GetConsumerAccessAsync();

            // Assert
            Assert.Contains(newId, okResult.FarmsIds);
            Assert.Equal(okResult.FarmsIds.Count, farmIdsCount + 1);
        }
        [Fact]
        public void FarmMappingTest()
        {
            var client = BuildClient();
            var farmmappings = client.GetFarmMappingAsync(new string[] { "0262172489" }, new string[] { "4c17a3f2-c03d-4d65-8440-3a896b245753", "324a23eb-b4bc-4de1-a01b-0e478afac252", "dd03e71c-d114-4cce-a5fe-6843f1fc8878", "d55fe787-6ea0-46e8-9f00-d9e5e86bad2b" }, "4c17a3f2-c03d-4d65-8440-3a896b245753").GetAwaiter().GetResult();
            //These asserts depend heavily on the current settings in DjustConnect. When adding (or removing) 
            //mappings, the order of farmmappings might possibly and the count will definitely change
            Assert.Equal("4402300237", farmmappings.ToArray()[0].FarmMappings.ToArray()[1].Value);
            Assert.True(farmmappings.ToArray()[0].FarmMappings.Count() == 2);
        }
    }
}
