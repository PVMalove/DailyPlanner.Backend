namespace DailyPlanner.API.Extensions;

public static class StringExtensions
{
    public static long? ToNullableLong(this string str)
    {
        return long.TryParse(str, out var result) ? result : null;
    }
}