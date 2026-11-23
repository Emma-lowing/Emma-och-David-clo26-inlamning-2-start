using Traningsbok.Models;

namespace Traningsbok.Services;

/// <summary>
/// Träningstyper och veckomål per träningstyp.
/// </summary>
public interface ITrainingTypeService
{
    List<TrainingType> GetAll();
    void Add(string name);

    /// <summary>Tar bort en träningstyp. Pass med typen flyttas till "Övrigt" (önskemål 3).</summary>
    void Delete(int trainingTypeId);

    /// <summary>Veckomålet i minuter för träningstypen (funktion 5).</summary>
    void SetWeeklyGoal(int trainingTypeId, int minutes);
}
