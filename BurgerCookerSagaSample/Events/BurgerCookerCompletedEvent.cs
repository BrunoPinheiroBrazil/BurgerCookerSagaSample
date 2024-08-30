namespace BurgerCookerSagaSample.Events
{
  public record BurgerCookerCompletedEvent
  {
    public Guid CorrelationId { get; init; }
    public string? CustomerName { get; set; }
    public string? CookTemp { get; set; }
  }
}
