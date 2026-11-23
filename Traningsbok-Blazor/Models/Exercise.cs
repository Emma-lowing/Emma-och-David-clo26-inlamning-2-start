namespace Traningsbok.Models;

/// <summary>
/// En övning i ett pass, till exempel "Knäböj, 3 set × 8 reps, 62,5 kg".
/// </summary>
public class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Sets { get; set; }
    public int Reps { get; set; }
    public decimal Weight { get; set; }

    // Du behöver bygga ut detta mer.
    // Väljer ni en relationsdatabas behöver övningen troligen en koppling till sitt pass.
}
