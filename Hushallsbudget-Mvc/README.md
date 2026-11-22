# Hushallsbudget, mvc

ASP.NET Core MVC. Ni skriver modeller i `Models/`, tjänster i `Services/`, databas i `Data/`, och kontroller och vyer i `Controllers/` och `Views/`.

## Så kör du

1. Gör en `dotnet user-secrets init` och sätt `DB_CONNECTION` (se nedan).
2. Kör: `dotnet run --project Hushallsbudget-Mvc`

Vid första start skapar programmet databasen och seedar den, om den är tom.

## Connectionstring (user-secrets)

Connectionstringen sparas i en secrets-fil på er dator. Den ligger aldrig i koden eller i repot.

```bash
dotnet user-secrets init --project Hushallsbudget-Mvc
dotnet user-secrets set "DB_CONNECTION" "<er connectionstring>" --project Hushallsbudget-Mvc
```

Kontrollera med `dotnet user-secrets list --project Hushallsbudget-Mvc`. Kommandot visar värdet, så dela inte skärmbilden.
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
