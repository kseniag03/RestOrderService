namespace RestOrderService.Models;

public enum Role
{
    [StringValue("customer")]
    CUSTOMER,
    [StringValue("chef")]
    CHEF,
    [StringValue("manager")]
    MANAGER
}

public static class RoleExtensions
{
    public static string GetStringValue(this Role value)
    {
        var stringValueAttribute = value.GetType()
            .GetField(value.ToString())
            .GetCustomAttributes(typeof(StringValueAttribute), false)
            .FirstOrDefault() as StringValueAttribute;

        return stringValueAttribute?.Value;
    }
}

public class StringValueAttribute : Attribute
{
    public string Value { get; }

    public StringValueAttribute(string value)
    {
        Value = value;
    }
}