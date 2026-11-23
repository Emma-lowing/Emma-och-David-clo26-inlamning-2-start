// Startpunkt för Sportstatistik (konsol). Här kopplas alla delar ihop, och sedan startar menyn.
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Configuration;
using Sportstatistik.Services;
using Sportstatistik.UI;

// Svenska tecken och svenskt format på tal och datum.
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
//   ILeagueRepository repository = new MongoLeagueRepository(connectionString);
// Er databasklass ska skapa databasen och seeda exempeldata vid första start, om den är tom.
// Här passar till exempel mönstret Factory: en klass som väljer och skapar rätt repository
// utifrån konfigurationen, så att Program.cs inte behöver känna till varje databasklass.
ILeagueRepository repository = new InMemoryLeagueRepository();

// De här ska ni bygga. Tills dess visar menyn demodata eller "Inte implementerat ännu".
IStandingsService standings = new StandingsService();
IPositionService positions = new PositionService();
ISquadLimitService squadLimits = new SquadLimitService();
IResultFormatter formatter = new ResultFormatter();

var teamMenu = new TeamMenu(repository, positions, squadLimits);
var matchMenu = new MatchMenu(repository, formatter);
var standingsMenu = new StandingsMenu(standings, formatter);
var positionMenu = new PositionMenu(positions);
var squadLimitMenu = new SquadLimitMenu(squadLimits, repository);

Console.WriteLine("Välkommen till Sportstatistik!");

new Menu("Sportstatistik", exitLabel: "Avsluta")
    .Add("1", "Lista lag", teamMenu.ListTeams)
    .Add("2", "Lägg till lag", teamMenu.AddTeam)
    .Add("3", "Registrera match", matchMenu.RegisterMatch)
    .Add("4", "Visa tabell", standingsMenu.ShowStandings)
    .Add("5", "Lägg till spelare i ett lag", teamMenu.AddPlayer)
    .Add("6", "Lista matcher", matchMenu.ListMatches)
    .Add("7", "Ta bort match", matchMenu.DeleteMatch)
    .Add("8", "Lista spelare i ett lag", teamMenu.ListPlayers)
    .Add("9", "Positioner", positionMenu.Run)
    .Add("10", "Truppgränser", squadLimitMenu.Run)
    .Run();

Console.WriteLine("Hej då!");
