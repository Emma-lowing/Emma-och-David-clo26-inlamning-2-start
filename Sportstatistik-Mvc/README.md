# Sportstatistik, mvc

ASP.NET Core MVC. Ni skriver modeller i `Models/`, tjänster i `Services/`, databas i `Data/`, och kontroller och vyer i `Controllers/` och `Views/`.

## Så kör du

1. Gör en `dotnet user-secrets init` och sätt `DB_CONNECTION` (se nedan).
2. Kör: `dotnet run --project Sportstatistik-Mvc`

Vid första start skapar programmet databasen och seedar den, om den är tom.

## Connectionstring (user-secrets)

Connectionstringen sparas i en secrets-fil på er dator. Den ligger aldrig i koden eller i repot.

```bash
dotnet user-secrets init --project Sportstatistik-Mvc
dotnet user-secrets set "DB_CONNECTION" "<er connectionstring>" --project Sportstatistik-Mvc
```

Kontrollera med `dotnet user-secrets list --project Sportstatistik-Mvc`. Kommandot visar värdet, så dela inte skärmbilden.
Läraren kör er kod mot sin egen Docker-server och sätter connectionstringen själv. Er kod måste därför bygga och seeda hela databasen vid första start.

---


En liten tabell och matchregistrering för ett seriespel, till exempel fotboll.

## Minsta menyval (måste finnas)

```
1. Lista lag
2. Lägg till lag
3. Registrera match (hemmalag, bortalag, mål hemma, mål borta)
4. Visa tabell (poäng, vinster, oavgjorda, förluster, målskillnad)
5. Lägg till spelare i ett lag
0. Avsluta
```

## Minimal modell (måste byggas ut)

- `Lag`: `Id`, `Namn`
- `Match`: `Id`, `Hemmalag`, `Bortalag`, `MalHemma`, `MalBorta`
- `Spelare`: `Id`, `Position`, `Fornamn`, `Efternamn`, `Fodelsedatum`, `HairColor`, `HeightCm`, `Religion` (hör till ett lag)

Lägg till: säsong, spelare, och en tabell som ni själva sorterar och motiverar.
