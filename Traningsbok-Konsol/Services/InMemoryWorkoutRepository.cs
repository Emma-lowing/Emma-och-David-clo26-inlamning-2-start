using Traningsbok.Models;

namespace Traningsbok.Services;

public class InMemoryWorkoutRepository : IWorkoutRepository
{
    private readonly List<Workout> _workouts = new();
    private readonly List<Athlete> _athletes = new();
    private readonly List<TrainingType> _trainingTypes = new();
    private readonly List<WeeklyGoal> _weeklyGoals = new();
    private readonly List<GoalNotification> _notifications = new();

    private int _nextWorkoutId = 1;
    private int _nextExerciseId = 1;
    private int _nextAthleteId = 1;
    private int _nextTrainingTypeId = 1;
    private int _nextWeeklyGoalId = 1;
    private int _nextNotificationId = 1;

    public InMemoryWorkoutRepository()
    {
        Seed();
    }

    // Workouts

    public List<Workout> GetAllWorkouts()
        => _workouts.OrderBy(w => w.Date).ToList();

    public Workout? GetWorkout(int id)
        => _workouts.FirstOrDefault(w => w.Id == id);

    public void AddWorkout(Workout workout)
    {
        workout.Id = _nextWorkoutId++;

        foreach (Exercise exercise in workout.Exercises)
        {
            exercise.Id = _nextExerciseId++;
        }

        _workouts.Add(workout);
    }

    public void DeleteWorkout(int id)
        => _workouts.RemoveAll(w => w.Id == id);

    public void UpdateWorkout(Workout workout)
    {
        int index = _workouts.FindIndex(w => w.Id == workout.Id);

        if (index == -1)
            throw new ArgumentException("Passet finns inte.");

        _workouts[index] = workout;
    }

    public void AddExercise(int workoutId, Exercise exercise)
    {
        Workout workout = GetWorkout(workoutId)
            ?? throw new ArgumentException("Passet finns inte.");

        exercise.Id = _nextExerciseId++;
        workout.Exercises.Add(exercise);
    }

    // Athletes

    public List<Athlete> GetAllAthletes()
        => _athletes.ToList();

    public Athlete? GetAthlete(int id)
        => _athletes.FirstOrDefault(a => a.Id == id);

    public void AddAthlete(Athlete athlete)
    {
        if (_athletes.Count >= 5)
            throw new ArgumentException("Max fem utövare är tillåtna.");

        athlete.Id = _nextAthleteId++;
        _athletes.Add(athlete);
    }

    // Training types

    public List<TrainingType> GetAllTrainingTypes()
        => _trainingTypes.ToList();

    public TrainingType? GetTrainingType(int id)
        => _trainingTypes.FirstOrDefault(t => t.Id == id);

    public void AddTrainingType(TrainingType trainingType)
    {
        trainingType.Id = _nextTrainingTypeId++;
        _trainingTypes.Add(trainingType);
    }

    public void DeleteTrainingType(int id)
        => _trainingTypes.RemoveAll(t => t.Id == id);

    // Weekly goals

    public List<WeeklyGoal> GetAllWeeklyGoals()
        => _weeklyGoals.ToList();

    public WeeklyGoal? GetWeeklyGoal(int athleteId, int trainingTypeId)
        => _weeklyGoals.FirstOrDefault(g =>
            g.AthleteId == athleteId &&
            g.TrainingTypeId == trainingTypeId);

    public void SaveWeeklyGoal(WeeklyGoal goal)
    {
        WeeklyGoal? existing = GetWeeklyGoal(
            goal.AthleteId, goal.TrainingTypeId);

        if (existing is null)
        {
            goal.Id = _nextWeeklyGoalId++;
            _weeklyGoals.Add(goal);
        }
        else
        {
            existing.TargetMinutes = goal.TargetMinutes;
            goal.Id = existing.Id;
        }
    }

    public void DeleteWeeklyGoal(int id)
        => _weeklyGoals.RemoveAll(g => g.Id == id);

    // Goal notifications

    public List<GoalNotification> GetAllGoalNotifications()
        => _notifications.ToList();

    public void AddGoalNotification(GoalNotification notification)
    {
        notification.Id = _nextNotificationId++;
        _notifications.Add(notification);
    }

    public void DeleteGoalNotifications(int weeklyGoalId)
        => _notifications.RemoveAll(n =>
            n.WeeklyGoalId == weeklyGoalId);

    // Example data

    private void Seed()
    {
        var athlete = new Athlete
        {
            FirstName = "Kim",
            LastName = "Exempelsson"
        };

        AddAthlete(athlete);

        var strength = new TrainingType { Name = "Styrka" };
        var cardio = new TrainingType { Name = "Kondition" };
        var mobility = new TrainingType { Name = "Rörlighet" };

        AddTrainingType(strength);
        AddTrainingType(cardio);
        AddTrainingType(mobility);
        AddTrainingType(new TrainingType { Name = "Bollsport" });

        AddTrainingType(new TrainingType
        {
            Name = "Övrigt",
            IsProtected = true
        });

        AddWorkout(new Workout
        {
            Date = new DateOnly(2026, 11, 12),
            AthleteId = athlete.Id,
            TrainingTypeId = strength.Id,
            TimeInMinutes = 65,
            Exercises =
            {
                new Exercise
                {
                    Name = "Knäböj",
                    Sets = 3,
                    Reps = 8,
                    Weight = 62.5m
                },
                new Exercise
                {
                    Name = "Marklyft",
                    Sets = 3,
                    Reps = 5,
                    Weight = 80m
                }
            }
        });

        AddWorkout(new Workout
        {
            Date = new DateOnly(2026, 11, 15),
            AthleteId = athlete.Id,
            TrainingTypeId = cardio.Id,
            TimeInMinutes = 30
        });

        AddWorkout(new Workout
        {
            Date = new DateOnly(2026, 11, 18),
            AthleteId = athlete.Id,
            TrainingTypeId = strength.Id,
            TimeInMinutes = 60,
            Exercises =
            {
                new Exercise
                {
                    Name = "Knäböj",
                    Sets = 4,
                    Reps = 6,
                    Weight = 65m
                }
            }
        });

        AddWorkout(new Workout
        {
            Date = new DateOnly(2026, 11, 20),
            AthleteId = athlete.Id,
            TrainingTypeId = mobility.Id,
            TimeInMinutes = 20
        });

        SaveWeeklyGoal(new WeeklyGoal
        {
            AthleteId = athlete.Id,
            TrainingTypeId = cardio.Id,
            TargetMinutes = 100
        });
    }
}