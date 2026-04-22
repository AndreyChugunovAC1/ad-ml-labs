using DataAnalyzer.DataAnalyzer.Parsing.Stats;
using DataAnalyzer.Dto;
using DataAnalyzer.Entities;
using DataAnalyzer.Entities.Manager;
using DataAnalyzer.Parser;
using DataAnalyzer.Parsing.Dto;

namespace DataAnalyzer;

public static class Handy
{
  public const string BASE_PATH = "../../../..";

  public static List<Teapot> GetPreparedTeapots()
  {
    var filePath = $"{BASE_PATH}/teapots.csv";

    var teapotDtos = CsvParser<TeapotDto, TeapotMap>.LoadValues(filePath);

    // normalize + convert ints to doubles:
    var teapots = TeapotManager.Prepare(teapotDtos).ToList();
    var stats = new TeapotStatsPrinter(teapots);
    stats.PrintStats();

    return teapots;
  }

  public static List<Order> GetPreparedOrders()
  {
    var filePath = "data.csv";

    var orderDtos = CsvParser<OrderDto, OrderDtoMap>.LoadValues(filePath);

    var stats = new OrderStatsPrinter(orderDtos);
    stats.PrintStats();

    // normalize + convert ints to doubles:
    var orders = OrderManager.Prepare(orderDtos).ToList();

    return orders;
  }
}