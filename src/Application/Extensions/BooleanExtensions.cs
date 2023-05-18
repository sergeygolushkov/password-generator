namespace Application.Extensions;

public static class BooleanExtensions
{
    public static byte ToShort(this bool value) => (byte)(value ? 1 : 0);
}
