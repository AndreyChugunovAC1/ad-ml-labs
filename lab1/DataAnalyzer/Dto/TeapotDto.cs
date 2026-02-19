using CsvHelper.Configuration;

namespace DataAnalyzer.Dto;

public class TeapotDto
{
  public string? Alice { get; set; }
  public double? Weight { get; set; }
  public string? Appearance { get; set; }
  public string? Dimensions { get; set; }
  public string? DimensionsAlt { get; set; }
  public string? Warranty { get; set; }
  public string? WarrantyProvidedBy { get; set; }
  public string? TemperatureRange { get; set; }
  public string? PowerCordLength { get; set; }
  public string? TeaBrewing { get; set; }
  public string? WaterFillingWithoutOpening { get; set; }
  public string? SoundSignalOnBoil { get; set; }
  public string? Indication { get; set; }
  public string? HeatingModesCount { get; set; }
  public string? WallsCount { get; set; }
  public string? MaxVolume { get; set; }
  public string? Marusya { get; set; }
  public string? BodyMaterial { get; set; }
  public string? Volume { get; set; }
  public string? AutoShutOffOnBoil { get; set; }
  public string? AutoShutOffNoWater { get; set; }
  public string? AutoShutOffOnLift { get; set; }
  public string? DelayedStart { get; set; }
  public string? PowerCordCompartment { get; set; }
  public string? Connection { get; set; }
  public string? BodyLighting { get; set; }
  public string? PowerConsumption { get; set; }
  public string? SmartHomeCompatible { get; set; }
  public string? TemperatureControl { get; set; }
  public string? Salute { get; set; }
  public string? TouchControl { get; set; }
  public string? SmartphoneRemote { get; set; }
  public string? Condition { get; set; }
  public string? HeatRetention { get; set; }
  public string? LidOpeningType { get; set; }
  public string? Lifespan { get; set; }
  public string? Country { get; set; }
  public string? NoiseReduction { get; set; }
  public string? HeatingElementType { get; set; }
  public string? ScaleFilter { get; set; }
  public string? Function { get; set; }
  public string? Color { get; set; }
  public string? BodyLightingColor { get; set; }
  public string? DigitalDisplay { get; set; }
  public string? Ecosystem { get; set; }
}

public sealed class TeapotDtoMap : CsvClassMap<TeapotDto>
{
  public TeapotDtoMap()
  {
    Map(t => t.Alice).Name("Алиса");
    Map(t => t.Weight).Name("Вес");
    Map(t => t.Appearance).Name("Внешний вид");
    Map(t => t.Dimensions).Name("Габаритные размеры (В*Ш*Г)");
    Map(t => t.DimensionsAlt).Name("Габаритные размеры (В*Ш*Д)");
    Map(t => t.Warranty).Name("Гарантия");
    Map(t => t.WarrantyProvidedBy).Name("Гарантия предоставляется");
    Map(t => t.TemperatureRange).Name("Диапазон температур");
    Map(t => t.PowerCordLength).Name("Длина сетевого шнура");
    Map(t => t.TeaBrewing).Name("Заваривание чая");
    Map(t => t.WaterFillingWithoutOpening).Name("Залив воды без открытия крышки");
    Map(t => t.SoundSignalOnBoil).Name("Звуковой сигнал при закипании");
    Map(t => t.Indication).Name("Индикация");
    Map(t => t.HeatingModesCount).Name("Количество режимов нагрева");
    Map(t => t.WallsCount).Name("Количество стенок");
    Map(t => t.MaxVolume).Name("Максимальный объем");
    Map(t => t.Marusya).Name("Маруся");
    Map(t => t.BodyMaterial).Name("Материал корпуса");
    Map(t => t.Volume).Name("Объем");
    Map(t => t.AutoShutOffOnBoil).Name("Отключение при закипании");
    Map(t => t.AutoShutOffNoWater).Name("Отключение при отсутствии воды");
    Map(t => t.AutoShutOffOnLift).Name("Отключение при снятии");
    Map(t => t.DelayedStart).Name("Отложенный старт");
    Map(t => t.PowerCordCompartment).Name("Отсек для сетевого шнура");
    Map(t => t.Connection).Name("Подключение");
    Map(t => t.BodyLighting).Name("Подсветка корпуса");
    Map(t => t.PowerConsumption).Name("Потребляемая мощность");
    Map(t => t.SmartHomeCompatible).Name("Работает в системе Умный дом");
    Map(t => t.TemperatureControl).Name("Регулировка температуры");
    Map(t => t.Salute).Name("Салют");
    Map(t => t.TouchControl).Name("Сенсорное управление");
    Map(t => t.SmartphoneRemote).Name("Смартфон в качестве пульта ДУ");
    Map(t => t.Condition).Name("Состояние");
    Map(t => t.HeatRetention).Name("Сохранение тепла");
    Map(t => t.LidOpeningType).Name("Способ открытия крышки");
    Map(t => t.Lifespan).Name("Срок службы");
    Map(t => t.Country).Name("Страна");
    Map(t => t.NoiseReduction).Name("Технология снижения уровня шума");
    Map(t => t.HeatingElementType).Name("Тип нагревательного элемента");
    Map(t => t.ScaleFilter).Name("Фильтр от накипи");
    Map(t => t.Function).Name("Функция");
    Map(t => t.Color).Name("Цвет");
    Map(t => t.BodyLightingColor).Name("Цвет подсветки корпуса");
    Map(t => t.DigitalDisplay).Name("Цифровой дисплей");
    Map(t => t.Ecosystem).Name("Экосистема");
  }
}