# Recept, blazor

Blazor Web App. Ni skriver modeller i `Models/`, tjänster i `Services/`, databas i `Data/`, och komponenter i `Components/`.

## Så kör du

1. Gör en `dotnet user-secrets init` och sätt `DB_CONNECTION` (se nedan).
2. Kör: `dotnet run --project Recept-Blazor`

Vid första start skapar programmet databasen och seedar den, om den är tom.

## Connectionstring (user-secrets)

Connectionstringen sparas i en secrets-fil på er dator. Den ligger aldrig i koden eller i repot.

```bash
dotnet user-secrets init --project Recept-Blazor
dotnet user-secrets set "DB_CONNECTION" "<er connectionstring>" --project Recept-Blazor
```

Kontrollera med `dotnet user-secrets list --project Recept-Blazor`. Kommandot visar värdet, så dela inte skärmbilden.
Läraren kör er kod mot sin egen Docker-server och sätter connectionstringen själv. Er kod måste därför bygga och seeda hela databasen vid första start.

---


Ett hushåll sparar recept med ingredienser, planerar veckans måltider, får en inköpslista och får varning när en kategori används för ofta. Beställningen finns i uppgiftsdokumentet `bestallning_recept.md`.

## Minsta menyval (måste finnas)

```
1. Lista recept
2. Lägg till recept (namn, portioner, ingredienser)
3. Visa ett recept
4. Ta bort recept
5. Sök recept på ingrediens
6. Lägg till kock
0. Avsluta
```

## Minimal modell (måste byggas ut)

- `Recept`: `Id`, `Namn`, `Portioner`
- `Ingrediens`: `Namn`, `Mangd`, `Enhet`
- `Kock`: `Id`, `Fornamn`, `Efternamn`, `Fodelsedatum`, `HairColor`, `HeightCm`, `Religion` (den som skapat receptet)

Lägg till: kategorier, veckoplan, inköpslista, gräns per kategori, och det som beställaren vill ha.
