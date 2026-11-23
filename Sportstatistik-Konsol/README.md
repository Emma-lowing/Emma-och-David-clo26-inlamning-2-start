# Sportstatistik, konsol

Konsolprogram. Ni skriver meny och inmatning i `Program.cs` och klasser i mapparna `Data/`, `Models/`, `Services/` och `UI/`.

## Så kör du

1. Gör en `dotnet user-secrets init` och sätt `DB_CONNECTION` (se nedan).
2. Kör: `dotnet run --project Sportstatistik-Konsol`

Vid första start skapar programmet databasen och seedar den, om den är tom.

## Connectionstring (user-secrets)

Connectionstringen sparas i en secrets-fil på er dator. Den ligger aldrig i koden eller i repot.

```bash
dotnet user-secrets init --project Sportstatistik-Konsol
dotnet user-secrets set "DB_CONNECTION" "<er connectionstring>" --project Sportstatistik-Konsol
```

Kontrollera med `dotnet user-secrets list --project Sportstatistik-Konsol`. Kommandot visar värdet, så dela inte skärmbilden.
Läraren kör er kod mot sin egen Docker-server och sätter connectionstringen själv. Er kod måste därför bygga och seeda hela databasen vid första start.

---


En korpserie registrerar lag, spelare och matcher, ser tabellen för en säsong och får varning när ett lag närmar sig truppgränsen. Beställningen finns i uppgiftsdokumentet `bestallning_sportstatistik.md`.

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

Lägg till: säsong, positioner, truppgräns per lag, och det som beställaren vill ha.
