namespace Traningsbok.Services;
/// <summary>
/// Håller reda på vilken utövare som är aktiv
/// så att alla menyer arbetar med samma utövare.
/// </summary>
public class AthleteSession
{
    public int? ActiveAthleteId { get; set; }

    public int RequireActiveAthlete()
    {
        return ActiveAthleteId
            ?? throw new ArgumentException(
                "Du måste välja en aktiv utövare först.");
    }
}