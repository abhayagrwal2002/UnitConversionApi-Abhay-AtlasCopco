namespace UnitConversion.Api.Services;

public interface IConversionService
{
    ConversionResult Convert(string category, string fromUnit, string toUnit, decimal value);
}
