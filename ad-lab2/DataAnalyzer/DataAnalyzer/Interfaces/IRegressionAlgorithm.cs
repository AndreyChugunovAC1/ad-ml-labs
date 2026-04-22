namespace DataAnalyzer.DataAnalyzer.Interfaces;

public interface IRegressionAlgorithm<TPredictor> where TPredictor : IPredictor
{
  public TPredictor AnalyzeData(IReadOnlyCollection<IReadyToAnalyzeObject> data);
}