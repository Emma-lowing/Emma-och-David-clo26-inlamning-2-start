using Hushallsbudget.Models;

namespace Hushallsbudget.Services;

/// <summary>
/// Ska byggas av er. Kastar NotImplementedException, och då visas
/// "Budgetvarningar är inte implementerade ännu" efter att en transaktion har sparats.
/// </summary>
public class BudgetWarningService : IBudgetWarningService
{
    // Du behöver bygga ut detta mer: räkna ut hur stor del av kategorins budget som är använd
    // den månad transaktionen hör till, och returnera en varning vid 80 % och en annan vid 100 %.
    // Samma 80 %-varning får inte visas igen förrän månaden byts (önskemål 4).
    // Här passar till exempel mönstret Observer.
    public List<string> CheckBudget(Transaction transaction) => throw new NotImplementedException();
}
