// Startpunkt för Traningsbok (konsol). Här kopplas alla delar ihop, och sedan startar menyn.
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Configuration;
using Traningsbok.Services;
using Traningsbok.UI;

// Svenska tecken och svenskt talformat (62,5 i stället för 62.5).
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("sv-SE");
CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("sv-SE");

// Connectionstring läses från user-secrets, aldrig från koden.
var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

string? connectionString = config["DB_CONNECTION"];

// Byt den här raden när ni har en riktig databas, till exempel:
//   IWorkoutRepository repository = new MongoWorkoutRepository(connectionString);
// Er databasklass ska skapa databasen och seeda exempeldata vid första start, om den är tom.
// Här passar till exempel mönstret Factory: en klass som väljer och skapar rätt repository
// utifrån konfigurationen, så att Program.cs inte behöver känna till varje databasklass.
IWorkoutRepository repository = new InMemoryWorkoutRepository();
var athleteSession = new AthleteSession();
var athleteSelector = new AthleteSelector(repository, athleteSession);

// De här fyra ska ni bygga. Tills dess visar menyn demodata eller "Inte implementerat ännu".
ITrainingTypeService trainingTypes = new TrainingTypeService(repository);
IGoalNotifier goals = new GoalNotifier(repository);
IStatisticsService statistics = new StatisticsService(repository);
IWorkoutFormatter formatter = new WorkoutFormatter();

var workoutMenu = new WorkoutMenu(repository, trainingTypes, goals, formatter, athleteSession);

var trainingTypeMenu = new TrainingTypeMenu(trainingTypes, athleteSession);

var statisticsMenu = new StatisticsMenu(statistics, formatter, athleteSession);

Console.WriteLine("Välkommen till Träningsbok!");

new Menu("Träningsbok", exitLabel: "Avsluta",
    statusText: athleteSelector.GetActiveAthleteText)
    .Add("1", "Lista pass", workoutMenu.ListWorkouts)
    .Add("2", "Lägg till pass", workoutMenu.AddWorkout)
    .Add("3", "Lägg till övning till ett pass", workoutMenu.AddExercise)
    .Add("4", "Visa statistik för en övning", statisticsMenu.ShowExerciseStats)
    .Add("5", "Lägg till utövare", workoutMenu.AddAthlete)
    .Add("6", "Lista utövare", workoutMenu.ListAthletes)
    .Add("7", "Visa ett pass", workoutMenu.ShowWorkout)
    .Add("8", "Ta bort pass", workoutMenu.DeleteWorkout)
    .Add("9", "Träningstyper och veckomål", trainingTypeMenu.Run)
    .Add("10", "Veckoöversikt", statisticsMenu.ShowWeekOverview)
    .Add("11", "Byt aktiv utövare", athleteSelector.SelectAthlete)
    .Run();

Console.WriteLine("Hej då!");
