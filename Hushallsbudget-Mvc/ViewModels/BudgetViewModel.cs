using Microsoft.AspNetCore.Mvc.Rendering;
using Hushallsbudget.Models;

namespace Hushallsbudget.ViewModels;

/// <summary>
/// Budget per kategori för en månad, med beloppen redan formaterade.
/// </summary>
public class BudgetViewModel : MonthViewModel
{
    public List<BudgetLine> Lines { get; set; } = new();

    /// <summary>Kategorierna i formuläret "Sätt budget".</summary>
    public List<SelectListItem> Categories { get; set; } = new();
}

public class BudgetLine
{
    public string CategoryName { get; set; } = "";
    public string SpentText { get; set; } = "";
    public string BudgetText { get; set; } = "";
    public int Percent { get; set; }
    public BudgetLevel Level { get; set; }
}
