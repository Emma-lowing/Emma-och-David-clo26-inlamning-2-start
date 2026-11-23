using Hushallsbudget.Models;
using Hushallsbudget.Services;

namespace Hushallsbudget.UI;

/// <summary>
/// Menyval för transaktioner. Klassen sköter bara inmatning och utskrift;
/// lagring går via IHouseholdRepository och budgetvarningar via IBudgetWarningService.
/// </summary>
public class TransactionMenu
{
    private readonly IHouseholdRepository _repository;
    private readonly ICategoryService _categories;
    private readonly IBudgetWarningService _warnings;
    private readonly IMoneyFormatter _formatter;

    public TransactionMenu(IHouseholdRepository repository, ICategoryService categories,
        IBudgetWarningService warnings, IMoneyFormatter formatter)
    {
        _repository = repository;
        _categories = categories;
        _warnings = warnings;
        _formatter = formatter;
    }

    public void ListTransactions()
    {
        (int year, int month) = MonthInput.Ask();
        PrintTransactions(year, month);
    }

    public void AddTransaction()
    {
        int type = ConsoleInput.ReadInt("Typ (1 = utgift, 2 = inkomst)", min: 1, max: 2);
        decimal amount = ConsoleInput.ReadDecimal("Belopp i kronor, till exempel 249,50", min: 0.01m);
        DateOnly date = ConsoleInput.ReadDate("Datum");
        string category = AskForCategory();
        string note = ConsoleInput.ReadText("Anteckning (valfri)");

        // Du behöver bygga ut detta mer: i låtsasdatan är en utgift ett negativt belopp, som i
        // Carlos Excel-ark. Ger ni Transaction ett eget fält för inkomst eller utgift, ändra här.
        var transaction = new Transaction
        {
            Amount = type == 1 ? -amount : amount,
            Date = date,
            Category = category,
            Note = note,
        };

        _repository.AddTransaction(transaction);
        Console.WriteLine("Transaktionen är sparad.");

        ShowBudgetWarnings(transaction);
    }

    public void DeleteTransaction()
    {
        (int year, int month) = MonthInput.Ask();
        if (!PrintTransactions(year, month))
        {
            return;
        }

        int id = ConsoleInput.ReadInt("Transaktionens nummer", min: 1);
        Transaction? transaction = _repository.GetTransaction(id);
        if (transaction is null)
        {
            ConsoleInput.WriteWarning($"Det finns ingen transaktion med nummer {id}.");
            return;
        }

        Console.WriteLine($"Ta bort: {Describe(transaction)}");

        // Önskemål 5: bekräfta innan något tas bort. Svaret "n" lämnar allt oförändrat.
        if (!ConsoleInput.Confirm("Är du säker?"))
        {
            Console.WriteLine("Inget togs bort.");
            return;
        }

        _repository.DeleteTransaction(transaction.Id);
        Console.WriteLine("Transaktionen är borttagen.");
    }

    // Returnerar false om månaden saknar transaktioner.
    private bool PrintTransactions(int year, int month)
    {
        List<Transaction> transactions = _repository.GetTransactionsForMonth(year, month);

        Console.WriteLine($"Transaktioner för {MonthInput.Name(year, month)}");
        if (transactions.Count == 0)
        {
            Console.WriteLine("  Det finns inga transaktioner den månaden.");
            return false;
        }

        foreach (Transaction transaction in transactions)
        {
            Console.WriteLine($"  {transaction.Id}. {Describe(transaction)}");
        }
        return true;
    }

    private string Describe(Transaction transaction)
    {
        // Du behöver bygga ut detta mer: önskemål 1 säger att en utgift visas som ett positivt
        // belopp under Utgifter. Visa tydligt om raden är en inkomst eller en utgift.
        string note = transaction.Note.Length > 0 ? $" ({transaction.Note})" : "";
        return $"{transaction.Date:yyyy-MM-dd}  {transaction.Category}: {_formatter.Format(transaction.Amount)}{note}";
    }

    private string AskForCategory()
    {
        List<Category> categories = _categories.GetAll();
        foreach (Category category in categories)
        {
            Console.WriteLine($"  {category.Id}. {category.Name}");
        }

        while (true)
        {
            int id = ConsoleInput.ReadInt("Kategorins nummer", min: 1);
            Category? selected = categories.FirstOrDefault(c => c.Id == id);
            if (selected is not null)
            {
                return selected.Name;
            }
            ConsoleInput.WriteWarning($"Det finns ingen kategori med nummer {id}.");
        }
    }

    private void ShowBudgetWarnings(Transaction transaction)
    {
        try
        {
            foreach (string warning in _warnings.CheckBudget(transaction))
            {
                ConsoleInput.WriteWarning(warning);
            }
        }
        catch (NotImplementedException)
        {
            // Transaktionen är redan sparad, så här visas bara en upplysning.
            ConsoleInput.WriteWarning("Budgetvarningar är inte implementerade ännu. Dem ska ni bygga i BudgetWarningService.");
        }
    }
}
