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
        // DISCLAIMER:
        // Asserts in these tests depend heavily on the specific settings in DjustConnect for this specific test-consumer. 
        // specify your own setup when trying to execute the tests and consider them rather as a manual on how to use the 
        // ConsumerClient code

        private readonly ConsumerClient _client;
        // GetFarmIdTypes
        // GetResources
        // GetResourceHealth
        // GetRarStatus
        // GetDarStatus
        // GetFarmStatus
        // ConsumerAccess GET -> POST -> GET testen of de data wel degelijk correct wordt opgeslagen
        // FarmMapping
        // Farmer (Getfarms)
        public ConsumerClientTests()
        {
            _client = BuildClient();
        }
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
            var okResult = _client.GetFarmStatusAsync(farmNumberFilter).Result;
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
            var okResult = _client.GetFarmStatusAsync(farmNumberFilter).Result;
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
            var okResult = _client.GetDarStatusAsync(farmNumberFilter).Result;
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
            var okResult = _client.GetDarStatusAsync(farmNumberFilter).Result;
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
            var okResult = _client.GetResourceHealthAsync(resourceId).Result;

            // Assert
            Assert.NotNull(okResult);
        }

        [Fact]
        public void GetResourceHealthStatus_ReturnsCorrectResult()
        {
            // Arrange
            Guid? resourceId = Guid.Parse("4fbfa55c-e188-4e35-4089-08d8c2981168");

            // Act
            var okResult = _client.GetResourceHealthAsync(resourceId).Result;
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
            var okResult = _client.GetResourcesAsync().Result;

            // Assert
            Assert.NotNull(okResult);
        }

        [Fact]
        public void GetFarmIdTypes_ReturnsNotNull()
        {
            // Arrange

            // Act
            var okResult = _client.GetFarmIdTypesAsync().Result;

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
            var okResult = _client.GetRarStatusAsync(resourceName).Result;

            // Assert
            Assert.NotNull(okResult);
        }
        [Fact]
        public void GetRarStatusWithFilter_ReturnsNotNull()
        {
            // Arrange
            RarStatusFilter filter = new RarStatusFilter
            {
                ResourceName = "prov-2-test",
                PageSize = 2,
                PageNumber = 1
            };

            // Act
            var okResult = _client.GetRarStatusAsyncWithFilter(filter, CancellationToken.None).Result;
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
            var okResult = _client.GetDarStatusAsyncWithFilter(filter, CancellationToken.None).Result;

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
            var okResult = _client.GetFarmStatusAsyncWithFilter(filter, CancellationToken.None).Result;

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
            var okResult = _client.GetDarStatusAsyncWithFilter(filter, CancellationToken.None).Result;
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
            var okResult = await _client.GetConsumerAccessAsync(CancellationToken.None);
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

            var newId = "test-ilvo" + Guid.NewGuid();
            // Act
            // Get
            var consumerAccess = await _client.GetConsumerAccessAsync();
            var farmIdsCount = consumerAccess.FarmsIds.Count;
            consumerAccess.FarmsIds.Add(newId);
            // Post
            await _client.PostConsumerAccessAsync(consumerAccess);
            // Get again
            var okResult = await _client.GetConsumerAccessAsync();

            // Assert
            Assert.Contains(newId, okResult.FarmsIds);
            Assert.Equal(okResult.FarmsIds.Count, farmIdsCount + 1);
        }
        [Fact]
        public async void FarmMapping()
        {
            var farmmappings = await _client.GetFarmMappingAsync(new string[] { "0262172489" }, new string[] { "4c17a3f2-c03d-4d65-8440-3a896b245753", "324a23eb-b4bc-4de1-a01b-0e478afac252", "dd03e71c-d114-4cce-a5fe-6843f1fc8878", "d55fe787-6ea0-46e8-9f00-d9e5e86bad2b" }, "4c17a3f2-c03d-4d65-8440-3a896b245753");
            Assert.Equal("4402300237", farmmappings.ToArray()[0].FarmMappings.ToArray()[1].Value);
            Assert.True(farmmappings.ToArray()[0].FarmMappings.Count() == 2);
        }
        [Fact]
        public async void Farmer()
        {
            var farms = await _client.GetFarmsAsync("720FCBFB-2AEC-42E8-82AA-DCAEBD2DC563");
            Assert.Contains<string>("0262172489", farms);
        }

        [Fact]
        public async void GetPushNotificationDetails()
        {
            // asserts will fail when endpoint is deactivated! 
            var result = await _client.GetPushNotificationsEndpointAsync();
            Assert.Contains("samples.djustconnect.be/Consumercsharp/Events", result.Endpoint);
            Assert.NotNull(result.SubscriptionStatus);
        }

        [Fact]
        public async void DectivatePushNotificationEndpoint()
        {
            // Possible StatusCodes (200 = ok, 403 not a consumer (forbidden))
            // Toggle between activate and deactivate then check status with GetPushNotificationDetails
            var result = await _client.DeactivatePushNotificationsEndpointAsync("https://samples.djustconnect.be/Consumercsharp/Events");
            Assert.Equal("OK", result);
        }
        [Fact]
        public async void ActivatePushNotificationEndpoint()
        {   // Possible StatusCodes (200 = ok, 400 = activation endpoint not valid (bad request), 403 not a consumer (forbidden))
            // Toggle between activate and deactivate then check status with GetPushNotificationDetails
            var result = await _client.ActivatePushNotificationsEndpointAsync("https://samples.djustconnect.be/Consumercsharp/Events");
            Assert.Equal("OK", result);
        }
    }
}
