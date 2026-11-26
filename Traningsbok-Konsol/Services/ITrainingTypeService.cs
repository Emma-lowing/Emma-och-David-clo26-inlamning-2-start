using Traningsbok.Models;

namespace Traningsbok.Services;

public interface ITrainingTypeService
{
    List<TrainingType> GetAll();
    void Add(string name);
    void Delete(int trainingTypeId);
    void SetWeeklyGoal(int athleteId, int trainingTypeId, int minutes);
}