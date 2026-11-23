using Sportstatistik.Models;
using Sportstatistik.Services;

namespace Sportstatistik.UI;

/// <summary>
/// Menyval för spelarpositioner. Logiken ligger i IPositionService.
/// </summary>
public class PositionMenu
{
    private readonly IPositionService _positions;

    public PositionMenu(IPositionService positions)
    {
        _positions = positions;
    }

    public void Run()
    {
        new Menu("Positioner")
            .Add("1", "Lista positioner", ListPositions)
            .Add("2", "Lägg till position", AddPosition)
            .Add("3", "Ta bort position", DeletePosition)
            .Run();
    }

    private void ListPositions()
    {
        foreach (Position position in _positions.GetAll())
        {
            Console.WriteLine($"{position.Id}. {position.Name}");
        }
    }

    private void AddPosition()
    {
        // Ingen kontroll av tom text här: den regeln hör hemma i PositionService.
        string name = ConsoleInput.ReadText("Namn på positionen");
        _positions.Add(name);
        Console.WriteLine($"Positionen \"{name}\" är tillagd.");
    }

    private void DeletePosition()
    {
        ListPositions();
        int id = ConsoleInput.ReadInt("Positionens nummer", min: 1);

        if (!ConsoleInput.Confirm("Spelarna med positionen flyttas till \"Övrigt\". Fortsätta?"))
        {
            Console.WriteLine("Inget togs bort.");
            return;
        }

        _positions.Delete(id);
        Console.WriteLine("Positionen är borttagen.");
    }
}
