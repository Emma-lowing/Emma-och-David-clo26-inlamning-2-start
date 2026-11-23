namespace Recept.ViewModels;

/// <summary>
/// Ett recept som det ska visas, med mängderna redan formaterade.
/// </summary>
public class RecipeDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int OriginalServings { get; set; }
    public int Servings { get; set; }
    public List<string> IngredientLines { get; set; } = new();
}
