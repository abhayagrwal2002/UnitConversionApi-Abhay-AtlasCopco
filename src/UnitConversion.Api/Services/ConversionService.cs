namespace UnitConversion.Api.Services;

public sealed class ConversionService : IConversionService
{
    private static readonly Dictionary<string, UnitCategory> Categories = new(StringComparer.OrdinalIgnoreCase)
    {
        ["length"] = new UnitCategory(
            "length",
            new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
            {
                ["meter"] = 1m,
                ["meters"] = 1m,
                ["kilometer"] = 1000m,
                ["kilometers"] = 1000m,
                ["centimeter"] = 0.01m,
                ["centimeters"] = 0.01m,
                ["millimeter"] = 0.001m,
                ["millimeters"] = 0.001m,
                ["foot"] = 0.3048m,
                ["feet"] = 0.3048m,
                ["inch"] = 0.0254m,
                ["inches"] = 0.0254m,
                ["mile"] = 1609.344m,
                ["miles"] = 1609.344m
            }),

        ["mass"] = new UnitCategory(
            "mass",
            new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
            {
                ["kilogram"] = 1m,
                ["kilograms"] = 1m,
                ["gram"] = 0.001m,
                ["grams"] = 0.001m,
                ["milligram"] = 0.000001m,
                ["milligrams"] = 0.000001m,
                ["pound"] = 0.45359237m,
                ["pounds"] = 0.45359237m,
                ["ounce"] = 0.028349523125m,
                ["ounces"] = 0.028349523125m
            }),

        ["weight"] = new UnitCategory(
            "mass",
            new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
            {
                ["kilogram"] = 1m,
                ["kilograms"] = 1m,
                ["gram"] = 0.001m,
                ["grams"] = 0.001m,
                ["milligram"] = 0.000001m,
                ["milligrams"] = 0.000001m,
                ["pound"] = 0.45359237m,
                ["pounds"] = 0.45359237m,
                ["ounce"] = 0.028349523125m,
                ["ounces"] = 0.028349523125m
            })
    };

    private static readonly Dictionary<string, TemperatureUnit> TemperatureUnits = new(StringComparer.OrdinalIgnoreCase)
    {
        ["celsius"] = TemperatureUnit.Celsius,
        ["fahrenheit"] = TemperatureUnit.Fahrenheit,
        ["kelvin"] = TemperatureUnit.Kelvin
    };

    public ConversionResult Convert(string category, string fromUnit, string toUnit, decimal value)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException("Category is required.", nameof(category));
        }

        if (string.IsNullOrWhiteSpace(fromUnit))
        {
            throw new ArgumentException("From unit is required.", nameof(fromUnit));
        }

        if (string.IsNullOrWhiteSpace(toUnit))
        {
            throw new ArgumentException("To unit is required.", nameof(toUnit));
        }

        var normalizedCategory = category.Trim();
        var normalizedFromUnit = fromUnit.Trim();
        var normalizedToUnit = toUnit.Trim();

        if (normalizedCategory.Equals("temperature", StringComparison.OrdinalIgnoreCase))
        {
            return ConvertTemperature(normalizedFromUnit, normalizedToUnit, value);
        }

        if (!Categories.TryGetValue(normalizedCategory, out var unitCategory))
        {
            throw new ArgumentException($"Unsupported category '{category}'.", nameof(category));
        }

        if (!unitCategory.Units.TryGetValue(normalizedFromUnit, out var fromFactor))
        {
            throw new ArgumentException($"Unsupported source unit '{fromUnit}' for category '{category}'.", nameof(fromUnit));
        }

        if (!unitCategory.Units.TryGetValue(normalizedToUnit, out var toFactor))
        {
            throw new ArgumentException($"Unsupported target unit '{toUnit}' for category '{category}'.", nameof(toUnit));
        }


        if (value < 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"The value for {unitCategory.Name} cannot be less than 0.");
        }

        var baseValue = value * fromFactor;
        var result = baseValue / toFactor;

        return new ConversionResult(
            value,
            normalizedFromUnit,
            normalizedToUnit,
            unitCategory.Name,
            decimal.Round(result, 6));
    }

    private static ConversionResult ConvertTemperature(string fromUnit, string toUnit, decimal value)
    {
        if (!TemperatureUnits.TryGetValue(fromUnit, out var fromTemperatureUnit))
        {
            throw new ArgumentException($"Unsupported source unit '{fromUnit}' for category 'temperature'.", nameof(fromUnit));
        }

        if (!TemperatureUnits.TryGetValue(toUnit, out var toTemperatureUnit))
        {
            throw new ArgumentException($"Unsupported target unit '{toUnit}' for category 'temperature'.", nameof(toUnit));
        }

        if ((fromTemperatureUnit == TemperatureUnit.Kelvin && value < 0m) || (fromTemperatureUnit == TemperatureUnit.Celsius && value < -273.15m) ||(fromTemperatureUnit == TemperatureUnit.Fahrenheit && value < -459.67m))
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"The temperature value is below absolute zero (-273.15°C / -459.67°F / 0K).");
        }

        var celsius = ToCelsius(value, fromTemperatureUnit);
        var result = FromCelsius(celsius, toTemperatureUnit);

        return new ConversionResult(
            value,
            fromUnit,
            toUnit,
            "temperature",
            decimal.Round(result, 6));
    }

    private static decimal ToCelsius(decimal value, TemperatureUnit unit)
    {
        return unit switch
        {
            TemperatureUnit.Celsius => value,
            TemperatureUnit.Fahrenheit => (value - 32m) * 5m / 9m,
            TemperatureUnit.Kelvin => value - 273.15m,
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
    }

    private static decimal FromCelsius(decimal value, TemperatureUnit unit)
    {
        return unit switch
        {
            TemperatureUnit.Celsius => value,
            TemperatureUnit.Fahrenheit => value * 9m / 5m + 32m,
            TemperatureUnit.Kelvin => value + 273.15m,
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
    }

    private sealed record UnitCategory(string Name, IReadOnlyDictionary<string, decimal> Units);

    private enum TemperatureUnit
    {
        Celsius,
        Fahrenheit,
        Kelvin
    }
}
