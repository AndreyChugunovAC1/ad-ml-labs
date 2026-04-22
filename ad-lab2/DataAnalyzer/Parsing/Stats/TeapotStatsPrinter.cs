using DataAnalyzer.Entities;
using DataAnalyzer.Parsing.Dto;

namespace DataAnalyzer.DataAnalyzer.Parsing.Stats;

public class TeapotStatsPrinter(IReadOnlyCollection<Teapot> teapots) : BaseStatsPrinter
{
  public override void PrintStats()
  {
    Console.WriteLine("=== BOOLEAN FIELDS ===");
    PrintBool(nameof(Teapot.AIHelper), teapots.Select(x => x.AIHelper));
    PrintBool(nameof(Teapot.IsNew), teapots.Select(x => x.IsNew));
    PrintBool(nameof(Teapot.TeaBrewing), teapots.Select(x => x.TeaBrewing));
    PrintBool(nameof(Teapot.WaterFillingWithoutOpening), teapots.Select(x => x.WaterFillingWithoutOpening));
    PrintBool(nameof(Teapot.SoundSignalOnBoil), teapots.Select(x => x.SoundSignalOnBoil));
    PrintBool(nameof(Teapot.IndicationTemperature), teapots.Select(x => x.IndicationTemperature));
    PrintBool(nameof(Teapot.IndicationWaterLevel), teapots.Select(x => x.IndicationWaterLevel));
    PrintBool(nameof(Teapot.IndicationSwitchingOn), teapots.Select(x => x.IndicationSwitchingOn));
    PrintBool(nameof(Teapot.AutoShutOffNoWater), teapots.Select(x => x.AutoShutOffNoWater));
    PrintBool(nameof(Teapot.AutoShutOffOnLift), teapots.Select(x => x.AutoShutOffOnLift));
    PrintBool(nameof(Teapot.DelayedStart), teapots.Select(x => x.DelayedStart));
    PrintBool(nameof(Teapot.WarrantyProvidedBySeller), teapots.Select(x => x.WarrantyProvidedBySeller));

    Console.WriteLine();
    Console.WriteLine("=== NUMERIC FIELDS ===");
    PrintNum(nameof(Teapot.Weight), teapots.Select(x => x.Weight));
    PrintNum(nameof(Teapot.Width), teapots.Select(x => x.Width));
    PrintNum(nameof(Teapot.Length), teapots.Select(x => x.Length));
    PrintNum(nameof(Teapot.Height), teapots.Select(x => x.Height));
    PrintNum(nameof(Teapot.MaxVolume), teapots.Select(x => x.MaxVolume));
    PrintNum(nameof(Teapot.HeatingModesCount), teapots.Select(x => x.HeatingModesCount));
    PrintNum(nameof(Teapot.WallsCount), teapots.Select(x => x.WallsCount));

    Console.WriteLine();
    Console.WriteLine("=== ENUM FIELDS ===");
    PrintEnum(nameof(Teapot.Warranty), teapots.Select(x => x.Warranty));
    PrintEnum(nameof(Teapot.Country), teapots.Select(x => x.Country));

    Console.WriteLine();
    Console.WriteLine("=== FLAG ENUM FIELDS ===");
    PrintEnumFlags(nameof(Teapot.BodyMaterial), teapots.Select(x => x.BodyMaterial));
    PrintEnumFlags(nameof(Teapot.Functions), teapots.Select(x => x.Functions));
  }

}