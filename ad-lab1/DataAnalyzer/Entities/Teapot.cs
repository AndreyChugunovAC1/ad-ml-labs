using CsvHelper.Configuration;

namespace DataAnalyzer.Entities;

public enum Warranity
{
  No,
  HalfYear,
  OneYear,
  OneAndHalfYear,
  TwoYears,
  ThreeYears
}

[Flags]
public enum BodyMaterial
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

public enum Country
{
  China,
  Russia,
  Turkey,
  Usa,
  Germany
}

public class Teapot
{
  public bool AIHelper { get; set; }
  public double? Weight { get; set; }
  public bool IsNew { get; set; } = false;
  public double? Width { get; set; }
  public double? Length { get; set; }
  public double? Height { get; set; }
  public Warranity Warranty { get; set; }
  public bool? WarrantyProvidedBySeller { get; set; }
  // ?? public string? TemperatureRange { get; set; }
  public string? PowerCordLength { get; set; }
  public bool TeaBrewing { get; set; }
  public bool WaterFillingWithoutOpening { get; set; }
  public bool SoundSignalOnBoil { get; set; }
  // public string? Indication { get; set; }
  public bool IndicationTemperature { get; set; } = false;
  public bool IndicationWaterLevel { get; set; } = false;
  public bool IndicationSwitchingOn { get; set; } = false;
  public int? HeatingModesCount { get; set; }
  public int? WallsCount { get; set; }
  public double? MaxVolume { get; set; }
  public BodyMaterial BodyMaterial { get; set; }
  // public string? Volume { get; set; }
  // public bool AutoShutOffOnBoil { get; set; } -- not correctly collected
  public bool AutoShutOffNoWater { get; set; }
  public bool AutoShutOffOnLift { get; set; }
  public bool DelayedStart { get; set; }
  // public string? PowerCordCompartment { get; set; }
  // public string? Connection { get; set; }
  // ?? public string? BodyLighting { get; set; }
  // ?? public string? PowerConsumption { get; set; }
  // ?? public string? SmartHomeCompatible { get; set; }
  // ?? public string? TemperatureControl { get; set; }
  // ?? public string? Salute { get; set; }
  // ?? public string? TouchControl { get; set; }
  // ?? public string? SmartphoneRemote { get; set; }
  // ?? public string? Condition { get; set; }
  // ?? public string? HeatRetention { get; set; }
  // ?? public string? LidOpeningType { get; set; }
  // ?? public string? Lifespan { get; set; }
  public Country? Country { get; set; }
  // ?? public string? NoiseReduction { get; set; }
  // ?? public string? HeatingElementType { get; set; }
  // ?? public string? ScaleFilter { get; set; }
  public Function? Functions { get; set; }
  // public string? Color { get; set; }
  // ?? public string? BodyLightingColor { get; set; }
  // ?? public string? DigitalDisplay { get; set; }
  // ?? public string? Ecosystem { get; set; }
}
