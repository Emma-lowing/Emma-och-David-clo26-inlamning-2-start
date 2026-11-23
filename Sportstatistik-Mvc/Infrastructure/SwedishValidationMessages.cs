using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Sportstatistik.Infrastructure;

/// <summary>
/// Felmeddelanden på svenska när ett formulärvärde inte går att tolka, till exempel "abc" som mängd.
/// </summary>
public static class SwedishValidationMessages
{
    public static void Apply(DefaultModelBindingMessageProvider messages)
    {
        messages.SetAttemptedValueIsInvalidAccessor((value, field) => $"\"{value}\" är inte ett giltigt värde för {field}.");
        messages.SetValueMustBeANumberAccessor(field => $"{field} måste vara ett tal.");
        messages.SetValueMustNotBeNullAccessor(value => $"Värdet \"{value}\" är ogiltigt.");
        messages.SetMissingBindRequiredValueAccessor(field => $"{field} saknas.");
        messages.SetValueIsInvalidAccessor(value => $"\"{value}\" är ogiltigt.");
        messages.SetNonPropertyAttemptedValueIsInvalidAccessor(value => $"\"{value}\" är ogiltigt.");
        messages.SetUnknownValueIsInvalidAccessor(field => $"Värdet för {field} är ogiltigt.");
        messages.SetNonPropertyUnknownValueIsInvalidAccessor(() => "Värdet är ogiltigt.");
        messages.SetNonPropertyValueMustBeANumberAccessor(() => "Värdet måste vara ett tal.");
        messages.SetMissingKeyOrValueAccessor(() => "Värde saknas.");
        messages.SetMissingRequestBodyRequiredValueAccessor(() => "Formuläret är tomt.");
    }
}
