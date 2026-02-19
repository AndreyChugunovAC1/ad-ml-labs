using DataAnalyzer.DataAnalyzer.Utils;
using DataAnalyzer.Dto;

namespace DataAnalyzer.Entities.Manager;

public static class OrderManager
{
  public static ICollection<Order> Prepare(this ICollection<OrderDto> orders)
  {
    var normalizedOrders = orders.Select(o => new Order
    {
      OrderId = o.OrderId,
      CustomerId = o.CustomerId,
      Date = o.Date.ToOADate(),
      Age = o.Age,
      Gender = o.Gender,
      City = o.City,
      ProductCategory = o.ProductCategory,
      UnitPrice = o.UnitPrice,
      Quantity = o.Quantity,
      DiscountAmount = o.DiscountAmount,
      TotalAmount = o.TotalAmount,
      PaymentMethod = o.PaymentMethod,
      DeviceType = o.DeviceType,
      SessionDurationMinutes = o.SessionDurationMinutes,
      PagesViewed = o.PagesViewed,
      IsReturningCustomer = o.IsReturningCustomer,
      DeliveryTimeDays = o.DeliveryTimeDays,
      CustomerRating = o.CustomerRating
    }).ToList();

    var doubleProperties = typeof(Order).GetProperties()
        .Where(p => p.PropertyType == typeof(double))
        .ToList();

    foreach (var prop in doubleProperties)
    {
      var values = normalizedOrders.Select(o => (double)prop.GetValue(o)!).ToList();
      var normalizer = new DaNormalizer(values);

      foreach (var order in normalizedOrders)
      {
        var value = (double)prop.GetValue(order)!;
        prop.SetValue(order, normalizer.NormalizedValueOf(value));
      }
    }

    return normalizedOrders;
  }
}