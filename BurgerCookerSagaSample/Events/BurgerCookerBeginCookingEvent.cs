﻿namespace BurgerCookerSagaSample.Events
{
	public record BurgerCookerBeginCookingEvent
	{
		public Guid CorrelationId { get; init; }
		public string? CustomerName { get; set; } 
		public string? CookTemp { get; set; }
	}
}
