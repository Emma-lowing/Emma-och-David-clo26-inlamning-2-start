namespace Hushallsbudget.Models;

/// <summary>
/// Ett hushåll, till exempel "Familjen Berg". Minimal modell: bara det som README:n kräver.
/// </summary>
public class Household
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    // Du behöver bygga ut detta mer.
    // Beställningen säger att ett hushåll har 1–5 medlemmar. Fundera på hur medlemmarna
    // kopplas till hushållet: en lista här, eller ett HouseholdId i Member.
}
