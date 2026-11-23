using System.ComponentModel.DataAnnotations;
using Traningsbok.Models;

namespace Traningsbok.ViewModels;

/// <summary>
/// Formuläret "Lägg till övning". Alla tal är nullable, så att ett tomt eller felaktigt fält
/// ger ett meddelande i stället för att tyst bli 0.
/// </summary>
public class ExerciseFormModel
{
    [Display(Name = "Övning")]
    [Required(ErrorMessage = "Övningen måste ha ett namn.")]
    public string? Name { get; set; }

    [Display(Name = "antal set")]
    [Required(ErrorMessage = "Skriv antal set.")]
    [Range(1, 100, ErrorMessage = "Antal set måste vara mellan 1 och 100.")]
    public int? Sets { get; set; }

    [Display(Name = "antal reps")]
    [Required(ErrorMessage = "Skriv antal reps.")]
    [Range(1, 1000, ErrorMessage = "Antal reps måste vara mellan 1 och 1000.")]
    public int? Reps { get; set; }

    [Display(Name = "vikt")]
    [Required(ErrorMessage = "Skriv en vikt i kg, till exempel 62,5.")]
    [Range(typeof(decimal), "0", "1000", ErrorMessage = "Vikten måste vara mellan 0 och 1000 kg.")]
    public decimal? Weight { get; set; }

    public Exercise ToExercise() => new()
    {
        Name = Name!.Trim(),
        Sets = Sets!.Value,
        Reps = Reps!.Value,
        Weight = Weight!.Value,
    };
}
