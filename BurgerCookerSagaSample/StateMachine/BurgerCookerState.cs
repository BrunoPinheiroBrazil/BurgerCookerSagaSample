namespace BurgerCookerSagaSample.StateMachine
{
  using MassTransit;
  using System;

  public class BurgerCookerState :
    SagaStateMachineInstance
  {
    public int CurrentState { get; set; }
    public string? CustomerName { get; set; }
    public string? CookTemp { get; set; }
    public Guid CorrelationId { get; set; }
  }
}