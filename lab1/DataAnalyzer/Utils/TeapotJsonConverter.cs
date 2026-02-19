using System.Text.Json;
using System.Text.Json.Serialization;
using DataAnalyzer.Entities;

namespace DataAnalyzer.Utils;

public class WarranityConverter : JsonConverter<Warranity>
{
  public override Warranity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    throw new NotImplementedException();
  }

  public override void Write(Utf8JsonWriter writer, Warranity value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.ToString());
  }
}

public class BodyMaterialConverter : JsonConverter<BodyMaterial>
{
  public override BodyMaterial Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    throw new NotImplementedException();
  }

  public override void Write(Utf8JsonWriter writer, BodyMaterial value, JsonSerializerOptions options)
  {
    var materials = new List<string>();

    if (value.HasFlag(BodyMaterial.Plastic)) materials.Add("пластик");
    if (value.HasFlag(BodyMaterial.Steel)) materials.Add("сталь");
    if (value.HasFlag(BodyMaterial.Glass)) materials.Add("стекло");
    if (value.HasFlag(BodyMaterial.Metal)) materials.Add("металл");
    if (value.HasFlag(BodyMaterial.Ceramic)) materials.Add("керамика");
    if (value.HasFlag(BodyMaterial.Silicone)) materials.Add("силикон");
    if (value.HasFlag(BodyMaterial.Aluminum)) materials.Add("алюминий");

    JsonSerializer.Serialize(writer, materials, options);
  }
}

public class FunctionConverter : JsonConverter<Function>
{
  public override Function Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    throw new NotImplementedException();
  }

  public override void Write(Utf8JsonWriter writer, Function value, JsonSerializerOptions options)
  {
    var functions = new List<string>();

    if (value.HasFlag(Function.NoiseReduction)) functions.Add("шумоподавление");
    if (value.HasFlag(Function.TemperatureMaintenance)) functions.Add("поддержание температуры");
    if (value.HasFlag(Function.TeaBrewing)) functions.Add("заваривание чая");

    JsonSerializer.Serialize(writer, functions, options);
  }
}

public class CountryConverter : JsonConverter<Country>
{
  public override Country Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    throw new NotImplementedException();
  }

  public override void Write(Utf8JsonWriter writer, Country value, JsonSerializerOptions options)
  {
    var countryNames = new Dictionary<Country, string>
    {
      [Country.China] = "Китай",
      [Country.Russia] = "Россия",
      [Country.Turkey] = "Турция",
      [Country.Usa] = "США",
      [Country.Germany] = "Германия"
    };

    writer.WriteStringValue(countryNames[value]);
  }
}

public class TeapotJsonConverter
{
  public static string ConvertToJson(List<Teapot> teapots)
  {
    var header = new dynamic[]
    {
      new { feature_name = "AIHelper", type = "boolean" },
      new { feature_name = "Weight", type = "numeric" },
      new { feature_name = "IsNew", type = "boolean" },
      new { feature_name = "Width", type = "numeric" },
      new { feature_name = "Length", type = "numeric" },
      new { feature_name = "Height", type = "numeric" },
      new {
        feature_name = "Warranty",
        type = "category",
        values = new[] { "No", "HalfYear", "OneYear", "OneAndHalfYear", "TwoYears", "ThreeYears" }
      },
      new { feature_name = "WarrantyProvidedBySeller", type = "boolean" },
      new { feature_name = "PowerCordLength", type = "text" },
      new { feature_name = "TeaBrewing", type = "boolean" },
      new { feature_name = "WaterFillingWithoutOpening", type = "boolean" },
      new { feature_name = "SoundSignalOnBoil", type = "boolean" },
      new { feature_name = "IndicationTemperature", type = "boolean" },
      new { feature_name = "IndicationWaterLevel", type = "boolean" },
      new { feature_name = "IndicationSwitchingOn", type = "boolean" },
      new { feature_name = "HeatingModesCount", type = "integer" },
      new { feature_name = "WallsCount", type = "integer" },
      new { feature_name = "MaxVolume", type = "numeric" },
      new {
        feature_name = "BodyMaterial",
        type = "multicategory",
        values = new[] { "пластик", "сталь", "стекло", "металл", "керамика", "силикон", "алюминий" }
      },
      new { feature_name = "AutoShutOffNoWater", type = "boolean" },
      new { feature_name = "AutoShutOffOnLift", type = "boolean" },
      new { feature_name = "DelayedStart", type = "boolean" },
      new {
        feature_name = "Country",
        type = "category",
        values = new[] { "Китай", "Россия", "Турция", "США", "Германия" }
      },
      new {
        feature_name = "Functions",
        type = "multicategory",
        values = new[] { "шумоподавление", "поддержание температуры", "заваривание чая" }
      }
    };

    var data = teapots.Select(teapot => new
    {
      teapot.AIHelper,
      teapot.Weight,
      teapot.IsNew,
      teapot.Width,
      teapot.Length,
      teapot.Height,
      Warranty = teapot.Warranty.ToString(),
      teapot.WarrantyProvidedBySeller,
      teapot.PowerCordLength,
      teapot.TeaBrewing,
      teapot.WaterFillingWithoutOpening,
      teapot.SoundSignalOnBoil,
      teapot.IndicationTemperature,
      teapot.IndicationWaterLevel,
      teapot.IndicationSwitchingOn,
      teapot.HeatingModesCount,
      teapot.WallsCount,
      teapot.MaxVolume,
      teapot.BodyMaterial,
      teapot.AutoShutOffNoWater,
      teapot.AutoShutOffOnLift,
      teapot.DelayedStart,
      Country = teapot.Country?.ToString(),
      teapot.Functions
    }).ToList();

    var result = new
    {
      header,
      data
    };

    var options = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true,
      Converters =
      {
          new WarranityConverter(),
          new BodyMaterialConverter(),
          new FunctionConverter(),
          new CountryConverter()
      }
    };

    return JsonSerializer.Serialize(result, options);
  }
}