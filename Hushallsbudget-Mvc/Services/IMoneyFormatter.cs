namespace Hushallsbudget.Services;

/// <summary>
/// Formaterar belopp i kronor.
/// </summary>
public interface IMoneyFormatter
{
    /// <summary>Till exempel "1 249,50 kr". En utgift visas som ett positivt belopp (önskemål 1).</summary>
    string Format(decimal amount);
}
