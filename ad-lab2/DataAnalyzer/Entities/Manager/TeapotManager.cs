using DataAnalyzer.DataAnalyzer.Utils;
using DataAnalyzer.Dto;
using DataAnalyzer.Parsing.Dto;

namespace DataAnalyzer.Entities.Manager;

public static class TeapotManager
{
  public static List<Teapot> Prepare(this ICollection<TeapotDto> teapotDtos)
  {
    var normalizedTeapots = teapotDtos.Select(o => new Teapot
    {
      AIHelper = o.AIHelper,
      Weight = o.Weight ?? 0,
      IsNew = o.IsNew,
      Width = o.Width ?? 0,
      Length = o.Length ?? 0,
      Height = o.Height ?? 0,
      Warranty = o.Warranty,
      WarrantyProvidedBySeller = o.WarrantyProvidedBySeller,
      PowerCordLength = o.PowerCordLength ?? 0,
      TeaBrewing = o.TeaBrewing,
      WaterFillingWithoutOpening = o.WaterFillingWithoutOpening,
      SoundSignalOnBoil = o.SoundSignalOnBoil,
      IndicationTemperature = o.IndicationTemperature,
      IndicationWaterLevel = o.IndicationWaterLevel,
      IndicationSwitchingOn = o.IndicationSwitchingOn,
      HeatingModesCount = o.HeatingModesCount ?? 0,
      WallsCount = o.WallsCount ?? 0,
      MaxVolume = o.MaxVolume ?? 0,
      BodyMaterial = o.BodyMaterial,
      AutoShutOffNoWater = o.AutoShutOffNoWater,
      AutoShutOffOnLift = o.AutoShutOffOnLift,
      DelayedStart = o.DelayedStart,
      Country = o.Country,
      Functions = o.Functions
    }).ToList();

    var doubleProperties = typeof(Teapot).GetProperties()
        .Where(p => p.PropertyType == typeof(double))
        .ToList();

    foreach (var prop in doubleProperties)
    {
      var values = normalizedTeapots.Select(o => (double)prop.GetValue(o)!).ToList();
      var normalizer = new DaNormalizer(values);

      foreach (var teapot in normalizedTeapots)
      {
        var value = (double)prop.GetValue(teapot)!;
        prop.SetValue(teapot, normalizer.NormalizedValueOf(value));
      }
    }

    return normalizedTeapots;
  }
}