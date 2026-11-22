// Startpunkt för Hushallsbudget (konsol).
// Connectionstring läses från user-secrets, aldrig från koden.
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

string? connectionString = config["DB_CONNECTION"];

// TODO: Skapa databasen och seeda den (se Data/README.md)
// TODO: Starta menyn (se UI/README.md)

Console.WriteLine("Hushallsbudget startar här. Ta bort den här raden när ni börjat bygga.");
