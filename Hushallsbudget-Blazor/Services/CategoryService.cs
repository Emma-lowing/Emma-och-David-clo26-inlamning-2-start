using Hushallsbudget.Models;

namespace Hushallsbudget.Services;

/// <summary>
/// Ska byggas av er. GetAll visar demodata så att sidan går att titta på.
/// Övriga metoder kastar NotImplementedException, och sidan visar då "Inte implementerat ännu".
/// </summary>
public class CategoryService : ICategoryService
{
    // DEMO: en fast lista som inte sparas någonstans. Ersätt med kategorier från databasen.
    // Standardlistan ska alltid finnas: Boende, Mat, Transport, El och internet, Nöje, Hälsa och Övrigt.
    // "Lön" är ett exempel på en egen kategori som användaren har lagt till.
    public List<Category> GetAll() =>
    [
        new Category { Id = 1, Name = "Boende" },
        new Category { Id = 2, Name = "Mat" },
        new Category { Id = 3, Name = "Transport" },
        new Category { Id = 4, Name = "El och internet" },
        new Category { Id = 5, Name = "Nöje" },
        new Category { Id = 6, Name = "Hälsa" },
        new Category { Id = 7, Name = "Övrigt" },
        new Category { Id = 8, Name = "Lön" },
    ];

    // Tom kategori ska ge ett tydligt felmeddelande. Kasta ArgumentException, så visar sidan texten.
    public void Add(string name) => throw new NotImplementedException();

    // "Övrigt" får aldrig tas bort, och transaktionerna får inte försvinna (önskemål 3).
    public void Delete(int categoryId) => throw new NotImplementedException();
}
