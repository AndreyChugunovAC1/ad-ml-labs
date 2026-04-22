using DataAnalyzer.DataAnalyzer.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace DataAnalyzer.DataAnalyzer.Algorithms;

public class RidgeRegressionClassifier(double Lambda = 0.1) : IRegressionAlgorithm<LinearPredictor>
{
  public LinearPredictor AnalyzeData(IReadOnlyCollection<IReadyToAnalyzeObject> data)
  {
    var (X, y) = PrepareMatrixes(data);

    var matrixX = DenseMatrix.OfArray(X);
    var vectorY = DenseVector.OfArray(y);

    var weights = CalculateWeights(matrixX, vectorY, Lambda);

    return new LinearPredictor(weights.SkipLast(1), weights.Last());
  }

  private static (double[,] X, double[] y) PrepareMatrixes(IReadOnlyCollection<IReadyToAnalyzeObject> data)
  {
    int n = data.Count;
    int featureCount = data.First().Features.Count;
    int cols = featureCount + 1;

    double[,] X = new double[n, cols];
    double[] y = new double[n];

    int i = 0;
    foreach (var item in data)
    {
      for (int j = 0; j < featureCount; j++)
      {
        X[i, j] = item.Features[j];
      }

      X[i, featureCount] = 1.0;
      y[i] = item.IsTargetAsDouble;

      i++;
    }

    return (X, y);
  }

  private static Vector<double> CalculateWeights(Matrix<double> X, Vector<double> y, double lambda)
  {
    int m = X.ColumnCount;
    var XtX = X.TransposeThisAndMultiply(X);

    // X^TX += LI
    for (int i = 0; i < m; i++)
    {
      XtX[i, i] += lambda;
    }

    // X^T * y
    var Xty = X.TransposeThisAndMultiply(y);

    // Solution for (X^TX + LI) * w = X^Ty
    return XtX.Solve(Xty);
  }
}
