using System.ComponentModel.DataAnnotations;
using Hushallsbudget.Models;

namespace Hushallsbudget.ViewModels;

/// <summary>
/// Formuläret "Lägg till transaktion". Ogiltigt belopp, okänt datum och tom kategori
/// ger ett meddelande i formuläret i stället för en krasch.
/// </summary>
public class TransactionFormModel : IValidatableObject
{
    public const string Expense = "expense";
    public const string Income = "income";

    [Display(Name = "Typ")]
    public string Type { get; set; } = Expense;

    [Display(Name = "Belopp")]
    public decimal? Amount { get; set; }

    [Display(Name = "Datum")]
    public DateOnly? Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Kategori")]
    public string? Category { get; set; }

    [Display(Name = "Anteckning")]
    public string? Note { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Amount is null)
        {
            yield return new ValidationResult("Skriv ett belopp, till exempel 249,50.", [nameof(Amount)]);
        }
        else if (Amount <= 0)
        {
            yield return new ValidationResult("Beloppet måste vara större än 0.", [nameof(Amount)]);
        }
        if (Date is null)
        {
            yield return new ValidationResult("Skriv ett datum.", [nameof(Date)]);
        }
        if (string.IsNullOrWhiteSpace(Category))
        {
            yield return new ValidationResult("Välj en kategori.", [nameof(Category)]);
        }
        if (Type != Expense && Type != Income)
        {
            yield return new ValidationResult("Välj utgift eller inkomst.", [nameof(Type)]);
        }
    }

    // Du behöver bygga ut detta mer: i låtsasdatan är en utgift ett negativt belopp, som i
    // Carlos Excel-ark. Ger ni Transaction ett eget fält för inkomst eller utgift, ändra här.
    public Transaction ToTransaction() => new()
    {
        Amount = Type == Expense ? -Amount!.Value : Amount!.Value,
        Date = Date!.Value,
        Category = Category!.Trim(),
        Note = Note?.Trim() ?? "",
    };
}
