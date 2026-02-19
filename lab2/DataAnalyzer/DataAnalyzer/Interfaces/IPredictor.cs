namespace DataAnalyzer.DataAnalyzer.Interfaces;

public interface IPredictor
{
  public virtual bool Predict(IReadOnlyList<double> features) => PredictScore(features) > 0;

  public double PredictScore(IReadOnlyList<double> features);
}