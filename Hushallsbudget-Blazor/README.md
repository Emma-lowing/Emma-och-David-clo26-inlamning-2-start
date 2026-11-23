# Hushallsbudget, blazor

Blazor (Interactive Server). **Gränssnittet är färdigt:** sidor och formulär. Ni bygger det som ligger bakom: databas, repository och logik. Se avsnittet [Så är koden uppbyggd](#så-är-koden-uppbyggd).

## Så kör du

1. Gör en `dotnet user-secrets init` och sätt `DB_CONNECTION` (se nedan).
2. Kör: `dotnet run --project Hushallsbudget-Blazor`

Vid första start skapar programmet databasen och seedar den, om den är tom.

## Connectionstring (user-secrets)

Connectionstringen sparas i en secrets-fil på er dator. Den ligger aldrig i koden eller i repot.

```bash
dotnet user-secrets init --project Hushallsbudget-Blazor
dotnet user-secrets set "DB_CONNECTION" "<er connectionstring>" --project Hushallsbudget-Blazor
```

Kontrollera med `dotnet user-secrets list --project Hushallsbudget-Blazor`. Kommandot visar värdet, så dela inte skärmbilden.
Läraren kör er kod mot sin egen Docker-server och sätter connectionstringen själv. Er kod måste därför bygga och seeda hela databasen vid första start.

---


Ett hushåll registrerar inkomster och utgifter, ser månadsöversikt och får varning när en kategori går över budget. Beställningen finns i uppgiftsdokumentet `bestallning_hushallsbudget.md`.

## Minsta menyval (måste finnas)

```
1. Visa transaktioner för en månad
2. Lägg till transaktion (belopp, datum, kategori, anteckning)
3. Ta bort transaktion
4. Visa månadsöversikt (inkomst, utgift, saldo)
5. Lägg till medlem i hushållet
0. Avsluta
```

## Minimal modell (måste byggas ut)

- `Hushall`: `Id`, `Namn`
- `Medlem`: `Id`, `Fornamn`, `Efternamn`, `Fodelsedatum`, `HairColor`, `HeightCm`, `Religion` (1–5 per hushåll)
- `Transaktion`: `Id`, `Belopp`, `Datum`, `Kategori`, `Anteckning`

Lägg till: kategorier, budget per kategori, och det som beställaren vill ha.

---

## Så är koden uppbyggd

Programmet går att köra direkt, utan databas. Sidorna är färdiga och ska inte behöva ändras, förutom där en kommentar säger det.

| Mapp | Innehåll | Vad ni gör |
|------|----------|------------|
| `Components/Pages/` | Sidor och formulär. Pratar bara med interface i `Services/`. | Lägger till fält i formulären när ni bygger ut modellerna. |
| `Components/` (`PageBase`, `InputParser`, `Shared/`) | Gör om fel till meddelanden, bland annat "Inte implementerat ännu", och tolkar inmatning. | Inget. |
| `Models/` | Minimala modeller, bara det som README:n kräver. | Bygger ut med fälten beställningen kräver, och tar bort de som inte behövs. |
| `Services/` | Interface och klasser som sidorna anropar. | Bygger allt som kastar `NotImplementedException`. |
| `Data/` | Tom. | Er databaskoppling. |

### Vad fungerar redan?

- **Transaktioner och hushållet** (lista per månad, lägga till, ta bort, lägga till medlem) fungerar mot `InMemoryHouseholdRepository`. Den är en låtsasdatabas: data ligger i listor och försvinner när programmet stängs.
- **Månadsöversikt, kategorier och budget** visar demodata från `MonthlyOverviewService`, `CategoryService` och `BudgetService`: hårdkodade siffror som inte sparas och som är likadana för alla månader. De stämmer med låtsasdatabasens transaktioner för november 2026, så ni kan jämföra med er egen uträkning. Allt som ändrar något (lägga till eller ta bort kategori, sätta budget) kastar `NotImplementedException`, och då visas *"Inte implementerat ännu"*.
- **Budgetvarningar** (`BudgetWarningService`) kastar `NotImplementedException`. Transaktionen sparas ändå, men i stället för en varning visas *"Budgetvarningar är inte implementerade ännu"*.
- **Demodatan visar hur sidorna ska se ut** när ni är klara, men den är hårdkodad. Ersätt allt som är märkt `DEMO` med riktig logik.
- **En sak är fel med flit:** belopp visas som `1249,50 kr` i stället för `1 249,50 kr`, och en utgift visas med minustecken (önskemål 1). Det rättas i `MoneyFormatter`.
- **Inkomst eller utgift** skiljs i låtsasdatan åt med minustecken i `Amount`, som i Carlos Excel-ark. Det är ett val ni själva ska utvärdera.

Sök efter `Du behöver bygga ut detta mer` i koden för att hitta alla ställen ni ska bygga vidare på.

### Byt låtsasdatabasen mot en riktig

1. Skriv en klass, till exempel `MongoHouseholdRepository` eller `EfHouseholdRepository`, som implementerar `IHouseholdRepository`.
2. Låt den skapa databasen och seeda exempeldata vid första start, om den är tom.
3. Byt registreringen av `InMemoryHouseholdRepository` i `Program.cs` mot er klass.
4. Ta bort `InMemoryHouseholdRepository.cs` när ni inte behöver den längre.

Felaktig inmatning som upptäcks i era services: kasta `ArgumentException` med ett meddelande på svenska. Sidan visar meddelandet i stället för att krascha.

### Svenska namn i README, engelska i koden

| README | Koden |
|--------|-------|
| `Hushall` (`Namn`) | `Household` (`Name`) |
| `Medlem` (`Fornamn`, `Efternamn`, `Fodelsedatum`, `HairColor`, `HeightCm`, `Religion`) | `Member` (`FirstName`, `LastName`, `BirthDate`, `HairColor`, `HeightCm`, `Religion`) |
| `Transaktion` (`Belopp`, `Datum`, `Kategori`, `Anteckning`) | `Transaction` (`Amount`, `Date`, `Category`, `Note`) |
