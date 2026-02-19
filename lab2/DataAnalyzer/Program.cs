using System.Globalization;
using DataAnalyzer;
using DataAnalyzer.DataAnalyzer;
using DataAnalyzer.DataAnalyzer.Algorithms;

// LearningCurveGraph("rr_emp_risk.csv");
// LearningCurveGraph("gd_emp_risk.csv");

// TargetFunctionGraph("rr_target_function.csv");
// TargetFunctionGraph("gd_target_function.csv");

// L1RegularizationCoefficientGraph("l1_graph.csv");
// L2RegularizationCoefficientGraph("l2_graph.csv");

#pragma warning disable CS8321 // Local function is declared but never used
static void L1RegularizationCoefficientGraph(string filename)
{
  var objects = Handy.GetPreparedTeapots();

  var regularizationValues = Enumerable.Range(-4, 7)
      .Select(power => Math.Pow(10, power))
      .ToList();

  using var file = File.CreateText($"{Handy.BASE_PATH}/{filename}");

  int featureCount = objects.First().Features.Count;
  file.Write("regularization_coef");
  for (int i = 0; i < featureCount; i++)
    file.Write($",weight_{i}");
  file.WriteLine();

  foreach (var coef in regularizationValues)
  {
    var parameters = GradientDescentTrainingParams.ParamsFromL(l1: coef, l2: 0.0);
    var algorithm = new GradientDescentClassifier(parameters);
    var predictor = algorithm.AnalyzeData(objects);

    file.Write(coef.ToString(CultureInfo.InvariantCulture));
    for (int i = 0; i < featureCount; i++)
      file.Write($",{predictor.Weights[i].ToString(CultureInfo.InvariantCulture)}");
    file.WriteLine();
  }
}

static void L2RegularizationCoefficientGraph(string filename)
{
  var objects = Handy.GetPreparedTeapots();

  var regularizationValues = Enumerable.Range(-4, 7)
      .Select(power => Math.Pow(10, power))
      .ToList();

  using var file = File.CreateText($"{Handy.BASE_PATH}/{filename}");

  int featureCount = objects.First().Features.Count;
  file.Write("regularization_coef");
  for (int i = 0; i < featureCount; i++)
    file.Write($",weight_{i}");
  file.WriteLine();

  foreach (var coef in regularizationValues)
  {
    var parameters = GradientDescentTrainingParams.ParamsFromL(l1: 0.0, l2: coef);
    var algorithm = new GradientDescentClassifier(parameters);
    var predictor = algorithm.AnalyzeData(objects);

    file.Write(coef.ToString(CultureInfo.InvariantCulture));
    for (int i = 0; i < featureCount; i++)
      file.Write($",{predictor.Weights[i].ToString(CultureInfo.InvariantCulture)}");
    file.WriteLine();
  }
}

static void TargetFunctionGraph(string filename)
{
  var objects = Handy.GetPreparedTeapots();
  var random = new Random();
  const int TIMES = 10;
  var algorithm = new RidgeRegressionClassifier();

  var testRatios = Enumerable.Range(1, 5).Select(r => r * 0.1).ToList();

  using var file = File.CreateText($"{Handy.BASE_PATH}/{filename}");
  file.WriteLine("test_ratio,empirical_risk,repetition");

  foreach (var testRatio in testRatios)
  {
    Console.WriteLine($"test_ratio = {testRatio}");
    for (int rep = 0; rep < TIMES; rep++)
    {
      Console.WriteLine($"  test_ratio = {testRatio}; iteration = {rep + 1}/{TIMES}");
      var shuffled = objects.OrderBy(_ => random.Next()).ToList();
      var testCount = (int)(objects.Count * testRatio);
      var testObjects = shuffled.Take(testCount).ToList();
      var trainObjects = shuffled.Skip(testCount).ToList();

      var predictor = algorithm.AnalyzeData(trainObjects);
      var analyzer = new ResultAnalyzer(testObjects);
      var risk = analyzer.RidgeRegressionEmpiricalRisk(predictor, 0.1);

      file.WriteLine($"{testRatio.ToString(CultureInfo.InvariantCulture)},{risk.ToString(CultureInfo.InvariantCulture)},{rep}");
    }
  }

  var fullPredictor = algorithm.AnalyzeData(objects);
  var fullAnalyzer = new ResultAnalyzer(objects);
  var fullRisk = fullAnalyzer.RidgeRegressionEmpiricalRisk(fullPredictor, 0.1);

  file.WriteLine($"\n\n{fullRisk.ToString(CultureInfo.InvariantCulture)}");
}

static void LearningCurveGraph(string filename)
{
  var objects = Handy.GetPreparedTeapots();
  var random = new Random(42);

  const int TIMES = 10;
  var algorithm = new GradientDescentClassifier(GradientDescentTrainingParams.BestParams);
  var file = File.CreateText($"{Handy.BASE_PATH}/{filename}");
  file.WriteLine("x,y");
  foreach (var size in Enumerable.Range(1, 10).Select(s => s * 100))
  {
    double empiricalRisk = 0.0;
    Console.WriteLine($"training_size = {size}");
    for (int i = 0; i < TIMES; i++)
    {
      var trainObjects = objects.OrderBy(_ => random.Next()).Take(size).ToList();
      var predictor = algorithm.AnalyzeData(trainObjects);
      var resultAnalyzer = new ResultAnalyzer(trainObjects);

      empiricalRisk += 1 - resultAnalyzer.Accuracy(predictor);
    }
    file.WriteLine($"{size},{(empiricalRisk / TIMES).ToString(CultureInfo.InvariantCulture)}");
  }
  file.Close();
}
#pragma warning restore CS8321 // Local function is declared but never used

/* 
var testSize = 0.2;
var nRepetitions = 10;
var trainSizes = Enumerable.Range(2, 9).Select(s => s * 100).ToList();

using var file = File.CreateText($"{Handy.BASE_PATH}/learning_curve_with_ci.csv");
file.WriteLine("train_size,train_score,test_score,repetition");

foreach (var trainSize in trainSizes)
{
  Console.WriteLine($"train_size = {trainSize}");

  for (int repetition = 0; repetition < nRepetitions; repetition++)
  {
    var shuffled = objects.OrderBy(_ => random.Next()).ToList();
    var trainObjects = shuffled.Take(trainSize).ToList();
    var testCount = (int)(objects.Count * testSize);
    var testObjects = shuffled.Skip(trainSize).Take(testCount).ToList();

    var predictor = algorithm.AnalyzeData(trainObjects);
    var resultAnalyzerTrain = new ResultAnalyzer(trainObjects);
    var resultAnalyzerTest = new ResultAnalyzer(testObjects);

    var trainAccuracy = resultAnalyzerTrain.Accuracy(predictor);
    var testAccuracy = resultAnalyzerTest.Accuracy(predictor);

    file.WriteLine(string.Join(",",
      $"{trainSize}",
      $"{trainAccuracy.ToString(CultureInfo.InvariantCulture)}",
      $"{testAccuracy.ToString(CultureInfo.InvariantCulture)}",
      $"{repetition}"));
  }
}
*/
/* 
var algorithm = new RidgeRegressionClassifier(bestLambda);
var file = File.CreateText($"{Handy.BASE_PATH}/RidgeRegressionClassifier_curve_with_smooth_risk.csv");
file.WriteLine("y,x");
foreach (var size in Enumerable.Range(20, 80).Select(s => s * 10))
{
  var trainObjects = objects.OrderBy(_ => random.Next()).Take(size).ToList();
  var predictor = algorithm.AnalyzeData(trainObjects);
  var resultAnalyzer = new ResultAnalyzer(objects);
  file.WriteLine($"{size},{resultAnalyzer.Accuracy(predictor).ToString(CultureInfo.InvariantCulture)}");
}
file.Close();
 */
/*
Лучшее lambda:
var lambdaStats = new Dictionary<double, double>();

foreach (double lambda in new List<double> { 0.001, 0.01, 0.1, 1.0, 10.0, 100.0, 1000.0 })
{
  var algorithmTmp = new RidgeRegressionClassifier(lambda);
  var predictorTmp = algorithmTmp.AnalyzeData(trainingPart);

  lambdaStats[lambda] = resultAnalyzer.Accuracy(predictorTmp);
}
*/