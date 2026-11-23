using Microsoft.AspNetCore.Mvc;

namespace Recept.Controllers;

/// <summary>
/// Basklass för controllers. Kör anrop mot tjänsterna och fångar fel direkt här,
/// så att en sida aldrig kraschar och debuggern inte stannar.
/// NotImplementedException visar sidan "Inte implementerat ännu".
/// </summary>
public abstract class AppController : Controller
{
    protected IActionResult Run(Func<IActionResult> action)
    {
        try
        {
            return action();
        }
        catch (NotImplementedException)
        {
            return View("NotImplemented");
        }
    }

    /// <summary>
    /// Kör en åtgärd från ett formulär och går tillbaka till en sida med ett meddelande.
    /// Felaktig inmatning som upptäcks i era services: kasta ArgumentException med ett
    /// meddelande på svenska, så visas det i stället för att något kraschar.
    /// </summary>
    protected IActionResult RunAndRedirect(Action action, string successMessage, string redirectAction, object? routeValues = null)
    {
        return Run(() =>
        {
            try
            {
                action();
                TempData["Message"] = successMessage;
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(redirectAction, routeValues);
        });
    }
}
