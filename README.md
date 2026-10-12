# Unit Conversion API

ASP.NET Core Web API for converting numeric values between units of measurement.

The API currently supports:

- Length
- Temperature
- Mass / weight

Unit definitions are stored in memory for this version of the project. The code is structured so those definitions can later move to configuration, a database, or another data source without changing the controller contract.

## Technology

- .NET 8
- ASP.NET Core Web API
- xUnit
- Swagger / OpenAPI

## Project Structure

```text
UnitConversionApi/
  src/
    UnitConversion.Api/
      Controllers/
      Models/
      Services/
      Program.cs

  tests/
    UnitConversion.Api.Tests/
      ConversionServiceTests.cs
```

## API Endpoint

```http
POST /api/conversions
```

Example request:

```json
{
  "category": "length",
  "fromUnit": "meters",
  "toUnit": "feet",
  "value": 10
}
```

Example response:

```json
{
  "inputValue": 10,
  "fromUnit": "meters",
  "toUnit": "feet",
  "category": "length",
  "result": 32.808399
}
```

## Running Locally

From the repository root:

```bash
dotnet restore
dotnet run --project src/UnitConversion.Api
```

Swagger is available in development at:

```text
https://localhost:<port>/swagger
```

The exact port is shown in the terminal when the API starts.

## Running Tests

```bash
dotnet test
```

## Design Decisions

The solution uses a simple service layer pattern. The controller handles HTTP concerns, while `ConversionService` contains the conversion rules.

Most length and mass conversions are handled by converting through a base unit:

- Length uses meters as the base unit.
- Mass uses kilograms as the base unit.

Temperature conversion is handled separately because it needs formulas instead of only multiplication factors.

