using Traningsbok.Models;

namespace Traningsbok.Services;

/// <summary>
/// Ska byggas av er. GetAll visar demodata så att sidan går att titta på.
/// Övriga metoder kastar NotImplementedException, och menyn visar då "Inte implementerat ännu".
/// </summary>
public class TrainingTypeService : ITrainingTypeService
{
    private readonly IWorkoutRepository _repository;

    public TrainingTypeService(IWorkoutRepository repository)
    {
        _repository = repository;
    }
    // DEMO: en fast lista som inte sparas någonstans. Ersätt med träningstyper från databasen.
    // Standardlistan ska alltid finnas: Styrka, Kondition, Rörlighet, Bollsport och Övrigt.
    // "Klättring" visar hur en egen träningstyp ser ut i listan.
    public List<TrainingType> GetAll()
     => _repository.GetAllTrainingTypes();

    // Tom träningstyp ska ge ett tydligt felmeddelande. Kasta ArgumentException, så visar menyn texten.
    public void Add(string name) => throw new NotImplementedException();

    public void Delete(int trainingTypeId) => throw new NotImplementedException();

    public void SetWeeklyGoal(int athleteId, int trainingTypeId, int minutes)
    => throw new NotImplementedException();
}
