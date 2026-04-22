using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DataAnalyzer.Dto;

namespace DataAnalyzer.Parser;

public static class CsvParser<T, TMap> where TMap : ClassMap<T>
{
  public static List<T> LoadValues(string filePath)
  {
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
      Delimiter = ",",
      TrimOptions = TrimOptions.Trim,
      DetectDelimiter = false,
      BadDataFound = null,
      MissingFieldFound = null,
      HeaderValidated = null
    };

    using var reader = new StreamReader(filePath);
    using var csv = new CsvReader(reader, config);

    csv.Context.RegisterClassMap<TMap>();

    return csv.GetRecords<T>().ToList();
  }
}