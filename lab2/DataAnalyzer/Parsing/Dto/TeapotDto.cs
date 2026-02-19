using CsvHelper.Configuration;
using DataAnalyzer.Entities;

namespace DataAnalyzer.Parsing.Dto;

public class TeapotDto
{
  public bool AIHelper { get; set; }
  public double? Weight { get; set; }
  public bool IsNew { get; set; } = false;
  public double? Width { get; set; }
  public double? Length { get; set; }
  public double? Height { get; set; }
  public WarrantyType Warranty { get; set; }
  public bool WarrantyProvidedBySeller { get; set; }
  public double? PowerCordLength { get; set; }
  public bool TeaBrewing { get; set; }
  public bool WaterFillingWithoutOpening { get; set; }
  public bool SoundSignalOnBoil { get; set; }
  public bool IndicationTemperature { get; set; } = false;
  public bool IndicationWaterLevel { get; set; } = false;
  public bool IndicationSwitchingOn { get; set; } = false;
  public double? HeatingModesCount { get; set; }
  public double? WallsCount { get; set; }
  public double? MaxVolume { get; set; }
  public BodyMaterialType BodyMaterial { get; set; }
  public bool AutoShutOffNoWater { get; set; }
  public bool AutoShutOffOnLift { get; set; }
  public bool DelayedStart { get; set; }
  public CountryType Country { get; set; }
  public Function Functions { get; set; }
}

public class TeapotMap : ClassMap<TeapotDto>
{
  public TeapotMap()
  {
    Map(x => x.AIHelper);
    Map(x => x.Weight);
    Map(x => x.IsNew);
    Map(x => x.Width);
    Map(x => x.Length);
    Map(x => x.Height);
    Map(x => x.Warranty).Convert(row =>
    {
        var field = row.Row.GetField("Warranty");

        if (string.IsNullOrWhiteSpace(field))
            return WarrantyType.No;

        return Enum.TryParse<WarrantyType>(field.Trim(), ignoreCase: true, out var result)
            ? result
            : WarrantyType.No;
    });
    Map(x => x.WarrantyProvidedBySeller);
    Map(x => x.PowerCordLength);
    Map(x => x.TeaBrewing);
    Map(x => x.WaterFillingWithoutOpening);
    Map(x => x.SoundSignalOnBoil);
    Map(x => x.IndicationTemperature);
    Map(x => x.IndicationWaterLevel);
    Map(x => x.IndicationSwitchingOn);
    Map(x => x.HeatingModesCount);
    Map(x => x.WallsCount);
    Map(x => x.MaxVolume);

    Map(x => x.BodyMaterial).Convert(row =>
    {
      var s = row.Row.GetField("BodyMaterial");
      if (string.IsNullOrWhiteSpace(s))
        return BodyMaterialType.None;

      return s.Split(',', StringSplitOptions.RemoveEmptyEntries)
              .Select(v => Enum.Parse<BodyMaterialType>(v.Trim()))
              .Aggregate(BodyMaterialType.None, (a, b) => a | b);
    });

    Map(x => x.AutoShutOffNoWater);
    Map(x => x.AutoShutOffOnLift);
    Map(x => x.DelayedStart);
    Map(x => x.Country);

    Map(x => x.Functions).Convert(row =>
    {
      var s = row.Row.GetField("Functions");
      if (string.IsNullOrWhiteSpace(s))
        return Function.None;

      return s.Split(',', StringSplitOptions.RemoveEmptyEntries)
              .Select(v => Enum.Parse<Function>(v.Trim()))
              .Aggregate(Function.None, (a, b) => a | b);
    });
  }
}
