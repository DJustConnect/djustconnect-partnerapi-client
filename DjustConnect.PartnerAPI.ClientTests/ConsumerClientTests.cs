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
            string thumbprint = "323856cd8ab9e1890b9d654ea609b38849a54ca7"; //"E7A8C44F41EA5B0A62422C2E431F4D8B90EC208B";
            // DjustConnect subscriptionkey
            string subscriptionkey = "41d959b9f179424faa0c6f5a97b21c56";
            var client = new ConsumerClient(thumbprint, subscriptionkey); // beide te vinden online (ACC)
            client.BaseUrl = "https://partnerapi.acc.djustconnect.cegeka.com";
            return client;
        }

        [Fact]
        public async Task GetFarmStatus_ReturnsCorrectPaging()
        {
            // Arrange
            string farmNumberFilter = "0262172489";

            // Act
            var okResult = await _client.GetFarmStatusAsync(farmNumberFilter);
            var expectedPageNumber = 1;
            var expectedTotalCount = 1;
            var expectedPageSize = 10;

            // Assert
            AssertPagedResult(okResult);
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
        public async Task GetFarmStatusWithSorting_ReturnsSortedData()
        {
            var filter = new FarmStatusFilter { Sort = FarmStatusSort.FarmNumber };
            var result = await _client.GetFarmStatusAsync(filter);
            AssertPagedResult(result);
            AssertSorted(result.Result.Select(d => d.FarmNumber), SortDirection.Ascending);
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
            Assert.True(okResult.TotalCount > 0);
            Assert.Equal(expectedPageSize, okResult.PageSize);
        }

        [Fact]
        public async Task GetDarStatus_ReturnsCorrectResultContent()
        {
            // Arrange
            string farmNumberFilter = "0123";

            // Act
            var okResult = (await _client.GetDarStatusAsync(farmNumberFilter))?.Result?.FirstOrDefault();
            var expectedFarmNumber = "0123";
            var expectedResourceStatus = "Approved";
            var expectedDarStatus = "NotApplicable";

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(expectedFarmNumber, okResult.FarmNumber);
            Assert.Equal(expectedResourceStatus, okResult.ResourceStatus);
            Assert.Equal(expectedDarStatus, okResult.DarStatus);
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
            Assert.True(okResult.Length > 0);
        }

        [Fact]
        public void GetFarmIdTypes_ReturnsNotNull()
        {
            // Arrange

            // Act
            var okResult = _client.GetFarmIdTypesAsync().Result;

            // Assert
            Assert.NotNull(okResult);
            Assert.True(okResult.Length > 0);
        }

        [Fact]
        public async Task GetRarStatusWithFilter_ReturnsNotNull()
        {
            // Arrange
            var filter = new RarStatusFilter { ResourceName = "prov-2-test" };

            // Act
            var okResult = await _client.GetRarStatusAsync(filter);

            // Assert
            AssertPagedResult(okResult);
        }

        [Fact]
        public async Task GetRarStatusWithSorting_ReturnsSortedData()
        {
            var filter = new RarStatusFilter { Sort = RarStatusSort.ApiName };
            var result = await _client.GetRarStatusAsync(filter);
            AssertPagedResult(result);
            AssertSorted(result.Result.Select(d => d.ApiName), SortDirection.Ascending);

            filter = new RarStatusFilter { Sort = RarStatusSort.ResourceName, SortDirection = SortDirection.Descending };
            result = await _client.GetRarStatusAsync(filter);
            AssertPagedResult(result);
            AssertSorted(result.Result.Select(d => d.ResourceName), SortDirection.Descending);
        }

        [Fact]
        public async Task GetDarStatusWithFilter_ReturnsCorrectResultContent()
        {
            // Arrange
            var filter = new DarStatusFilter { ResourceName = "prov-2-test" };

            // Act
            var result = await _client.GetDarStatusAsync(filter);
            var firstResult = result?.Result.FirstOrDefault();
            var expectedFarmNumber = "0123";
            var expectedResourceStatus = "Approved";
            var expectedDarStatus = "NotApplicable";

            // Assert
            AssertPagedResult(result);
            Assert.NotNull(firstResult);
            Assert.Equal(expectedFarmNumber, firstResult?.FarmNumber);
            Assert.Equal(expectedResourceStatus, firstResult?.ResourceStatus);
            Assert.Equal(expectedDarStatus, firstResult?.DarStatus);
        }

        [Fact]
        public async Task GetDarStatusWithSorting_ReturnsSortedData()
        {
            var filter = new DarStatusFilter { Sort = DarStatusSort.FarmNumber };
            var result = await _client.GetDarStatusAsync(filter);
            AssertPagedResult(result);
            AssertSorted(result.Result.Select(d => d.FarmNumber), SortDirection.Ascending);

            filter = new DarStatusFilter { Sort = DarStatusSort.ResourceName, SortDirection = SortDirection.Descending };
            result = await _client.GetDarStatusAsync(filter);
            AssertPagedResult(result);
            AssertSorted(result.Result.Select(d => d.ResourceName), SortDirection.Descending);
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
        public async void FarmMappingTest()
        {
            var farmmappings = await _client.GetFarmMappingAsync(new string[] { "0262172489" }, new string[] { "4c17a3f2-c03d-4d65-8440-3a896b245753", "324a23eb-b4bc-4de1-a01b-0e478afac252", "dd03e71c-d114-4cce-a5fe-6843f1fc8878", "d55fe787-6ea0-46e8-9f00-d9e5e86bad2b" }, "4c17a3f2-c03d-4d65-8440-3a896b245753");
            //These asserts depend heavily on the current settings in DjustConnect. When adding (or removing) 
            //mappings, the order of farmmappings might possibly and the count will definitely change
            Assert.Equal("4402300237", farmmappings.ToArray()[0].FarmMappings.ToArray()[1].Value);
            Assert.True(farmmappings.ToArray()[0].FarmMappings.Count() == 2);
        }
        [Fact]
        public async void FarmerTest()
        {
            var farms = await _client.GetFarmsAsync("720FCBFB-2AEC-42E8-82AA-DCAEBD2DC563");
            Assert.Contains<string>("0262172489", farms);
        }

        [Fact]
        public async void GetPushNotificationDetails_ReturnsResult()
        {
            var result = await _client.GetPushNotificationsEndpointAsync();
            Assert.NotNull(result);
        }

        [Fact]
        public async void DeactivatePushNotificationEndpointTest()
        {
            // Possible StatusCodes (200 = ok, 403 not a consumer (forbidden))
            // Toggle between activate and deactivate then check status with GetPushNotificationDetails
            await _client.DeactivatePushNotificationsEndpointAsync("https://samples.djustconnect.be/Consumercsharp/Events");
            var result = await _client.GetPushNotificationsEndpointAsync();
            Assert.Null(result.Endpoint);
            Assert.Null(result.SubscriptionStatus);
            Assert.False(result.ReceiveNotifications);
        }

        [Fact]
        public async void ActivatePushNotificationEndpointTest()
        {   
            // Possible StatusCodes (200 = ok, 400 = activation endpoint not valid (bad request), 403 not a consumer (forbidden))
            // Toggle between activate and deactivate then check status with GetPushNotificationDetails
            await _client.ActivatePushNotificationsEndpointAsync("https://samples.djustconnect.be/Consumercsharp/Events");
            var result = await _client.GetPushNotificationsEndpointAsync();
            Assert.Contains("samples.djustconnect.be/Consumercsharp/Events", result.Endpoint);
            Assert.NotNull(result.SubscriptionStatus);
            Assert.True(result.ReceiveNotifications);
        }

        private void AssertPagedResult<T>(PagedResult<T> pagedResult)
        {
            Assert.NotNull(pagedResult);
            Assert.True(pagedResult.PageSize > 0);
            Assert.True(pagedResult.TotalCount > 0);
            Assert.True(pagedResult.Pages > 0);
            Assert.Equal(1, pagedResult.PageNumber);
        }

        private void AssertSorted<T>(IEnumerable<T> list, SortDirection direction) where T : IComparable
        {
            T previous = list.First();
            list = list.Skip(1);
            foreach (var current in list)
            {
                switch (direction)
                {
                    case SortDirection.Ascending:
                        Assert.True(previous.CompareTo(current) <= 0);
                        break;
                    case SortDirection.Descending:
                        Assert.True(previous.CompareTo(current) >= 0);
                        break;
                }
                previous = current;
            }
        }
    }
}
