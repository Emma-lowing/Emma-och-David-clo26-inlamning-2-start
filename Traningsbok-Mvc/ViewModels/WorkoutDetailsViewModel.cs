namespace Traningsbok.ViewModels;

/// <summary>
/// Ett pass som det ska visas, med övningarna redan formaterade.
/// </summary>
public class WorkoutDetailsViewModel
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string Type { get; set; } = "";
    public List<string> ExerciseLines { get; set; } = new();

    // Du behöver bygga ut detta mer: längd, anteckning och utövare när modellen Workout har de fälten.
}
