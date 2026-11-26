using Traningsbok.Models;
using Traningsbok.Services;

namespace Traningsbok.UI;

public class AthleteSelector
{
    private readonly IWorkoutRepository _repository;
    private readonly AthleteSession _session;

    public AthleteSelector(IWorkoutRepository repository,
        AthleteSession session)
    {
        _repository = repository;
        _session = session;
    }

    public void SelectAthlete()
    {
        List<Athlete> athletes = _repository.GetAllAthletes();

        if (athletes.Count == 0)
        {
            Console.WriteLine("Det finns inga utövare.");
            return;
        }

        foreach (Athlete athlete in athletes)
        {
            Console.WriteLine($"{athlete.Id}. {athlete.FirstName} {athlete.LastName}");
        }

        int id = ConsoleInput.ReadInt("Välj utövare", min: 1);

        if (!athletes.Any(a => a.Id == id))
        {
            Console.WriteLine("Utövaren finns inte.");
            return;
        }

        _session.ActiveAthleteId = id;

        Console.WriteLine("Aktiv utövare har valts.");
    }
}