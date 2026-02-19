using DataAnalyzer.Utils;

public class DaNormalizer(ICollection<double> allValues)
{
  private readonly double _mean = allValues.Mean();
  private readonly double _std = allValues.Std();

  public double NormalizedValueOf(double value)
  {
    return _std == 0 ? 0 : (value - _mean) / _std;
  }
}