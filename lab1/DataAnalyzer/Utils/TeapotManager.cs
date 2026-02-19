using System.Globalization;
using DataAnalyzer.Dto;
using DataAnalyzer.Entities;

namespace DataAnalyzer.Utils;

public static class TeapotManager
{
  public static Teapot CreateTeapotFromDto(TeapotDto rawTeapot)
  {
    Teapot teapot = new();
    var teapotProperties = typeof(Teapot).GetProperties();
    var rawProperties = typeof(TeapotDto).GetProperties();

    foreach (var teapotProp in teapotProperties)
    {
      var rawProp = rawProperties.FirstOrDefault(p => p.Name == teapotProp.Name);

      if (rawProp != null && teapotProp.PropertyType == rawProp.PropertyType)
        teapotProp.SetValue(teapot, rawProp.GetValue(rawTeapot));
    }

    teapot.IsNew = rawTeapot.Appearance == "новый";
    string? dimentionsString;
    if (string.IsNullOrEmpty(rawTeapot.Dimensions))
      dimentionsString = rawTeapot.DimensionsAlt;
    else
      dimentionsString = rawTeapot.Dimensions;

    var dims = string.IsNullOrEmpty(dimentionsString) ? null : dimentionsString
      .Replace(" см", "")
      .Split('*')
      .Select(s => string.IsNullOrEmpty(s) ? null : (double?)double.Parse(s, CultureInfo.InvariantCulture))
      .ToArray();
    if (dims != null)
    {
      teapot.Width = dims.ElementAtOrDefault(2);
      teapot.Length = dims.ElementAtOrDefault(1);
      teapot.Height = dims.ElementAtOrDefault(0);
    }
    teapot.Warranty = rawTeapot.Warranty switch
    {
      "1 год" => Warranity.OneYear,
      "6 месяцев" => Warranity.HalfYear,
      "2 года" => Warranity.TwoYears,
      "3 года" => Warranity.ThreeYears,
      "1,5 года" => Warranity.OneAndHalfYear,
      _ => Warranity.No
    };
    teapot.WarrantyProvidedBySeller = rawTeapot.WarrantyProvidedBy switch
    {
      "производителем" => false,
      "продавцом" => true,
      _ => null
    };
    teapot.TeaBrewing = rawTeapot.TeaBrewing == "Да";
    teapot.WaterFillingWithoutOpening = rawTeapot.WaterFillingWithoutOpening == "Да";
    teapot.SoundSignalOnBoil = rawTeapot.SoundSignalOnBoil == "Да";

    var indication = rawTeapot.Indication ?? "";
    teapot.IndicationTemperature = indication.Contains("температуры");
    teapot.IndicationWaterLevel = indication.Contains("уровня воды");
    teapot.IndicationSwitchingOn = indication.Contains("включения");
    teapot.HeatingModesCount = string.IsNullOrEmpty(rawTeapot.HeatingModesCount) ? null : int.Parse(rawTeapot.HeatingModesCount);
    teapot.WallsCount = string.IsNullOrEmpty(rawTeapot.WallsCount) ? null : int.Parse(rawTeapot.WallsCount);
    teapot.MaxVolume = string.IsNullOrEmpty(rawTeapot.MaxVolume) ? null : double.Parse(rawTeapot.MaxVolume, CultureInfo.InvariantCulture);
    teapot.AIHelper = rawTeapot.Alice == "Да" || rawTeapot.Marusya == "Да";
    var material = rawTeapot.BodyMaterial?.ToLower() ?? "";
    teapot.BodyMaterial = BodyMaterial.None;

    if (material.Contains("пластик") || material.Contains("plastic") || material.Contains("полимер") || material.Contains("пвх") || material.Contains("abs"))
      teapot.BodyMaterial |= BodyMaterial.Plastic;
    if (material.Contains("стекло") || material.Contains("glass"))
      teapot.BodyMaterial |= BodyMaterial.Glass;
    if (material.Contains("сталь") || material.Contains("steel"))
      teapot.BodyMaterial |= BodyMaterial.Steel;
    if (material.Contains("металл") || material.Contains("metal"))
      teapot.BodyMaterial |= BodyMaterial.Metal;
    if (material.Contains("алюмин"))
      teapot.BodyMaterial |= BodyMaterial.Aluminum;
    if (material.Contains("керамик") || material.Contains("фарфор"))
      teapot.BodyMaterial |= BodyMaterial.Ceramic;
    if (material.Contains("силикон"))
      teapot.BodyMaterial |= BodyMaterial.Silicone;
    teapot.AutoShutOffNoWater = rawTeapot.AutoShutOffNoWater == "Да";
    teapot.AutoShutOffOnLift = rawTeapot.AutoShutOffOnLift == "Да";
    teapot.DelayedStart = rawTeapot.DelayedStart == "Да";
    teapot.Country = rawTeapot.Country switch
    {
      "Китай" => Country.China,
      "Россия" => Country.Russia,
      "Турция" => Country.Turkey,
      "США" => Country.Usa,
      "Германия" => Country.Germany,
      _ => null
    };
    var func = rawTeapot.Function?.ToLower() ?? "";
    teapot.Functions = Function.None;

    if (func.Contains("снижения уровня шума"))
      teapot.Functions |= Function.NoiseReduction;
    if (func.Contains("поддержание температуры"))
      teapot.Functions |= Function.TemperatureMaintenance;
    if (func.Contains("заваривание чая"))
      teapot.Functions |= Function.TeaBrewing;
    return teapot;
  }

  public static void Normalize(ICollection<Teapot> teapots)
  {
    foreach (var prop in typeof(Teapot).GetProperties().Where(p => p.PropertyType == typeof(double?)))
    {
      var values = teapots.Select(t => prop.GetValue(t))
          .Where(v => v != null)
          .Select(Convert.ToDouble)
          .ToList();

      if (!values.Any()) continue;

      var normalizer = new DaNormalizer(values);

      foreach (var teapot in teapots)
      {
        var value = prop.GetValue(teapot);
        if (value != null)
          prop.SetValue(teapot, normalizer.NormalizedValueOf((double)value));
      }
    }
  }
}