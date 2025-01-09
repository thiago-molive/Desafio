using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Abstractions.Extensions;

public static class EnumerableExtensions
{
    public static string GetName(this Enum enumValue)
    {
        if (enumValue == null)
            return null;
        var displayAttribute = enumValue.GetDisplayAttribute();
        return displayAttribute == null ? enumValue.ToString() : displayAttribute.Name;
    }

    private static DisplayAttribute? GetDisplayAttribute(this Enum enumValue)
    {
        FieldInfo? field = enumValue.GetType().GetField(enumValue.ToString());
        return (object)field == null ? null : field.GetCustomAttribute<DisplayAttribute>();
    }
}

