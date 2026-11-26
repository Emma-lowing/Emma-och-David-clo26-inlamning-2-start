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

    // WorkoutId behövs inte eftersom övningarna
    // lagras som inbäddade dokument i Workout.
}
