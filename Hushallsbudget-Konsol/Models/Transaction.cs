namespace Hushallsbudget.Models;

/// <summary>
/// En transaktion: en inkomst eller en utgift. Minimal modell: bara det som README:n kräver.
/// I låtsasdatan är en utgift ett negativt belopp och en inkomst ett positivt, som i Carlos Excel-ark.
/// </summary>
public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public string Category { get; set; } = "";

    /// <summary>Valfri, till exempel "ICA". Tom text betyder ingen anteckning.</summary>
    public string Note { get; set; } = "";

    // Du behöver bygga ut detta mer.
    // Fundera på om inkomst och utgift ska skiljas åt med minustecken, som nu, eller med ett eget fält.
    // Fundera också på om kategorin ska sparas som text, som nu, eller som en koppling till Category.
    // Valet påverkar hur lätt önskemål 3 blir (transaktionerna flyttas till "Övrigt").
    // När ni ändrar fälten: uppdatera även UI/TransactionMenu.cs.
}
