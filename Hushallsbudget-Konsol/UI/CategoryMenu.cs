using Hushallsbudget.Models;
using Hushallsbudget.Services;

namespace Hushallsbudget.UI;

/// <summary>
/// Menyval för kategorier. Logiken ligger i ICategoryService.
/// </summary>
public class CategoryMenu
{
    private readonly ICategoryService _categories;

    public CategoryMenu(ICategoryService categories)
    {
        _categories = categories;
    }

    public void Run()
    {
        new Menu("Kategorier")
            .Add("1", "Lista kategorier", ListCategories)
            .Add("2", "Lägg till kategori", AddCategory)
            .Add("3", "Ta bort kategori", DeleteCategory)
            .Run();
    }

    private void ListCategories()
    {
        foreach (Category category in _categories.GetAll())
        {
            Console.WriteLine($"{category.Id}. {category.Name}");
        }
    }

    private void AddCategory()
    {
        // Ingen kontroll av tom text här: den regeln hör hemma i CategoryService.
        string name = ConsoleInput.ReadText("Namn på kategorin");
        _categories.Add(name);
        Console.WriteLine($"Kategorin \"{name}\" är tillagd.");
    }

    private void DeleteCategory()
    {
        ListCategories();
        int id = ConsoleInput.ReadInt("Kategorins nummer", min: 1);

        // Önskemål 3: transaktionerna försvinner inte, de flyttas till "Övrigt".
        if (!ConsoleInput.Confirm("Transaktionerna i kategorin flyttas till \"Övrigt\". Är du säker?"))
        {
            Console.WriteLine("Inget togs bort.");
            return;
        }

        _categories.Delete(id);
        Console.WriteLine("Kategorin är borttagen.");
    }
}
