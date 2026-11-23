using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Traningsbok.Infrastructure;

/// <summary>
/// Tolkar decimaltal i formulären så att både "2,5" och "2.5" godkänns.
/// Standardtolkningen för sv-SE underkänner punkt, vilket förvirrar användare.
/// </summary>
public class FlexibleDecimalModelBinder : IModelBinder
{
    private static readonly CultureInfo Swedish = CultureInfo.GetCultureInfo("sv-SE");

    public Task BindModelAsync(ModelBindingContext context)
    {
        ValueProviderResult value = context.ValueProvider.GetValue(context.ModelName);
        if (value == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        context.ModelState.SetModelValue(context.ModelName, value);
        string text = value.FirstValue?.Trim() ?? "";
        var messages = context.ModelMetadata.ModelBindingMessageProvider;

        if (text.Length == 0)
        {
            if (context.ModelMetadata.IsReferenceOrNullableType)
            {
                context.Result = ModelBindingResult.Success(null);
            }
            else
            {
                context.ModelState.TryAddModelError(context.ModelName, messages.ValueMustNotBeNullAccessor(text));
            }
            return Task.CompletedTask;
        }

        if (decimal.TryParse(text.Replace('.', ','), NumberStyles.Number, Swedish, out decimal number))
        {
            context.Result = ModelBindingResult.Success(number);
        }
        else
        {
            context.ModelState.TryAddModelError(context.ModelName,
                messages.AttemptedValueIsInvalidAccessor(text, context.ModelMetadata.GetDisplayName()));
        }
        return Task.CompletedTask;
    }
}

/// <summary>Kopplar in FlexibleDecimalModelBinder för decimal och decimal?.</summary>
public class FlexibleDecimalModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        Type type = context.Metadata.UnderlyingOrModelType;
        return type == typeof(decimal) ? new FlexibleDecimalModelBinder() : null;
    }
}
