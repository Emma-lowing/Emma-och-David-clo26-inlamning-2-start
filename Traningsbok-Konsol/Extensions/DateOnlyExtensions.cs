using System.Globalization;
using Traningsbok.Extensions;
namespace Traningsbok.Extensions;

public static class DateOnlyExtensions
{
    public static DateOnly StartOfWeek(this DateOnly date)
    {
        int daysSinceMonday = ((int)date.DayOfWeek + 6) % 7;

        return date.AddDays(-daysSinceMonday);
    }

    public static DateOnly EndOfWeek(this DateOnly date)
    {
        return date.StartOfWeek().AddDays(6);
    }
}