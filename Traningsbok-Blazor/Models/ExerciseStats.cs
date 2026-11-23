namespace Traningsbok.Models;

/// <summary>
/// Statistik för en övning: högsta vikt och totalt antal set, i alla pass (funktion 7).
/// </summary>
public class ExerciseStats
{
    public string ExerciseName { get; set; } = "";
    public decimal MaxWeight { get; set; }
    public int TotalSets { get; set; }
}
