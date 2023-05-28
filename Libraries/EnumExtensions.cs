namespace RestOrderService.Libraries;

public static class EnumExtensions
{
    public static string GetStringValue<TEnum>(this TEnum value) where TEnum : Enum
    {
        var stringValueAttribute = value.GetType()
            .GetField(value.ToString())
            ?.GetCustomAttributes(typeof(StringValueAttribute), false)
            .FirstOrDefault() as StringValueAttribute;

        return stringValueAttribute?.Value ?? "";
    }
}