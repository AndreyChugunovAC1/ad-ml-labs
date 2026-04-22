using CsvHelper.Configuration;
using DataAnalyzer.DataAnalyzer.Interfaces;
using DataAnalyzer.Entities;

namespace DataAnalyzer.Dto;

/// <summary>
/// Только для парсинга
/// </summary>
public class OrderDto
{
  public string? OrderId { get; set; }
  public string? CustomerId { get; set; }
  public DateTime Date { get; set; }
  public int Age { get; set; }
  public Gender Gender { get; set; }
  public City City { get; set; }
  public ProductCategory ProductCategory { get; set; }
  public double UnitPrice { get; set; }
  public int Quantity { get; set; }
  public double DiscountAmount { get; set; }
  public double TotalAmount { get; set; }
  public PaymentMethod PaymentMethod { get; set; }
  public DeviceType DeviceType { get; set; }
  public double SessionDurationMinutes { get; set; }
  public int PagesViewed { get; set; }
  public bool IsReturningCustomer { get; set; }
  public int DeliveryTimeDays { get; set; }
  public int CustomerRating { get; set; }
}

public class OrderDtoMap : ClassMap<OrderDto>
{
  public OrderDtoMap()
  {
    Map(x => x.OrderId).Name("Order_ID");
    Map(x => x.CustomerId).Name("Customer_ID");
    Map(x => x.Date).Name("Date");
    Map(x => x.Age).Name("Age");

    Map(x => x.Gender).Name("Gender").Convert(row =>
    {
      var field = row.Row.GetField("Gender");
      if (string.IsNullOrWhiteSpace(field))
        return Gender.Other;
      return Enum.Parse<Gender>(field, ignoreCase: true);
    });

    Map(x => x.City).Name("City").Convert(row =>
    {
      var field = row.Row.GetField("City");
      return Enum.Parse<City>(field!, ignoreCase: true);
    });

    Map(x => x.ProductCategory).Name("Product_Category").Convert(row =>
    {
      var field = row.Row.GetField("Product_Category");
      return Enum.Parse<ProductCategory>(field!.Replace(" & ", ""), ignoreCase: true);
    });

    Map(x => x.UnitPrice).Name("Unit_Price");
    Map(x => x.Quantity).Name("Quantity");
    Map(x => x.DiscountAmount).Name("Discount_Amount");
    Map(x => x.TotalAmount).Name("Total_Amount");

    Map(x => x.PaymentMethod).Name("Payment_Method").Convert(row =>
    {
      var field = row.Row.GetField("Payment_Method");
      return Enum.Parse<PaymentMethod>(field!.Replace(" ", ""), ignoreCase: true);
    });

    Map(x => x.DeviceType).Name("Device_Type").Convert(row =>
    {
      var field = row.Row.GetField("Device_Type");
      return Enum.Parse<DeviceType>(field!, ignoreCase: true);
    });

    Map(x => x.SessionDurationMinutes).Name("Session_Duration_Minutes");
    Map(x => x.PagesViewed).Name("Pages_Viewed");

    Map(x => x.IsReturningCustomer).Name("Is_Returning_Customer").Convert(row => 
    {
      var field = row.Row.GetField("Is_Returning_Customer");
      return bool.Parse(field!);
    });

    Map(x => x.DeliveryTimeDays).Name("Delivery_Time_Days");
    Map(x => x.CustomerRating).Name("Customer_Rating");
  }
}