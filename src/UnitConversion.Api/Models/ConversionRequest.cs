using System.ComponentModel.DataAnnotations;

namespace UnitConversion.Api.Models;

public sealed class ConversionRequest
{
    [Required]
    public string Category { get; init; } = string.Empty;

    [Required]
    public string FromUnit { get; init; } = string.Empty;

    [Required]
    public string ToUnit { get; init; } = string.Empty;

    public decimal Value { get; init; }
}
