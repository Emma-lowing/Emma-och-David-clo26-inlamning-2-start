using Traningsbok.Models;

namespace Traningsbok.Services;

/// <summary>
/// Ska byggas av er. GetAll visar demodata så att sidan går att titta på.
/// Övriga metoder kastar NotImplementedException, och sidan visar då "Inte implementerat ännu".
/// </summary>
public class TrainingTypeService : ITrainingTypeService
{
    // DEMO: en fast lista som inte sparas någonstans. Ersätt med träningstyper från databasen.
    // Standardlistan ska alltid finnas: Styrka, Kondition, Rörlighet, Bollsport och Övrigt.
    // "Klättring" visar hur en egen träningstyp ser ut i listan.
    public List<TrainingType> GetAll() =>
    [
        new TrainingType { Id = 1, Name = "Styrka" },
        new TrainingType { Id = 2, Name = "Kondition" },
        new TrainingType { Id = 3, Name = "Rörlighet" },
        new TrainingType { Id = 4, Name = "Bollsport" },
        new TrainingType { Id = 5, Name = "Övrigt" },
        new TrainingType { Id = 6, Name = "Klättring" },
    ];

    // Tom träningstyp ska ge ett tydligt felmeddelande. Kasta ArgumentException, så visar sidan texten.
    public void Add(string name) => throw new NotImplementedException();

    public void Delete(int trainingTypeId) => throw new NotImplementedException();

    public void SetWeeklyGoal(int trainingTypeId, int minutes) => throw new NotImplementedException();
}
