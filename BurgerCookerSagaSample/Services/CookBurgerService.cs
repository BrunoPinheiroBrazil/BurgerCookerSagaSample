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
    private readonly IBus _bus;
    public CookBurgerService(IBus bus)
    {
      _bus = bus;
    }

    public async Task SendCookBurgerMessage(CookBurger cookBurger)
    {
      var burgerBeginCookingEvent = new BurgerCookerOrderedEvent
      {
        CorrelationId = cookBurger.CorrelationId,
        CookTemp = cookBurger.CookTemp,
        CustomerName = cookBurger.CustomerName
      };

      await _bus.Publish(burgerBeginCookingEvent);
      Console.WriteLine("CookBurger Message Sent");
    }
  }
}
