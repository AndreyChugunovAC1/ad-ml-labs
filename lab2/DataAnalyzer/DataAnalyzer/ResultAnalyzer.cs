using DataAnalyzer.DataAnalyzer.Algorithms;
using DataAnalyzer.DataAnalyzer.Interfaces;

namespace DataAnalyzer.DataAnalyzer;

public class ResultAnalyzer(IReadOnlyCollection<IReadyToAnalyzeObject> objects)
{
  public double GradientDescentEmpiricalRisk(IPredictor predictor)
  {
    double sum = 0;

    foreach (var obj in objects)
    {
      double predicted = predictor.PredictScore(obj.Features);
      double actual = obj.IsTarget ? 1.0 : 0.0;

      sum += (actual - predicted) * (actual - predicted);
    }

    return sum / objects.Count;
  }

  public double RidgeRegressionEmpiricalRisk(LinearPredictor predictor, double lambda)
  {
    double mseSum = 0;

    foreach (var obj in objects)
    {
      double predicted = predictor.PredictScore(obj.Features);
      double actual = obj.IsTarget ? 1.0 : -1.0;

      mseSum += (actual - predicted) * (actual - predicted);
    }

    double mse = mseSum / objects.Count;
    double regularization = lambda * predictor.Weights.Sum(w => w * w);

    return mse + regularization;
  }

  public double Accuracy(IPredictor predictor) =>
      (double)objects.Count(o => o.IsTarget == predictor.Predict(o.Features)) / objects.Count;

  public double Precision(IPredictor predictor)
  {
    int truePositives = 0;
    int falsePositives = 0;

    foreach (var obj in objects)
    {
      var prediction = predictor.Predict(obj.Features);
      if (prediction)
      {
        if (obj.IsTarget) truePositives++;
        else falsePositives++;
      }
    }

    return truePositives + falsePositives == 0 ? 0 : (double)truePositives / (truePositives + falsePositives);
  }

  public double Recall(IPredictor predictor)
  {
    int truePositives = 0;
    int falseNegatives = 0;

    foreach (var obj in objects)
    {
      var prediction = predictor.Predict(obj.Features);
      if (obj.IsTarget)
      {
        if (prediction) truePositives++;
        else falseNegatives++;
      }
    }

    return truePositives + falseNegatives == 0 ? 0 : (double)truePositives / (truePositives + falseNegatives);
  }

  public double F1Score(IPredictor predictor)
  {
    var precision = Precision(predictor);
    var recall = Recall(predictor);

    return precision + recall == 0 ? 0 : 2 * precision * recall / (precision + recall);
  }

  public void PrintAllMetrics(IPredictor predictor)
  {
    Console.WriteLine($"=== Метрики успешности ===");
    Console.WriteLine($"Accuracy:  {Accuracy(predictor):F4}");
    Console.WriteLine($"Precision: {Precision(predictor):F4}");
    Console.WriteLine($"Recall:    {Recall(predictor):F4}");
    Console.WriteLine($"F1-Score:  {F1Score(predictor):F4}");
  }
}