namespace Traningsbok.Services;

/// <summary>
/// Ska byggas av er. Båda metoderna fungerar redan, men fel, så att ni ser vad som ska rättas.
/// </summary>
public class WorkoutFormatter : IWorkoutFormatter
{
    // Den här skriver "62,50 kg" i stället för "62,5 kg".
    // Du behöver bygga ut detta mer, så att den följer önskemål 1.
    public string FormatWeight(decimal weight) => $"{weight:F2} kg";

    // Den här skriver "65 min" i stället för "1 h 05 min".
    // Du behöver bygga ut detta mer, så att den följer önskemål 1.
    public string FormatDuration(int minutes) => $"{minutes} min";
}
