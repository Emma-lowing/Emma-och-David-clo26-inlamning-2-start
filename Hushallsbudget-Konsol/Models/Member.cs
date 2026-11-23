namespace Hushallsbudget.Models;

/// <summary>
/// En medlem i hushållet.
/// Fälten kommer från beställarens gamla Excel-ark. Utvärdera vart och ett i README:ns
/// avsnitt "Fält och data", och ta bort de fält som programmet inte behöver.
/// </summary>
public class Member
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public DateOnly BirthDate { get; set; }
    public string HairColor { get; set; } = "";
    public int HeightCm { get; set; }
    public string Religion { get; set; } = "";
}
