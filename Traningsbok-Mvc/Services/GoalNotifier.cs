using Traningsbok.Models;

namespace Traningsbok.Services;

/// <summary>
/// Ska byggas av er. CheckGoals kastar NotImplementedException, och sidan visar då
/// "Inte implementerat ännu" direkt efter att passet har sparats.
/// </summary>
public class GoalNotifier : IGoalNotifier
{
    // Du behöver bygga ut detta mer: räkna ihop veckans minuter för passets träningstyp,
    // och returnera ett meddelande vid 80 % och ett annat vid 100 % av veckomålet.
    // Samma 80 %-meddelande får inte visas igen förrän veckan byts (önskemål 4).
    // Här passar till exempel mönstret Observer.
    public List<string> CheckGoals(Workout workout) => throw new NotImplementedException();
}
