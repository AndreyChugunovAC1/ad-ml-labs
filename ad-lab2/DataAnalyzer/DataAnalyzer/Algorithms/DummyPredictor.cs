using DataAnalyzer.DataAnalyzer.Interfaces;

namespace DataAnalyzer.DataAnalyzer.Algorithms;

public class DummyPredictor(bool truthness) : IPredictor
{
  public double PredictScore(IReadOnlyList<double> _) => truthness ? 1.0 : -1.0;
}
