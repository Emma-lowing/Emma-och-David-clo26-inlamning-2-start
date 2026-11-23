using Microsoft.AspNetCore.Mvc;
using Recept.Models;
using Recept.Services;
using Recept.ViewModels;

namespace Recept.Controllers;

/// <summary>
/// Sidor för recept. Controllern sköter bara formulär och sidor;
/// lagring går via IRecipeRepository och räkning via IRecipeCalculator.
/// </summary>
public class RecipesController : AppController
{
    private readonly IRecipeRepository _repository;
    private readonly IRecipeCalculator _calculator;

    public RecipesController(IRecipeRepository repository, IRecipeCalculator calculator)
    {
        _repository = repository;
        _calculator = calculator;
    }

    public IActionResult Index(string? search) => Run(() =>
    {
        ViewData["Search"] = search;
        List<Recipe> recipes = string.IsNullOrWhiteSpace(search)
            ? _repository.GetAllRecipes()
            : _repository.SearchByIngredient(search.Trim());
        return View(recipes);
    });

    public IActionResult Create() => View(new RecipeFormModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(RecipeFormModel form) => Run(() =>
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        try
        {
            Recipe recipe = form.ToRecipe();
            _repository.AddRecipe(recipe);
            TempData["Message"] = $"Receptet \"{recipe.Name}\" är sparat.";
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            // Felaktig inmatning som upptäcks i era services visas i formuläret.
            ModelState.AddModelError("", ex.Message);
            return View(form);
        }
    });

    public IActionResult Details(int id, int? servings) => Run(() =>
    {
        Recipe? recipe = _repository.GetRecipe(id);
        if (recipe is null)
        {
            return NotFound();
        }

        int shownServings = servings is > 0 ? servings.Value : recipe.Servings;
        List<Ingredient> ingredients = shownServings == recipe.Servings
            ? recipe.Ingredients
            : _calculator.ScaleIngredients(recipe, shownServings);

        return View(new RecipeDetailsViewModel
        {
            Id = recipe.Id,
            Name = recipe.Name,
            OriginalServings = recipe.Servings,
            Servings = shownServings,
            IngredientLines = ingredients
                .Select(i => $"{_calculator.FormatAmount(i.Amount, i.Unit)} {i.Name}")
                .ToList(),
        });
    });

    // Önskemål 5: först en sida som frågar "Är du säker?", sedan tas receptet bort.
    public IActionResult Delete(int id) => Run(() =>
    {
        Recipe? recipe = _repository.GetRecipe(id);
        return recipe is null ? NotFound() : View(recipe);
    });

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id) =>
        RunAndRedirect(() => _repository.DeleteRecipe(id), "Receptet är borttaget.", nameof(Index));
}
