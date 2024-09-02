namespace BurgerCookerSagaSample.StateMachine
{
  using BurgerCookerSagaSample.Contracts;
  using BurgerCookerSagaSample.Events;
  using MassTransit;

  public class BurgerCookerStateMachine :
    MassTransitStateMachine<BurgerCookerState>
  {
    public BurgerCookerStateMachine()
    {
      InstanceState(x => x.CurrentState, Ordered);

      Event(() => BurgerCookerOrderedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
      Event(() => BurgerCookerBeginCookingEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
      Event(() => BurgerCookerFinishedCookingEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
      Event(() => BurgerCookerCompletedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

      Initially(
        When(BurgerCookerOrderedEvent)
          .Then(context =>
          {
            Console.WriteLine($"Cooker Ordering Event Started for: CorrelationID: {context.Message.CorrelationId} - Cooking temp: {context.Message.CookTemp} - Client Name: {context.Message.CustomerName}");
            context.Saga.CustomerName = context.Message.CustomerName;
            context.Saga.CookTemp = context.Message.CookTemp;
            
            if(context.TryGetPayload(out SagaConsumeContext<BurgerCookerState>? payload))
            {
              context.Saga.ResponseAddress = payload?.ResponseAddress;
              context.Saga.RequestId = payload?.RequestId;
            };

            var burgerCookerBeginCookingEvent = new BurgerCookerBeginCookingEvent
            {
              CorrelationId = context.Message.CorrelationId,
              CustomerName = context.Message.CustomerName,
              CookTemp = context.Message.CookTemp
            };

            context.Publish(burgerCookerBeginCookingEvent);
          })
          .TransitionTo(BeginCooking)
      );

      During(BeginCooking,
        When(BurgerCookerBeginCookingEvent)
          .Then(context =>
          {
            Console.WriteLine($"Burger Cooker Begin Cooking event Started for: CorrelationID: {context.Message.CorrelationId} - Cooking temp: {context.Message.CookTemp} - Client Name: {context.Message.CustomerName}");
            var cookBurger = new CookBurger
            {
              CookTemp = context.Message.CookTemp,
              CorrelationId = context.Message.CorrelationId,
              CustomerName = context.Message.CustomerName
            };
            context.Publish(cookBurger);
          })
      .TransitionTo(FinishedCooking)
      );

      During(FinishedCooking,
        When(BurgerCookerFinishedCookingEvent)
          .Then(async context =>
          {
            Console.WriteLine($"Order Up for: CorrelationID: {context.Saga.CorrelationId} - Client Name: {context.Saga.CustomerName} - Cook Temp: {context.Saga.CookTemp} ");
            var burgerCookerCompletedEvent = new BurgerCookerCompletedEvent
            {
              CorrelationId = context.Message.CorrelationId,
              CustomerName = context.Message.CustomerName,
              CookTemp = context.Message.CookTemp
            };

            var responseAddress = await context.GetSendEndpoint(context.Saga.ResponseAddress);

            await responseAddress.Send(burgerCookerCompletedEvent, c => c.RequestId = context.Saga.RequestId);
          })
      .TransitionTo(Completed)
      .Finalize());

      SetCompletedWhenFinalized();
    }

    public State Ordered { get; private set; }
    public State BeginCooking { get; private set; }
    public State FinishedCooking { get; private set; }
    public State Completed { get; private set; }

    public Event<BurgerCookerOrderedEvent> BurgerCookerOrderedEvent { get; private set; }
    public Event<BurgerCookerBeginCookingEvent> BurgerCookerBeginCookingEvent { get; private set; }
    public Event<BurgerCookerFinishedCookingEvent> BurgerCookerFinishedCookingEvent { get; private set; }
    public Event<BurgerCookerCompletedEvent> BurgerCookerCompletedEvent { get; private set; }
  }
}