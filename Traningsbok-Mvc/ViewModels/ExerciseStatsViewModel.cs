namespace Traningsbok.ViewModels;

/// <summary>
/// Statistik för en vald övning, med vikten redan formaterad.
/// </summary>
public class ExerciseStatsViewModel
{
    /// <summary>Övningen som användaren sökte på. Tom om ingen övning är vald än.</summary>
    public string Name { get; set; } = "";

    /// <summary>Övningar som finns i passen, som förslag i sökrutan.</summary>
    public List<string> ExerciseNames { get; set; } = new();

    public string? MaxWeight { get; set; }
    public int TotalSets { get; set; }

    public bool HasResult => MaxWeight is not null;
}
