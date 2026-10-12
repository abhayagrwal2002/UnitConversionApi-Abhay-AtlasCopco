namespace UnitConversion.Api.Services;

public sealed record ConversionResult(
    decimal InputValue,
    string FromUnit,
    string ToUnit,
    string Category,
    decimal Result);
