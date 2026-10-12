namespace UnitConversion.Api.Models;

public sealed class ConversionResponse
{
    public decimal InputValue { get; init; }

    public string FromUnit { get; init; } = string.Empty;

    public string ToUnit { get; init; } = string.Empty;

    public string Category { get; init; } = string.Empty;

    public decimal Result { get; init; }
}
