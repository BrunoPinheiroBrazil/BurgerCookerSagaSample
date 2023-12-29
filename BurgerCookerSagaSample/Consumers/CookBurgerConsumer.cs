using BurgerCookerSagaSample.Contracts;
using BurgerCookerSagaSample.Events;
using MassTransit;

namespace BurgerCookerSagaSample.Consumers
{
  public class CookBurgerConsumer : IConsumer<CookBurger>
  {
    public Task Consume(ConsumeContext<CookBurger> context)
    {
      if (context.Message.CookTemp == null)
        throw new ArgumentNullException(nameof(context.Message.CookTemp));

      Console.WriteLine($"Cooking temp {context.Message.CookTemp}");

      if (context.Message.CookTemp.Equals("RARE", StringComparison.InvariantCultureIgnoreCase))
      {
        Thread.Sleep(1000);
      }

      if (context.Message.CookTemp.Equals("MED", StringComparison.InvariantCultureIgnoreCase))
      {
        Thread.Sleep(2000);
      }

      if (context.Message.CookTemp.Equals("BURNED", StringComparison.InvariantCultureIgnoreCase))
      {
        Thread.Sleep(3000);
      }

      Console.WriteLine($"Cooking done!");

      context.Publish(new BurgerCookerFinishedCookingEvent
      {
        CorrelationId = context.Message.CorrelationId,
        CookTemp = context.Message.CookTemp
      });

      return Task.CompletedTask;
    }
  }
}
