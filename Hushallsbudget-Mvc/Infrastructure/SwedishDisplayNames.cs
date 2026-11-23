using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Hushallsbudget.Infrastructure;

/// <summary>
/// Svenska namn på fält i felmeddelanden, till exempel "Längd i cm" i stället för "HeightCm".
/// Gäller bara fält som saknar [Display]-attribut, så att modellerna kan hållas rena.
/// </summary>
public class SwedishDisplayNames : IDisplayMetadataProvider
{
    private static readonly Dictionary<string, string> Names = new()
    {
        ["FirstName"] = "Förnamn",
        ["LastName"] = "Efternamn",
        ["BirthDate"] = "Födelsedatum",
        ["HairColor"] = "Hårfärg",
        ["HeightCm"] = "Längd i cm",
        ["Religion"] = "Religion",
        ["Position"] = "Position",
        ["TeamId"] = "Lag",
    };

    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        string? propertyName = context.Key.Name;
        if (propertyName is not null
            && context.DisplayMetadata.DisplayName is null
            && Names.TryGetValue(propertyName, out string? swedishName))
        {
            context.DisplayMetadata.DisplayName = () => swedishName;
        }
    }
}
