namespace Company.StateMachines
{
    using Contracts;
    using MassTransit;

    public class BurgerCookerStateMachine :
        MassTransitStateMachine<BurgerCookerState> 
    {
        public BurgerCookerStateMachine()
        {
            InstanceState(x => x.CurrentState, Created);

            Event(() => BurgerCookerEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

            Initially(
                When(BurgerCookerEvent)
                    .Then(context => context.Instance.Value = context.Data.Value)
                    .TransitionTo(Created)
            );

            SetCompletedWhenFinalized();
        }

        public State Created { get; private set; }

        public Event<BurgerCookerEvent> BurgerCookerEvent { get; private set; }
    }
}