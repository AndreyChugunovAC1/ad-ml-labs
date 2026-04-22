using System.Collections.ObjectModel;

namespace DataAnalyzer.DataAnalyzer.Utils;

public static class DaUtils
{
  public static double Mean(this IEnumerable<double> values)
  {
    return values.Average();
  }

  public static double Var(this IEnumerable<double> values)
  {
    var mean = values.Mean();
    return values.Select(v => Math.Pow(v - mean, 2)).Average();
  }

  public static double Std(this IEnumerable<double> values)
  {
    return Math.Sqrt(values.Var());
  }
}