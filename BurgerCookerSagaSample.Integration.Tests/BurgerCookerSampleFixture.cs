using BurgerCookerSagaSample.Consumers;
using BurgerCookerSagaSample.StateMachine;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace BurgerCookerSagaSample.Integration.Tests
{
  public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
  {
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      builder.ConfigureServices(services =>
      {
        services.AddMassTransitTestHarness();
        
      });
      builder.UseEnvironment("Development");
    }
  }
  public class BurgerCookerSampleFixture
  {
    private readonly CustomWebApplicationFactory<Program> _factory;
    public HttpClient httpClient;
    public readonly ITestHarness _harness;

    public BurgerCookerSampleFixture()
    {
      _factory = new CustomWebApplicationFactory<Program>();
      httpClient = _factory.WithWebHostBuilder(cfg => cfg.UseSolutionRelativeContentRoot("./")).CreateClient();
    }
  }
}
