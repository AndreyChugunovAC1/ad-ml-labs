using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using DataAnalyzer.Entities;

namespace DataAnalyzer.Utils;

public static class TeapotArffConverter
{
  public static string ConvertToArff(List<Teapot> teapots, string relationName = "teapots")
  {
    var sb = new StringBuilder();

    sb.AppendLine($"@RELATION {relationName}");
    sb.AppendLine();

    AppendAttributes(sb);
    sb.AppendLine();
    sb.AppendLine("@DATA");

    foreach (var teapot in teapots)
      AppendTeapotData(sb, teapot);

    return sb.ToString();
  }

  private static void AppendAttributes(StringBuilder sb)
  {
    sb.AppendLine("@ATTRIBUTE AIHelper {true, false}");
    sb.AppendLine("@ATTRIBUTE Weight NUMERIC");
    sb.AppendLine("@ATTRIBUTE IsNew {true, false}");
    sb.AppendLine("@ATTRIBUTE Width NUMERIC");
    sb.AppendLine("@ATTRIBUTE Length NUMERIC");
    sb.AppendLine("@ATTRIBUTE Height NUMERIC");
    sb.AppendLine("@ATTRIBUTE Warranty {No, HalfYear, OneYear, OneAndHalfYear, TwoYears, ThreeYears}");
    sb.AppendLine("@ATTRIBUTE WarrantyProvidedBySeller {true, false}");
    sb.AppendLine("@ATTRIBUTE PowerCordLength STRING");
    sb.AppendLine("@ATTRIBUTE TeaBrewing {true, false}");
    sb.AppendLine("@ATTRIBUTE WaterFillingWithoutOpening {true, false}");
    sb.AppendLine("@ATTRIBUTE SoundSignalOnBoil {true, false}");
    sb.AppendLine("@ATTRIBUTE IndicationTemperature {true, false}");
    sb.AppendLine("@ATTRIBUTE IndicationWaterLevel {true, false}");
    sb.AppendLine("@ATTRIBUTE IndicationSwitchingOn {true, false}");
    sb.AppendLine("@ATTRIBUTE HeatingModesCount NUMERIC");
    sb.AppendLine("@ATTRIBUTE WallsCount NUMERIC");
    sb.AppendLine("@ATTRIBUTE MaxVolume NUMERIC");
    sb.AppendLine("@ATTRIBUTE BodyMaterial STRING");
    sb.AppendLine("@ATTRIBUTE AutoShutOffNoWater {true, false}");
    sb.AppendLine("@ATTRIBUTE AutoShutOffOnLift {true, false}");
    sb.AppendLine("@ATTRIBUTE DelayedStart {true, false}");
    sb.AppendLine("@ATTRIBUTE Country {Китай, Россия, Турция, США, Германия}");
    sb.AppendLine("@ATTRIBUTE Functions STRING");
  }

  private static void AppendTeapotData(StringBuilder sb, Teapot teapot)
  {
    var values = new List<string>
    {
      teapot.AIHelper.ToString().ToLower(),
      FormatNumeric(teapot.Weight),
      teapot.IsNew.ToString().ToLower(),
      FormatNumeric(teapot.Width),
      FormatNumeric(teapot.Length),
      FormatNumeric(teapot.Height),
      teapot.Warranty.ToString(),
      FormatNullableBool(teapot.WarrantyProvidedBySeller),
      FormatString(teapot.PowerCordLength),
      teapot.TeaBrewing.ToString().ToLower(),
      teapot.WaterFillingWithoutOpening.ToString().ToLower(),
      teapot.SoundSignalOnBoil.ToString().ToLower(),
      teapot.IndicationTemperature.ToString().ToLower(),
      teapot.IndicationWaterLevel.ToString().ToLower(),
      teapot.IndicationSwitchingOn.ToString().ToLower(),
      FormatNumeric(teapot.HeatingModesCount),
      FormatNumeric(teapot.WallsCount),
      FormatNumeric(teapot.MaxVolume),
      FormatString(FormatBodyMaterial(teapot.BodyMaterial)),
      teapot.AutoShutOffNoWater.ToString().ToLower(),
      teapot.AutoShutOffOnLift.ToString().ToLower(),
      teapot.DelayedStart.ToString().ToLower(),
      FormatCountry(teapot.Country),
      FormatString(FormatFunctions(teapot.Functions))
    };

    sb.AppendLine(string.Join(",", values));
  }

  private static string FormatNumeric(double? value) =>
      value.HasValue
          ? value.Value.ToString(CultureInfo.InvariantCulture)
          : "?";

  private static string FormatNumeric(int? value) =>
      value.HasValue
          ? value.Value.ToString(CultureInfo.InvariantCulture)
          : "?";

  private static string FormatNullableBool(bool? value) =>
      value.HasValue ? value.Value.ToString().ToLower() : "?";

  private static string FormatString(string? value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return "?";
    return $"'{value.Replace("'", "''")}'";
  }

  private static string FormatCountry(Country? country) => country?.ToString() switch
  {
    "China" => "Китай",
    "Russia" => "Россия",
    "Turkey" => "Турция",
    "Usa" => "США",
    "Germany" => "Германия",
    _ => "?"
  };

  private static string FormatBodyMaterial(BodyMaterial material)
  {
    var materials = new List<string>();
    if (material.HasFlag(BodyMaterial.Plastic)) materials.Add("пластик");
    if (material.HasFlag(BodyMaterial.Steel)) materials.Add("сталь");
    if (material.HasFlag(BodyMaterial.Glass)) materials.Add("стекло");
    if (material.HasFlag(BodyMaterial.Metal)) materials.Add("металл");
    if (material.HasFlag(BodyMaterial.Ceramic)) materials.Add("керамика");
    if (material.HasFlag(BodyMaterial.Silicone)) materials.Add("силикон");
    if (material.HasFlag(BodyMaterial.Aluminum)) materials.Add("алюминий");

    return materials.Count > 0 ? string.Join("_", materials) : "?";
  }

  private static string FormatFunctions(Function? functions)
  {
    if (functions == null) return "?";

    var funcs = new List<string>();
    if (functions.Value.HasFlag(Function.NoiseReduction)) funcs.Add("шумоподавление");
    if (functions.Value.HasFlag(Function.TemperatureMaintenance)) funcs.Add("поддержание_температуры");
    if (functions.Value.HasFlag(Function.TeaBrewing)) funcs.Add("заваривание_чая");

    return funcs.Count > 0 ? string.Join("_", funcs) : "?";
  }
}
