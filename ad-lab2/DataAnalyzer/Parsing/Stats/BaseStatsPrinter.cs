using MathNet.Numerics.Statistics;

namespace DataAnalyzer.DataAnalyzer.Parsing.Stats;

public abstract class BaseStatsPrinter
{
  public abstract void PrintStats();

  protected static void PrintNum(string name, IEnumerable<double> values)
  {
    var arr = values.ToArray();
    Console.WriteLine($"{name}: Min={arr.Min():F2}, Max={arr.Max():F2}, Avg={arr.Average():F2}");
  }

  protected static void PrintBool(string name, IEnumerable<bool> values)
  {
    var arr = values.ToArray();
    Console.WriteLine($"{name}: True={arr.Count(x => x)}, False={arr.Count(x => !x)}");
  }

  protected static void PrintNum(string name, IEnumerable<double?> values)
  {
    var arr = values.ToArray();
    Console.Write($"{name}: Min={arr.Where(x => x.HasValue).Min():F3}, Max={arr.Where(x => x.HasValue).Max():F3} ");
    Console.WriteLine($"E={arr.Mean():F2}, Var={arr.Variance():F2}, Null={arr.Count(x => !x.HasValue)}");
  }

  protected static void PrintEnum<T>(string name, IEnumerable<T> values) where T : struct
  {
    var groups = values.GroupBy(x => x)
      .OrderByDescending(g => g.Count());
    Console.WriteLine($"{name}:");
    foreach (var g in groups)
    {
      Console.WriteLine($"   {g.Key} — {g.Count()}");
    }
  }

  protected static void PrintEnumFlags<T>(string name, IEnumerable<T> values) where T : Enum
  {
    var groups = values.GroupBy(x => x.ToString())
                      .OrderByDescending(g => g.Count());

    Console.WriteLine($"{name} (distinct combinations):");
    foreach (var g in groups)
    {
      Console.WriteLine($"   {g.Key} — {g.Count()}");
    }
  }

}