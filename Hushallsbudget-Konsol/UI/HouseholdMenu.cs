using Hushallsbudget.Models;
using Hushallsbudget.Services;

namespace Hushallsbudget.UI;

/// <summary>
/// Menyval för hushållet och dess medlemmar. All lagring går via IHouseholdRepository.
/// </summary>
public class HouseholdMenu
{
    private readonly IHouseholdRepository _repository;

    public HouseholdMenu(IHouseholdRepository repository)
    {
        _repository = repository;
    }

    public void ShowHousehold()
    {
        Household household = _repository.GetHousehold();
        Console.WriteLine($"Hushåll: {household.Name}");

        List<Member> members = _repository.GetAllMembers();
        if (members.Count == 0)
        {
            Console.WriteLine("  Hushållet har inga medlemmar än.");
            return;
        }

        foreach (Member member in members)
        {
            Console.WriteLine($"  {member.Id}. {member.FirstName} {member.LastName}");
        }
    }

    public void AddMember()
    {
        // Fälten följer modellen Member. Tar ni bort ett fält ur modellen, ta bort raden här också.
        var member = new Member
        {
            FirstName = ConsoleInput.ReadRequiredText("Förnamn"),
            LastName = ConsoleInput.ReadText("Efternamn"),
            BirthDate = ConsoleInput.ReadDate("Födelsedatum"),
            HairColor = ConsoleInput.ReadText("Hårfärg"),
            HeightCm = ConsoleInput.ReadInt("Längd i cm", min: 0, max: 300),
            Religion = ConsoleInput.ReadText("Religion"),
        };

        _repository.AddMember(member);
        Console.WriteLine($"{member.FirstName} är tillagd i hushållet.");
    }
}
