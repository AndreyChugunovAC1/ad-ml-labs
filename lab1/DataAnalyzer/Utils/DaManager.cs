namespace DataAnalyzer.Utils;

public static class DaManager
{
  public static double Mean(this ICollection<double> values)
  {
    return values.Average();
  }

  public static double Var(this ICollection<double> values)
  {
    var mean = values.Mean();
    return values.Select(v => Math.Pow(v - mean, 2)).Average();
  }

  public static double Std(this ICollection<double> values)
  {
    return Math.Sqrt(values.Var());
  }
}