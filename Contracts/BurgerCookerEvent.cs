namespace Contracts
{
    using System;

    public record BurgerCookerEvent
    {
        public Guid CorrelationId { get; init; }
        public string Value { get; init; }
    }
}