using DataAnalyzer.DataAnalyzer.Interfaces;
using DataAnalyzer.Dto;

namespace DataAnalyzer.Entities;

public class Order : IReadyToAnalyzeObject
{
  public string? OrderId { get; set; }
  public string? CustomerId { get; set; }
  public double Date { get; set; }
  public double Age { get; set; }
  public Gender Gender { get; set; }
  public City City { get; set; }
  public ProductCategory ProductCategory { get; set; }
  public double UnitPrice { get; set; }
  public double Quantity { get; set; }
  public double DiscountAmount { get; set; }
  public double TotalAmount { get; set; }
  public PaymentMethod PaymentMethod { get; set; }
  public DeviceType DeviceType { get; set; }
  public double SessionDurationMinutes { get; set; }
  public double PagesViewed { get; set; }
  public bool IsReturningCustomer { get; set; }
  public double DeliveryTimeDays { get; set; }
  public double CustomerRating { get; set; }

  public IReadOnlyList<double> Features => [
    Date,
    // Age,
    UnitPrice,
    Quantity,
    DiscountAmount,
    TotalAmount,
    SessionDurationMinutes,
    PagesViewed,
    DeliveryTimeDays,
    // CustomerRating,

    // one-hot
    // Gender == Gender.Male ? 1.0 : 0.0,
    // Gender == Gender.Female ? 1.0 : 0.0,
    // Gender == Gender.Other ? 1.0 : 0.0,

    City == City.Ankara ? 1.0 : 0.0,
    City == City.Istanbul ? 1.0 : 0.0,
    City == City.Konya ? 1.0 : 0.0,
    City == City.Izmir ? 1.0 : 0.0,
    City == City.Kayseri ? 1.0 : 0.0,
    
    ProductCategory == ProductCategory.Books ? 1.0 : 0.0,
    ProductCategory == ProductCategory.HomeGarden ? 1.0 : 0.0,
    ProductCategory == ProductCategory.Sports ? 1.0 : 0.0,
    ProductCategory == ProductCategory.Food ? 1.0 : 0.0,
    ProductCategory == ProductCategory.Beauty ? 1.0 : 0.0,
    ProductCategory == ProductCategory.Toys ? 1.0 : 0.0,
    ProductCategory == ProductCategory.Fashion ? 1.0 : 0.0,
    ProductCategory == ProductCategory.Electronics ? 1.0 : 0.0,

    PaymentMethod == PaymentMethod.CreditCard ? 1.0 : 0.0,
    PaymentMethod == PaymentMethod.DebitCard ? 1.0 : 0.0,
    PaymentMethod == PaymentMethod.DigitalWallet ? 1.0 : 0.0,
    PaymentMethod == PaymentMethod.BankTransfer ? 1.0 : 0.0,
    PaymentMethod == PaymentMethod.CashOnDelivery ? 1.0 : 0.0,

    DeviceType == DeviceType.Mobile ? 1.0 : 0.0,
    DeviceType == DeviceType.Desktop ? 1.0 : 0.0,
    DeviceType == DeviceType.Tablet ? 1.0 : 0.0,

    IsReturningCustomer ? 1.0 : 0.0,
  ];

  public bool IsTarget => Gender == Gender.Female &&
    CustomerRating > 0 &&
    Age < 0;
}

public enum Gender
{
  Male,
  Female,
  Other
}

public enum City
{
  Ankara,
  Istanbul,
  Konya,
  Izmir,
  Kayseri,
  Bursa,
  Gaziantep,
  Adana,
  Eskisehir,
  Antalya
}

public enum ProductCategory
{
  No,
  Books,
  HomeGarden,
  Sports,
  Food,
  Beauty,
  Toys,
  Fashion,
  Electronics
}

public enum PaymentMethod
{
  CreditCard,
  DebitCard,
  DigitalWallet,
  BankTransfer,
  CashOnDelivery
}

public enum DeviceType
{
  Mobile,
  Desktop,
  Tablet
}
