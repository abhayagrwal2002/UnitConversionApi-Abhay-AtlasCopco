using UnitConversion.Api.Services;

namespace UnitConversion.Api.Tests;

public sealed class ConversionServiceTests
{
    private readonly ConversionService _service = new();

    [Fact]
    public void Convert_LengthFromMetersToFeet_ReturnsExpectedValue()
    {
        var result = _service.Convert("length", "meters", "feet", 10m);

        Assert.Equal(32.808399m, result.Result);
        Assert.Equal("length", result.Category);
    }

    [Fact]
    public void Convert_MassFromKilogramsToPounds_ReturnsExpectedValue()
    {
        var result = _service.Convert("mass", "kilograms", "pounds", 5m);

        Assert.Equal(11.023113m, result.Result);
    }

    [Fact]
    public void Convert_WeightAlias_UsesMassConversions()
    {
        var result = _service.Convert("weight", "pounds", "kilograms", 10m);

        Assert.Equal(4.535924m, result.Result);
        Assert.Equal("mass", result.Category);
    }

    [Fact]
    public void Convert_TemperatureFromCelsiusToFahrenheit_ReturnsExpectedValue()
    {
        var result = _service.Convert("temperature", "celsius", "fahrenheit", 100m);

        Assert.Equal(212m, result.Result);
    }

    [Fact]
    public void Convert_TemperatureFromKelvinToCelsius_ReturnsExpectedValue()
    {
        var result = _service.Convert("temperature", "kelvin", "celsius", 273.15m);

        Assert.Equal(0m, result.Result);
    }

    [Fact]
    public void Convert_UnsupportedCategory_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            _service.Convert("volume", "liters", "gallons", 1m));

        Assert.Contains("Unsupported category", exception.Message);
    }

    [Fact]
    public void Convert_UnsupportedUnit_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            _service.Convert("length", "meters", "kilograms", 1m));

        Assert.Contains("Unsupported target unit", exception.Message);
    }
}
