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
            var client = new ConsumerClient(thumbprint, subscriptionkey); // beide te vinden online (ACC) tijdelijk hard coded
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
            var client = new ConsumerClient(thumbprint, subscriptionkey); // beide te vinden online (ACC) tijdelijk hard coded
            client.BaseUrl = "https://partnerapi.acc.djustconnect.cegeka.com";

            // Act
            var okResult = client.GetFarmStatusAsync(farmNumberFilter).Result;
            var expectedStatus = "HasUser";
            var expectedFarmNumber = "0262172489";

            // Assert
            Assert.Equal(okResult.Result[0].Status, expectedStatus);
            Assert.Equal(okResult.Result[0].FarmNumber, expectedFarmNumber);
        }
    }
}
