using Hushallsbudget.Models;

namespace Hushallsbudget.Services;

/// <summary>
/// Kontraktet mellan gränssnittet och datalagret. Menyn känner bara till det här interfacet,
/// så ni kan byta InMemoryHouseholdRepository mot en riktig databas utan att röra menyn.
/// </summary>
public interface IHouseholdRepository
{
    Household GetHousehold();
    List<Member> GetAllMembers();
    void AddMember(Member member);

    /// <summary>Alla transaktioner från den första till den sista i månaden (önskemål 2).</summary>
    List<Transaction> GetTransactionsForMonth(int year, int month);
    Transaction? GetTransaction(int id);
    void AddTransaction(Transaction transaction);
    void DeleteTransaction(int id);

    // Du behöver bygga ut detta mer: metoder för kategorier och budgetar när ni sparar dem i databasen.
}
