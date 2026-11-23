namespace Hushallsbudget.Models;

/// <summary>
/// En kategori, till exempel "Mat" eller "Boende".
/// </summary>
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    // Du behöver bygga ut detta mer.
    // Beställningen kräver en månadsbudget per kategori (här eller i en egen modell),
    // och att "Övrigt" aldrig kan tas bort.
}
