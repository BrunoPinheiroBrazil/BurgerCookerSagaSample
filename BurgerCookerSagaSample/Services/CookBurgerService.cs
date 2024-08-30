using BurgerCookerSagaSample.Contracts;
using BurgerCookerSagaSample.Events;
using MassTransit;

namespace BurgerCookerSagaSample.Services
{
  public interface ICookBurgerService
  {
    Task SendCookBurgerMessage(CookBurger cookBurger);
  }
  public class CookBurgerService : ICookBurgerService
  {
    private readonly IRequestClient<BurgerCookerOrderedEvent> _client;
    public CookBurgerService(IRequestClient<BurgerCookerOrderedEvent> client)
    {
      _client = client;
    }

    public async Task SendCookBurgerMessage(CookBurger cookBurger)
    {
      var burgerBeginCookingEvent = new BurgerCookerOrderedEvent
      {
        CorrelationId = cookBurger.CorrelationId,
        CookTemp = cookBurger.CookTemp,
        CustomerName = cookBurger.CustomerName
      };

      Console.WriteLine("CookBurger Message Sent");
      await _client.GetResponse<BurgerCookerCompletedEvent>(burgerBeginCookingEvent);
    }
  }
}
