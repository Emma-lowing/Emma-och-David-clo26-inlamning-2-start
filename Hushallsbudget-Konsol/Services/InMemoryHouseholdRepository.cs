using Hushallsbudget.Models;

namespace Hushallsbudget.Services;

/// <summary>
/// Låtsasdatabas: allt ligger i listor i minnet och försvinner när programmet stängs.
/// Den finns bara för att menyn ska gå att köra från dag ett.
/// Ersätt den med en egen klass som implementerar IHouseholdRepository mot er databas,
/// och byt sedan en enda rad i Program.cs.
/// </summary>
public class InMemoryHouseholdRepository : IHouseholdRepository
{
    private readonly Household _household = new() { Id = 1, Name = "Familjen Berg" };
    private readonly List<Member> _members = new();
    private readonly List<Transaction> _transactions = new();
    private int _nextMemberId = 1;
    private int _nextTransactionId = 1;

    public InMemoryHouseholdRepository()
    {
        Seed();
    }

    public Household GetHousehold() => _household;

    public List<Member> GetAllMembers() => _members.ToList();

    public void AddMember(Member member)
    {
        // Du behöver bygga ut detta mer: beställningen säger att ett hushåll har 1–5 medlemmar.
        member.Id = _nextMemberId++;
        _members.Add(member);
    }

    // DateOnly har inget klockslag, så här räcker det att jämföra år och månad.
    // I en databas kan datumet ha ett klockslag. Tänk på önskemål 2 när ni skriver er fråga,
    // så att en transaktion kl. 23.59 den 31 januari hamnar i januari.
    public List<Transaction> GetTransactionsForMonth(int year, int month) =>
        _transactions
            .Where(t => t.Date.Year == year && t.Date.Month == month)
            .OrderBy(t => t.Date)
            .ToList();

    public Transaction? GetTransaction(int id) => _transactions.FirstOrDefault(t => t.Id == id);

    public void AddTransaction(Transaction transaction)
    {
        transaction.Id = _nextTransactionId++;
        _transactions.Add(transaction);
    }

    public void DeleteTransaction(int id) => _transactions.RemoveAll(t => t.Id == id);

    // Exempeldata, så att det finns något att titta på direkt. Alla namn och belopp är påhittade.
    // Er riktiga databas ska seeda sig själv på samma sätt vid första start, om den är tom.
    // Här passar till exempel mönstret Builder: en transaktion med belopp, datum, kategori och anteckning
    // blir lättare att läsa om den byggs steg för steg, till exempel .Expense(250).InCategory("Mat").
    // Utgifter är negativa belopp och inkomster positiva, som i Carlos Excel-ark.
    private void Seed()
    {
        AddMember(new Member { FirstName = "Kim", LastName = "Berg" });
        AddMember(new Member { FirstName = "Alex", LastName = "Berg" });

        // Den sista oktober: hör till oktober, inte till november (önskemål 2).
        AddTransaction(new Transaction { Amount = -100m, Date = new DateOnly(2026, 10, 31), Category = "Mat", Note = "Fika" });

        AddTransaction(new Transaction { Amount = 28500m, Date = new DateOnly(2026, 11, 1), Category = "Lön", Note = "Lön november" });
        AddTransaction(new Transaction { Amount = -9800m, Date = new DateOnly(2026, 11, 1), Category = "Boende", Note = "Hyra" });
        AddTransaction(new Transaction { Amount = -899m, Date = new DateOnly(2026, 11, 5), Category = "El och internet", Note = "Bredband och el" });
        AddTransaction(new Transaction { Amount = -1000m, Date = new DateOnly(2026, 11, 8), Category = "Mat", Note = "Storhandling" });
        AddTransaction(new Transaction { Amount = -249.5m, Date = new DateOnly(2026, 11, 12), Category = "Mat", Note = "ICA" });
        AddTransaction(new Transaction { Amount = -790m, Date = new DateOnly(2026, 11, 14), Category = "Nöje", Note = "Konsertbiljetter" });
        AddTransaction(new Transaction { Amount = -450m, Date = new DateOnly(2026, 11, 18), Category = "Transport", Note = "Busskort" });
        AddTransaction(new Transaction { Amount = -350m, Date = new DateOnly(2026, 11, 20), Category = "Hälsa", Note = "" });
    }
}
