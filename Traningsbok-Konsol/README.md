# Traningsbok, konsol

Konsolprogram. Ni skriver meny och inmatning i `Program.cs` och klasser i mapparna `Data/`, `Models/`, `Services/` och `UI/`.

## Så kör du

1. Gör en `dotnet user-secrets init` och sätt `DB_CONNECTION` (se nedan).
2. Kör: `dotnet run --project Traningsbok-Konsol`

Vid första start skapar programmet databasen och seedar den, om den är tom.

## Connectionstring (user-secrets)

Connectionstringen sparas i en secrets-fil på er dator. Den ligger aldrig i koden eller i repot.

```bash
dotnet user-secrets init --project Traningsbok-Konsol
dotnet user-secrets set "DB_CONNECTION" "<er connectionstring>" --project Traningsbok-Konsol
```

Kontrollera med `dotnet user-secrets list --project Traningsbok-Konsol`. Kommandot visar värdet, så dela inte skärmbilden.
Läraren kör er kod mot sin egen Docker-server och sätter connectionstringen själv. Er kod måste därför bygga och seeda hela databasen vid första start.

---


En utövare registrerar pass och övningar, ser veckoöversikt och får ett meddelande när ett veckomål närmar sig eller är nått. Beställningen finns i uppgiftsdokumentet `bestallning_traningsbok.md`.

## Minsta menyval (måste finnas)

```
1. Lista pass
2. Lägg till pass (datum, typ)
3. Lägg till övning till ett pass (namn, set, reps, vikt)
4. Visa statistik för en övning (högsta vikt, totalt antal set)
5. Lägg till utövare
0. Avsluta
```

## Minimal modell (måste byggas ut)

- `Pass`: `Id`, `Datum`, `Typ`
- `Ovning`: `Id`, `Namn`, `Set`, `Reps`, `Vikt`
- `Utovare`: `Id`, `Fornamn`, `Efternamn`, `Fodelsedatum`, `HairColor`, `HeightCm`, `Religion`

Lägg till: träningstyper, veckomål per träningstyp, och det som beställaren vill ha.
