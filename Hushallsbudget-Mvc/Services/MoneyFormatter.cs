namespace Hushallsbudget.Services;

/// <summary>
/// Ska byggas av er. Metoden fungerar redan, men fel, så att ni ser vad som ska rättas.
/// </summary>
public class MoneyFormatter : IMoneyFormatter
{
    // Den här skriver "1249,50 kr" i stället för "1 249,50 kr",
    // och en utgift visas med minustecken, till exempel "-249,50 kr".
    // Du behöver bygga ut detta mer, så att den följer önskemål 1.
    public string Format(decimal amount) => $"{amount:F2} kr";
}
