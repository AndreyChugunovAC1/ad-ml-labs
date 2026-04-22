using DataAnalyzer.DataAnalyzer.Interfaces;

namespace DataAnalyzer.Entities;

public class Teapot : IReadyToAnalyzeObject
{
  public bool AIHelper { get; set; }
  public double Weight { get; set; }
  public bool IsNew { get; set; }
  public double Width { get; set; }
  public double Length { get; set; }
  public double Height { get; set; }
  public WarrantyType Warranty { get; set; }
  public bool WarrantyProvidedBySeller { get; set; }
  public double PowerCordLength { get; set; }
  public bool TeaBrewing { get; set; }
  public bool WaterFillingWithoutOpening { get; set; }
  public bool SoundSignalOnBoil { get; set; }
  public bool IndicationTemperature { get; set; }
  public bool IndicationWaterLevel { get; set; }
  public bool IndicationSwitchingOn { get; set; }
  public double HeatingModesCount { get; set; }
  public double WallsCount { get; set; }
  public double MaxVolume { get; set; }
  public BodyMaterialType BodyMaterial { get; set; }
  public bool AutoShutOffNoWater { get; set; }
  public bool AutoShutOffOnLift { get; set; }
  public bool DelayedStart { get; set; }
  public CountryType Country { get; set; }
  public Function Functions { get; set; }

  public bool IsTarget => AIHelper ||
                          TeaBrewing ||
                          WaterFillingWithoutOpening ||
                          DelayedStart ||
                          HeatingModesCount > 1.0;

  public IReadOnlyList<double> Features => [
    Weight,
    Width,
    Length,
    Height,
    MaxVolume,
    WallsCount,
    PowerCordLength,
    // IsTarget ? 1.0 : 0.0,

    IsNew ? 1.0 : 0.0,
    SoundSignalOnBoil ? 1.0 : 0.0,
    IndicationTemperature ? 1.0 : 0.0,
    IndicationWaterLevel ? 1.0 : 0.0,
    IndicationSwitchingOn ? 1.0 : 0.0,
    AutoShutOffNoWater ? 1.0 : 0.0,
    AutoShutOffOnLift ? 1.0 : 0.0,
    WarrantyProvidedBySeller ? 1.0 : 0.0,

    Warranty == WarrantyType.OneYear ? 1.0 : 0.0,
    Warranty == WarrantyType.TwoYears ? 1.0 : 0.0,
    Warranty == WarrantyType.ThreeYears ? 1.0 : 0.0,
    (Warranty == WarrantyType.HalfYear || Warranty == WarrantyType.OneAndHalfYear) ? 1.0 : 0.0,

    Country == CountryType.China ? 1.0 : 0.0,

    BodyMaterial.HasFlag(BodyMaterialType.Steel) ? 1.0 : 0.0,
    BodyMaterial.HasFlag(BodyMaterialType.Plastic) ? 1.0 : 0.0,
    BodyMaterial.HasFlag(BodyMaterialType.Glass) ? 1.0 : 0.0,
    BodyMaterial.HasFlag(BodyMaterialType.Metal) ? 1.0 : 0.0,
    BodyMaterial.HasFlag(BodyMaterialType.Ceramic) ? 1.0 : 0.0,

    Functions.HasFlag(Function.TemperatureMaintenance) ? 1.0 : 0.0,
    Functions.HasFlag(Function.TeaBrewing) ? 1.0 : 0.0,
    Functions.HasFlag(Function.NoiseReduction) ? 1.0 : 0.0,
    Functions == Function.None ? 1.0 : 0.0
  ];
}

public enum WarrantyType
{
  No,
  HalfYear,
  OneYear,
  OneAndHalfYear,
  TwoYears,
  ThreeYears
}

[Flags]
public enum BodyMaterialType
{
  None = 0,
  Plastic = 1,
  Steel = 2,
  Glass = 4,
  Metal = 8,
  Ceramic = 16,
  Silicone = 32,
  Aluminum = 64
}

[Flags]
public enum Function
{
  None = 0,
  NoiseReduction = 1,
  TemperatureMaintenance = 2,
  TeaBrewing = 4
}

public enum CountryType
{
  China,
  Russia,
  Turkey,
  Usa,
  Germany,
  Other
}
