namespace Traningsbok.Models;

/// <summary>
/// En träningstyp, till exempel "Styrka" eller "Kondition".
/// </summary>
public class TrainingType
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public bool IsProtected { get; set; } // Skyddar Övrigt från borttagning så att träningspass alltid har en giltig träningstyp.

    // Du behöver bygga ut detta mer.
    // Beställningen kräver ett veckomål i minuter per träningstyp,
    // och att "Övrigt" aldrig kan tas bort.
}
