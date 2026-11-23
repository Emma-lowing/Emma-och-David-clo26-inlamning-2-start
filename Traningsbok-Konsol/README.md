# Traningsbok, konsol

Konsolprogram. **Gränssnittet är färdigt.** Ni bygger det som ligger bakom: databas, repository och logik. Se avsnittet [Så är koden uppbyggd](#så-är-koden-uppbyggd).

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

---

## Så är koden uppbyggd

Programmet går att köra direkt, utan databas. Menyerna i `UI/` är färdiga och ska inte behöva ändras, förutom där en kommentar säger det.

| Mapp | Innehåll | Vad ni gör |
|------|----------|------------|
| `UI/` | Menyer och inmatning. Pratar bara med interface i `Services/`. | Lägger till fält i formulären när ni bygger ut modellerna. |
| `Models/` | Minimala modeller, bara det som README:n kräver. | Bygger ut med fälten beställningen kräver, och tar bort de som inte behövs. |
| `Services/` | Interface och klasser som menyn anropar. | Bygger allt som kastar `NotImplementedException`. |
| `Data/` | Tom. | Er databaskoppling. |

### Vad fungerar redan?

- **Menyval 1–3 och 5–8** (pass, övningar och utövare) fungerar mot `InMemoryWorkoutRepository`. Den är en låtsasdatabas: data ligger i listor och försvinner när programmet stängs.
- **Träningstyper, veckoöversikt och statistik för en övning** visar demodata från `TrainingTypeService` och `StatisticsService`: hårdkodade värden som inte sparas och inte räknas fram. Allt som ändrar något (lägga till eller ta bort träningstyp, sätta veckomål) kastar `NotImplementedException`, och då visas *"Inte implementerat ännu"*.
- **Meddelanden vid veckomålet** (80 % och 100 %): passet sparas, men `GoalNotifier` kastar `NotImplementedException`, så efter varje nytt pass visas en varning om att det inte är implementerat ännu.
- **Demodatan visar hur sidorna ska se ut** när ni är klara, men den är hårdkodad. Ersätt allt som är märkt `DEMO` med riktig logik.
- **Två saker är fel med flit:** vikt visas som `62,50 kg` i stället för `62,5 kg`, och träningstid som `65 min` i stället för `1 h 05 min` (önskemål 1). Båda rättas i `WorkoutFormatter`.

Sök efter `Du behöver bygga ut detta mer` i koden för att hitta alla ställen ni ska bygga vidare på.

### Byt låtsasdatabasen mot en riktig

1. Skriv en klass, till exempel `MongoWorkoutRepository` eller `EfWorkoutRepository`, som implementerar `IWorkoutRepository`.
2. Låt den skapa databasen och seeda exempeldata vid första start, om den är tom.
3. Byt raden `new InMemoryWorkoutRepository()` i `Program.cs` mot er klass.
4. Ta bort `InMemoryWorkoutRepository.cs` när ni inte behöver den längre.

Felaktig inmatning som upptäcks i era services: kasta `ArgumentException` med ett meddelande på svenska. Menyn visar meddelandet i stället för att krascha.

### Svenska namn i README, engelska i koden

| README | Koden |
|--------|-------|
| `Pass` (`Datum`, `Typ`) | `Workout` (`Date`, `Type`) |
| `Ovning` (`Namn`, `Set`, `Reps`, `Vikt`) | `Exercise` (`Name`, `Sets`, `Reps`, `Weight`) |
| `Utovare` (`Fornamn`, `Efternamn`, `Fodelsedatum`, `HairColor`, `HeightCm`, `Religion`) | `Athlete` (`FirstName`, `LastName`, `BirthDate`, `HairColor`, `HeightCm`, `Religion`) |
| träningstyp | `TrainingType` |
| veckoöversikt | `WeekOverview` |
