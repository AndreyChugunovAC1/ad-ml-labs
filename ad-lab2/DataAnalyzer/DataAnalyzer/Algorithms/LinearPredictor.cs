using DataAnalyzer.DataAnalyzer.Interfaces;

namespace DataAnalyzer.DataAnalyzer.Algorithms;

public class LinearPredictor(IEnumerable<double> weights, double delta) : IPredictor
{
  public IReadOnlyList<double> Weights { get; }= weights.ToList();

  public double PredictScore(IReadOnlyList<double> features)
  {
    return delta + features
      .Zip(Weights, (f, w) => f * w)
      .Sum();
  }
}
