using DataAnalyzer.DataAnalyzer.Interfaces;

namespace DataAnalyzer.DataAnalyzer.Algorithms;

public delegate double LossFunction(double margin);
public delegate double LossDerivative(double margin);

public record GradientDescentTrainingParams(
  LossFunction LossFunc,
  LossDerivative LossDerivative,
  double LearningRate = 0.01,
  double L1Strength = 0.0,
  double L2Strength = 0.0,
  int MaxEpochs = 1000,
  double Tolerance = 1e-6
)
{
  // логистическая
  public static readonly LossFunction LogisticLoss = x => 
    Math.Log(1 + Math.Exp(-x));
  
  public static readonly LossDerivative LogisticLossDerivative = x => 
    -1 / (1 + Math.Exp(x));
  
  // SVM
  public static readonly LossFunction HingeLoss = x => 
    Math.Max(0, 1 - x);
  
  public static readonly LossDerivative HingeLossDerivative = x => 
    x >= 1 ? 0 : -1;
  
  // экспоненциальная потеря
  public static readonly LossFunction ExponentialLoss = x => 
    Math.Exp(-x);
  
  public static readonly LossDerivative ExponentialLossDerivative = x => 
    -Math.Exp(-x);

  public static readonly GradientDescentTrainingParams BestParams = new(
    LossFunc: LogisticLoss,
    LossDerivative: LogisticLossDerivative,
    LearningRate: 0.1,
    L1Strength: 0.0,
    L2Strength: 0.01,
    MaxEpochs: 100,
    Tolerance: 1e-4
  );

  public static GradientDescentTrainingParams ParamsFromL(double l1, double l2) => new(
    LossFunc: LogisticLoss,
    LossDerivative: LogisticLossDerivative,
    LearningRate: 0.1,
    L1Strength: l1,
    L2Strength: l2,
    MaxEpochs: 100,
    Tolerance: 1e-4
  );
}

public class GradientDescentClassifier(GradientDescentTrainingParams parameters) : IRegressionAlgorithm<LinearPredictor>
{
  private readonly GradientDescentTrainingParams _parameters = parameters;

  public LinearPredictor AnalyzeData(IReadOnlyCollection<IReadyToAnalyzeObject> data)
  {
    int featureCount = data.First().Features.Count;

    var weights = new double[featureCount];
    double bias = 0;
    var random = new Random();

    for (int i = 0; i < featureCount; i++)
      weights[i] = (random.NextDouble() - 0.5) * 0.01;

    double previousLoss = double.MaxValue;

    for (int epoch = 0; epoch < _parameters.MaxEpochs; epoch++)
    {
      double totalLoss = 0;
      double[] weightGradients = new double[featureCount];
      double biasGradient = 0;

      foreach (var sample in data)
      {
        double score = bias + weights.Zip(sample.Features, (w, f) => w * f).Sum();
        double margin = sample.IsTargetAsDouble * score;

        totalLoss += _parameters.LossFunc(margin);
        double derivative = _parameters.LossDerivative(margin);

        biasGradient += derivative * sample.IsTargetAsDouble;

        for (int j = 0; j < featureCount; j++)
        {
          weightGradients[j] += derivative * sample.IsTargetAsDouble * sample.Features[j];
        }
      }

      int sampleCount = data.Count;
      biasGradient /= sampleCount;
      for (int j = 0; j < featureCount; j++)
      {
        weightGradients[j] /= sampleCount;
      }

      totalLoss += CalculateRegularizationLoss(weights);
      AddRegularizationGradients(weightGradients, weights);

      bias -= _parameters.LearningRate * biasGradient;
      for (int j = 0; j < featureCount; j++)
      {
        weights[j] -= _parameters.LearningRate * weightGradients[j];
      }

      if (Math.Abs(previousLoss - totalLoss) < _parameters.Tolerance)
        break;

      previousLoss = totalLoss;
    }

    return new LinearPredictor(weights, bias);
  }

  private double CalculateRegularizationLoss(double[] weights)
  {
    double l1Loss = weights.Sum(Math.Abs);
    double l2Loss = weights.Sum(w => w * w);

    return _parameters.L1Strength * l1Loss + _parameters.L2Strength * l2Loss;
  }

  private void AddRegularizationGradients(double[] gradients, double[] weights)
  {
    for (int i = 0; i < gradients.Length; i++)
    {
      gradients[i] += _parameters.L1Strength * Math.Sign(weights[i]);
      gradients[i] += 2 * _parameters.L2Strength * weights[i];
    }
  }
}
