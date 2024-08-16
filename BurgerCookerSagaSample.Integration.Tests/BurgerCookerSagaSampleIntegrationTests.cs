using BurgerCookerSagaSample.Contracts;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace BurgerCookerSagaSample.Integration.Tests
{
  public class BurgerCookerSagaSampleIntegrationTests : IClassFixture<BurgerCookerSampleFixture>
  {
    private readonly BurgerCookerSampleFixture _fixture;
    private readonly HttpClient _httpClient;

    public BurgerCookerSagaSampleIntegrationTests(BurgerCookerSampleFixture fixture)
    {
      _fixture = fixture;
      _httpClient = _fixture.httpClient;
    }

    [Fact(DisplayName ="Should Process Burger Successfully")]
    public async Task ShouldProcessBurgerSuccessFully()
    {
      //Arrange
      var sagaID = Guid.NewGuid();
      var payload = new CookBurger
      {
        CorrelationId = sagaID,
        CustomerName = "JOSIAS",
        CookTemp = "BURNED"
      };

      var payloadJson = JsonConvert.SerializeObject(payload);
      var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

      //Act
      var response = await _httpClient.PostAsync("/api/burgercooker/cook", content);

      //Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
  }
}
