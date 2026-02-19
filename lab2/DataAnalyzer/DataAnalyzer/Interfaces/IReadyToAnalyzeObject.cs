namespace DataAnalyzer.DataAnalyzer.Interfaces;

public interface IReadyToAnalyzeObject
{
  /// <summary>
  /// Вектор признаков объекта для анализа
  /// </summary>
  public IReadOnlyList<double> Features { get; }

  /// <summary>
  /// Целевая метка
  /// </summary>
  public bool IsTarget { get; }
  public sealed double IsTargetAsDouble => IsTarget ? 1.0 : -1.0;
}