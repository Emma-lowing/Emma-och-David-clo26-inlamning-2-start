using Traningsbok.Models;
using Traningsbok.Services;

namespace Traningsbok.UI;

/// <summary>
/// Menyval för träningstyper och veckomål. Logiken ligger i ITrainingTypeService.
/// </summary>
public class TrainingTypeMenu
{
    private readonly AthleteSession _session;
    private readonly ITrainingTypeService _trainingTypes;

    public TrainingTypeMenu(ITrainingTypeService trainingTypes,
     AthleteSession session)
    {
        _trainingTypes = trainingTypes;
        _session = session;
    }

    public void Run()
    {
        new Menu("Träningstyper")
            .Add("1", "Lista träningstyper", ListTrainingTypes)
            .Add("2", "Lägg till träningstyp", AddTrainingType)
            .Add("3", "Ta bort träningstyp", DeleteTrainingType)
            .Add("4", "Sätt veckomål", SetWeeklyGoal)
            .Run();
    }

    private void ListTrainingTypes()
    {
        foreach (TrainingType trainingType in _trainingTypes.GetAll())
        {
            // Du behöver bygga ut detta mer: visa veckomålet när TrainingType har det fältet.
            Console.WriteLine($"{trainingType.Id}. {trainingType.Name}");
        }
    }

    private void AddTrainingType()
    {
        // Ingen kontroll av tom text här: den regeln hör hemma i TrainingTypeService.
        string name = ConsoleInput.ReadText("Namn på träningstypen");
        _trainingTypes.Add(name);
        Console.WriteLine($"Träningstypen \"{name}\" är tillagd.");
    }

    private void DeleteTrainingType()
    {
        ListTrainingTypes();
        int id = ConsoleInput.ReadInt("Träningstypens nummer", min: 1);

        if (!ConsoleInput.Confirm("Passen med träningstypen flyttas till \"Övrigt\". Fortsätta?"))
        {
            Console.WriteLine("Inget togs bort.");
            return;
        }

        _trainingTypes.Delete(id);
        Console.WriteLine("Träningstypen är borttagen.");
    }

    private void SetWeeklyGoal()
    {
        ListTrainingTypes();
        int id = ConsoleInput.ReadInt("Träningstypens nummer", min: 1);
        // En vecka har 10 080 minuter.
        int minutes = ConsoleInput.ReadInt("Veckomål i minuter", min: 1, max: 10080);

        _trainingTypes.SetWeeklyGoal(
    _session.RequireActiveAthlete(), id, minutes);
        Console.WriteLine("Veckomålet är sparat.");
    }
}
