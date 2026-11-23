using System.ComponentModel.DataAnnotations;
using Sportstatistik.Models;

namespace Sportstatistik.ViewModels;

/// <summary>
/// Formuläret "Registrera match". Målen är int?, så att tom eller felaktig inmatning
/// ger ett meddelande i stället för att tyst bli 0.
/// </summary>
public class MatchFormModel
{
    [Display(Name = "Hemmalag")]
    public int HomeTeamId { get; set; }

    [Display(Name = "Bortalag")]
    public int AwayTeamId { get; set; }

    [Display(Name = "Mål hemma")]
    [Required(ErrorMessage = "Skriv antal mål för hemmalaget.")]
    [Range(0, 99, ErrorMessage = "Mål hemma måste vara ett heltal mellan 0 och 99.")]
    public int? HomeGoals { get; set; }

    [Display(Name = "Mål borta")]
    [Required(ErrorMessage = "Skriv antal mål för bortalaget.")]
    [Range(0, 99, ErrorMessage = "Mål borta måste vara ett heltal mellan 0 och 99.")]
    public int? AwayGoals { get; set; }

    // Du behöver bygga ut detta mer: datum och en valfri anteckning när modellen Match har de fälten.

    public Match ToMatch() => new()
    {
        HomeTeamId = HomeTeamId,
        AwayTeamId = AwayTeamId,
        HomeGoals = HomeGoals!.Value,
        AwayGoals = AwayGoals!.Value,
    };
}
