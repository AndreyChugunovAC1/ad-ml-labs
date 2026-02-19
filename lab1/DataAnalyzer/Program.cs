using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DataAnalyzer.Dto;
using DataAnalyzer.Entities;
using DataAnalyzer.Utils;

var configRead = new CsvConfiguration()
{
  Delimiter = "\t",
  HasHeaderRecord = true,
  CultureInfo = CultureInfo.InvariantCulture
};

List<TeapotDto> records;
using (var reader = new StreamReader("data.csv"))
{
  using var csv = new CsvReader(reader, configRead);
  csv.Configuration.RegisterClassMap<TeapotDtoMap>();
  records = csv.GetRecords<TeapotDto>().ToList();
}

var teapots = records.Select(rawTeapot => TeapotManager.CreateTeapotFromDto(rawTeapot)).ToList();


Console.WriteLine(string.Join(", ", records.Select(t => t.Warranty).ToHashSet()));
Console.WriteLine();
Console.WriteLine($"IsNew: {teapots.Count(t => t.IsNew)} {teapots.Count(t => !t.IsNew)}");
Console.WriteLine($"Weight: {teapots.Sum(t => t.Weight ?? 0)}");
Console.WriteLine($"Dimensions: {teapots.Average(t => t.Height)} {teapots.Average(t => t.Length)} {teapots.Average(t => t.Width)}");
Console.WriteLine($"{teapots.Count(t => t.WarrantyProvidedBySeller ?? false)} -- {teapots.Count(t => !t.WarrantyProvidedBySeller ?? false)} -- {teapots.Count(t => t.WarrantyProvidedBySeller == null)}");
Console.WriteLine($"{teapots.Count(t => t.TeaBrewing)} -- {teapots.Count(t => !t.TeaBrewing)}");
Console.WriteLine($"{teapots.Count(t => t.WaterFillingWithoutOpening)} -- {teapots.Count(t => !t.WaterFillingWithoutOpening)}");
Console.WriteLine($"{teapots.Count(t => t.SoundSignalOnBoil)} -- {teapots.Count(t => !t.SoundSignalOnBoil)}");
Console.WriteLine($"{teapots.Count(t => t.IndicationSwitchingOn)} -- {teapots.Count(t => !t.IndicationSwitchingOn)}");
Console.WriteLine($"{teapots.Count(t => t.IndicationTemperature)} -- {teapots.Count(t => !t.IndicationTemperature)}");
Console.WriteLine($"{teapots.Count(t => t.IndicationWaterLevel)} -- {teapots.Count(t => !t.IndicationWaterLevel)}");
Console.WriteLine($"{teapots.Average(t => t.HeatingModesCount)}");
Console.WriteLine($"{teapots.Average(t => t.WallsCount)}");
Console.WriteLine($"{teapots.Max(t => t.MaxVolume)}");
Console.WriteLine($"{teapots.Count(t => t.AIHelper)}");
Console.WriteLine($"Metal: {teapots.Count(t => t.BodyMaterial.HasFlag(BodyMaterial.Metal))}");
Console.WriteLine($"Plastic: {teapots.Count(t => t.BodyMaterial.HasFlag(BodyMaterial.Plastic))}");
Console.WriteLine($"Glass: {teapots.Count(t => t.BodyMaterial.HasFlag(BodyMaterial.Glass))}");
Console.WriteLine($"Aluminum: {teapots.Count(t => t.BodyMaterial.HasFlag(BodyMaterial.Aluminum))}");
Console.WriteLine($"Steel: {teapots.Count(t => t.BodyMaterial.HasFlag(BodyMaterial.Steel))}");
Console.WriteLine($"Ceramic: {teapots.Count(t => t.BodyMaterial.HasFlag(BodyMaterial.Ceramic))}");
Console.WriteLine($"Silicone: {teapots.Count(t => t.BodyMaterial.HasFlag(BodyMaterial.Silicone))}");
Console.WriteLine($"{teapots.Count(t => t.AutoShutOffNoWater)} -- {teapots.Count(t => !t.AutoShutOffNoWater)}");
Console.WriteLine($"{teapots.Count(t => t.AutoShutOffOnLift)} -- {teapots.Count(t => !t.AutoShutOffOnLift)}");
Console.WriteLine($"Delayed start: {teapots.Count(t => t.DelayedStart)} -- {teapots.Count(t => !t.DelayedStart)}");
Console.WriteLine($"{teapots.Count(t => t.Country == null)}");
Console.WriteLine($"From China: {teapots.Count(t => t.Country == Country.China)}");
Console.WriteLine($"From Russia: {teapots.Count(t => t.Country == Country.Russia)}");
Console.WriteLine($"From Usa: {teapots.Count(t => t.Country == Country.Usa)}");
Console.WriteLine($"From Turkey: {teapots.Count(t => t.Country == Country.Turkey)}");
Console.WriteLine($"From Germany: {teapots.Count(t => t.Country == Country.Germany)}");
Console.WriteLine($"Function: {teapots.Count(t => t.Functions?.HasFlag(Function.NoiseReduction) ?? false)}");
Console.WriteLine($"Function: {teapots.Count(t => t.Functions?.HasFlag(Function.TeaBrewing) ?? false)}");
Console.WriteLine($"Function: {teapots.Count(t => t.Functions?.HasFlag(Function.TemperatureMaintenance) ?? false)}");

// foreach (var record in records)
// {
//   Console.WriteLine(record.Volume);
// }

Console.WriteLine(string.Join("\n", records.Select(t => t.Function).ToHashSet()));

TeapotManager.Normalize(teapots);
Console.WriteLine($"Dimensions: {teapots.Average(t => t.Height)} {teapots.Average(t => t.Length)} {teapots.Average(t => t.Width)}");

string json = TeapotJsonConverter.ConvertToJson(teapots);
File.WriteAllText("data.json", json);
string arff = TeapotArffConverter.ConvertToArff(teapots);
File.WriteAllText("data.arff", arff);

using (var stream = new StreamWriter("data_final.cvs"))
{
  var configWrite = new CsvConfiguration()
  {
    Delimiter = ",",
    HasHeaderRecord = true,
    CultureInfo = CultureInfo.InvariantCulture
  };

  using var csv = new CsvWriter(stream, configWrite);
  csv.WriteRecords(teapots);
}