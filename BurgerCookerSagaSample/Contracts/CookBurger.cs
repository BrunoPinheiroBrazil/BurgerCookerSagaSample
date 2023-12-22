namespace BurgerCookerSagaSample.Contracts
{
	public record CookBurger
	{
		public Guid CorrelationId { get; init; }
		public string? CookTemp { get; init; }
		public string? CustomerName { get; init; }
	}
}
