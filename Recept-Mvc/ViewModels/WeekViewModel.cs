using Recept.Models;

namespace Recept.ViewModels;

/// <summary>
/// En vald vecka: veckoplanen eller inköpslistan.
/// </summary>
public class WeekViewModel
{
    public int Year { get; set; }
    public int Week { get; set; }

    /// <summary>Måltiderna, med receptnamnet uppslaget.</summary>
    public List<(DateOnly Date, string RecipeName)> Meals { get; set; } = new();

    /// <summary>Inköpslistan, med mängderna redan formaterade.</summary>
    public List<string> ShoppingLines { get; set; } = new();
}
