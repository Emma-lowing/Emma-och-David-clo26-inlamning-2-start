using System.ComponentModel.DataAnnotations;
using Traningsbok.Models;

namespace Traningsbok.ViewModels;

/// <summary>
/// Formuläret "Lägg till pass".
/// </summary>
public class WorkoutFormModel
{
    // Okänt datum, till exempel "abc", ger ett felmeddelande från SwedishValidationMessages.
    [Display(Name = "Datum")]
    [Required(ErrorMessage = "Välj ett datum.")]
    public DateOnly? Date { get; set; }

    [Display(Name = "Träningstyp")]
    [Required(ErrorMessage = "Välj en träningstyp.")]
    public string? Type { get; set; }

    // Du behöver bygga ut detta mer: längd i minuter, anteckning och utövare när modellen Workout har de fälten.

    public Workout ToWorkout() => new()
    {
        Date = Date!.Value,
        Type = Type!.Trim(),
    };
}
