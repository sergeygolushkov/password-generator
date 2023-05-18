namespace Application.Extensions;

public static class StringExtensions
{
    public static bool ToBoolean(this string value) => (value != null && (value == "1" || value.ToLower() == "true")) ? true : false;

    public static char GetRandomChar(this string value, int randomInt)
    {
        if (value is null || value.Length < 1) throw new ArgumentNullException(nameof(value));

        var randomNumber = (int)Math.Floor((double)(Math.Abs(randomInt % value.Length)));
        return value[randomNumber];
    }
}
