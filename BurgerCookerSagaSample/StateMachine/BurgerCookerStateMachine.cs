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

			Initially(
				When(BurgerCookerOrderedEvent)
					.Then(context =>
					{
						context.Saga.CustomerName = context.Message.CustomerName;
						context.Saga.CookTemp = context.Message.CookTemp;

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
						var cookBurger = new CookBurger
						{
							CookTemp = context.Message.CookTemp,
							CorrelationId = context.Message.CorrelationId
						};
						context.Publish(cookBurger);
					})
			.TransitionTo(FinishedCooking)
			);

			During(FinishedCooking,
				When(BurgerCookerFinishedCookingEvent)
					.Then(context =>
					{
						Console.WriteLine($"Order Up for: {context.Saga.CustomerName} Cook Temp: {context.Saga.CookTemp} ");
					})
			.TransitionTo(Ordered)
			);

			SetCompletedWhenFinalized();
		}

		public State Ordered { get; private set; }
		public State BeginCooking { get; private set; }
		public State FinishedCooking { get; private set; }
		public State Completed { get; private set; }

		public Event<BurgerCookerOrderedEvent> BurgerCookerOrderedEvent { get; private set; }
		public Event<BurgerCookerBeginCookingEvent> BurgerCookerBeginCookingEvent { get; private set; }
		public Event<BurgerCookerFinishedCookingEvent> BurgerCookerFinishedCookingEvent { get; private set; }
	}
}