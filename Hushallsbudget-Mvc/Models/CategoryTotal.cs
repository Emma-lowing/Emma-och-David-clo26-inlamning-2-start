namespace Hushallsbudget.Models;

/// <summary>
/// En rad i månadsöversikten, till exempel "Mat: 249,50 kr".
/// </summary>
public class CategoryTotal
{
    public string CategoryName { get; set; } = "";
    public decimal Amount { get; set; }
}
