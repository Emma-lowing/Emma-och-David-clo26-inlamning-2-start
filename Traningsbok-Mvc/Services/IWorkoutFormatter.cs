namespace Traningsbok.Services;

/// <summary>
/// Formaterar vikt och träningstid så att de går att läsa (önskemål 1).
/// </summary>
public interface IWorkoutFormatter
{
    /// <summary>Till exempel "62,5 kg".</summary>
    string FormatWeight(decimal weight);

    /// <summary>Till exempel "1 h 05 min", eller "45 min" under en timme.</summary>
    string FormatDuration(int minutes);
}
