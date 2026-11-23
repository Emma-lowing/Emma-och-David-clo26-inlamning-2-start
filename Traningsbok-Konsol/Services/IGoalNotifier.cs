using Traningsbok.Models;

namespace Traningsbok.Services;

/// <summary>
/// Meddelanden när en träningstyp närmar sig eller når sitt veckomål (funktion 6).
/// </summary>
public interface IGoalNotifier
{
    /// <summary>
    /// Anropas efter att ett pass har sparats. Returnerar de meddelanden användaren ska se,
    /// till exempel "Kondition är på 81 % av veckomålet". Tom lista betyder inga meddelanden.
    /// </summary>
    List<string> CheckGoals(Workout workout);
}
