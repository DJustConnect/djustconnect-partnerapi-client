using DjustConnect.PartnerAPI.Client;
using System;
using Xunit;

namespace DjustConnect.PartnerAPI.ClientTests
{
    public class ConsumerClientTests // Optimaliseer dmv fixtures zodat je niet teveel code herhaalt voor elke test
    {
        [Fact]
        public void GetFarmStatus_ReturnsCorrectPaging()
        {
            // Arrange
            string thumbprint = "E7A8C44F41EA5B0A62422C2E431F4D8B90EC208B";
            string subscriptionkey = "41d959b9f179424faa0c6f5a97b21c56";
            string farmNumberFilter = "0262172489";
            var client = new ConsumerClient(thumbprint, subscriptionkey); // beide te vinden online (ACC)
            client.BaseUrl = "https://partnerapi.acc.djustconnect.cegeka.com";

            // Act
            var okResult = client.GetFarmStatusAsync(farmNumberFilter).Result;
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
            string thumbprint = "E7A8C44F41EA5B0A62422C2E431F4D8B90EC208B";
            string subscriptionkey = "41d959b9f179424faa0c6f5a97b21c56";
            string farmNumberFilter = "0262172489";
            var client = new ConsumerClient(thumbprint, subscriptionkey); // beide te vinden online (ACC)
            client.BaseUrl = "https://partnerapi.acc.djustconnect.cegeka.com";

            // Act
            var okResult = client.GetFarmStatusAsync(farmNumberFilter).Result;
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
            string thumbprint = "E7A8C44F41EA5B0A62422C2E431F4D8B90EC208B";
            string subscriptionkey = "41d959b9f179424faa0c6f5a97b21c56";
            string farmNumberFilter = "0262172489";
            var client = new ConsumerClient(thumbprint, subscriptionkey); // beide te vinden online (ACC)
            client.BaseUrl = "https://partnerapi.acc.djustconnect.cegeka.com";

            // Act
            var okResult = client.GetDarStatusAsync(farmNumberFilter).Result;
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
            string thumbprint = "E7A8C44F41EA5B0A62422C2E431F4D8B90EC208B";
            string subscriptionkey = "41d959b9f179424faa0c6f5a97b21c56";
            string farmNumberFilter = "0123";
            var client = new ConsumerClient(thumbprint, subscriptionkey); // beide te vinden online (ACC)
            client.BaseUrl = "https://partnerapi.acc.djustconnect.cegeka.com";

            // Act
            var okResult = client.GetDarStatusAsync(farmNumberFilter).Result;
            var expectedFarmNumber = "0123";
            var expectedResourceStatus = "Approved";
            var expectedDarStatus = "NotApplicable";

            // Assert
            Assert.Equal(okResult.Result[0].FarmNumber, expectedFarmNumber);
            Assert.Equal(okResult.Result[0].ResourceStatus, expectedResourceStatus);
            Assert.Equal(okResult.Result[0].DarStatus, expectedDarStatus);
        }
        [Fact]
        public void GetResourceHealthStatus_ReturnsCorrectPaging()
        {
            // Arrange
            string thumbprint = "E7A8C44F41EA5B0A62422C2E431F4D8B90EC208B";
            string subscriptionkey = "41d959b9f179424faa0c6f5a97b21c56";
            string resourceId = "4fbfa55c-e188-4e35-4089-08d8c2981168"; //c12b085e-92e5-47c6-635a-08d85fc84f9f ceb56085-b760-4d8b-635b-08d85fc84f9f
            var client = new ConsumerClient(thumbprint, subscriptionkey); // beide te vinden online (ACC)
            client.BaseUrl = "https://partnerapi.acc.djustconnect.cegeka.com";

            // Act
            var okResult = client.GetResourceHealthAsync(resourceId).Result;
            var expectedPageNumber = 1;
            var expectedTotalCount = 1;
            var expectedPageSize = 10;


            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(okResult.PageNumber, expectedPageNumber);
            Assert.Equal(okResult.TotalCount, expectedTotalCount);
            Assert.Equal(okResult.PageSize, expectedPageSize);
        }

    }
}
