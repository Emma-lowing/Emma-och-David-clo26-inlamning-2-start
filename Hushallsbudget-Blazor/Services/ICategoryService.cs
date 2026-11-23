using Hushallsbudget.Models;

namespace Hushallsbudget.Services;

/// <summary>
/// Kategorier: standardlistan och användarens egna.
/// </summary>
public interface ICategoryService
{
    List<Category> GetAll();
    void Add(string name);

    /// <summary>Tar bort en kategori. Transaktionerna i kategorin flyttas till "Övrigt" (önskemål 3).</summary>
    void Delete(int categoryId);
}
