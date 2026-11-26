using Traningsbok.Models;

namespace Traningsbok.Services;

public interface IWorkoutRepository
{
    // Workouts
    List<Workout> GetAllWorkouts();
    Workout? GetWorkout(int id);
    void AddWorkout(Workout workout);
    void DeleteWorkout(int id);
    void UpdateWorkout(Workout workout);
    void AddExercise(int workoutId, Exercise exercise);

    // Athletes
    List<Athlete> GetAllAthletes();
    Athlete? GetAthlete(int id);
    void AddAthlete(Athlete athlete);

    // Training types
    List<TrainingType> GetAllTrainingTypes();
    TrainingType? GetTrainingType(int id);
    void AddTrainingType(TrainingType trainingType);
    void DeleteTrainingType(int id);

    // Weekly goals
    List<WeeklyGoal> GetAllWeeklyGoals();
    WeeklyGoal? GetWeeklyGoal(int athleteId, int trainingTypeId);
    void SaveWeeklyGoal(WeeklyGoal goal);
    void DeleteWeeklyGoal(int id);

    // Goal notifications
    List<GoalNotification> GetAllGoalNotifications();
    void AddGoalNotification(GoalNotification notification);
    void DeleteGoalNotifications(int weeklyGoalId);
}