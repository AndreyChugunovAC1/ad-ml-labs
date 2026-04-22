using DataAnalyzer.Dto;

namespace DataAnalyzer.DataAnalyzer.Parsing.Stats;

public class OrderStatsPrinter(IReadOnlyCollection<OrderDto> orders) : BaseStatsPrinter
{
  public override void PrintStats()
  {
    Console.WriteLine("=== ORDER DATASET STATISTICS ===");
    Console.WriteLine($"Total records: {orders.Count}");

    Console.WriteLine("\n=== NUMERIC FIELDS ===");
    PrintNum(nameof(OrderDto.Age), orders.Select(x => (double)x.Age));
    PrintNum(nameof(OrderDto.UnitPrice), orders.Select(x => x.UnitPrice));
    PrintNum(nameof(OrderDto.Quantity), orders.Select(x => (double)x.Quantity));
    PrintNum(nameof(OrderDto.DiscountAmount), orders.Select(x => x.DiscountAmount));
    PrintNum(nameof(OrderDto.TotalAmount), orders.Select(x => x.TotalAmount));
    PrintNum(nameof(OrderDto.SessionDurationMinutes), orders.Select(x => x.SessionDurationMinutes));
    PrintNum(nameof(OrderDto.PagesViewed), orders.Select(x => (double)x.PagesViewed));
    PrintNum(nameof(OrderDto.DeliveryTimeDays), orders.Select(x => (double)x.DeliveryTimeDays));
    PrintNum(nameof(OrderDto.CustomerRating), orders.Select(x => (double)x.CustomerRating));

    Console.WriteLine("\n=== CATEGORICAL FIELDS ===");
    PrintEnum(nameof(OrderDto.Gender), orders.Select(x => x.Gender));
    PrintEnum(nameof(OrderDto.City), orders.Select(x => x.City));
    PrintEnum(nameof(OrderDto.ProductCategory), orders.Select(x => x.ProductCategory));
    PrintEnum(nameof(OrderDto.PaymentMethod), orders.Select(x => x.PaymentMethod));
    PrintEnum(nameof(OrderDto.DeviceType), orders.Select(x => x.DeviceType));

    Console.WriteLine("\n=== BOOLEAN FIELD ===");
    var returningCustomers = orders.Count(x => x.IsReturningCustomer);
    var newCustomers = orders.Count(x => !x.IsReturningCustomer);
    Console.WriteLine($"{nameof(OrderDto.IsReturningCustomer)}: Returning={returningCustomers}, New={newCustomers}");

    Console.WriteLine("\n=== DATE FIELD ===");
    var dates = orders.Select(x => x.Date).ToArray();
    Console.WriteLine($"Date range: {dates.Min():yyyy-MM-dd} to {dates.Max():yyyy-MM-dd}");
  }
}