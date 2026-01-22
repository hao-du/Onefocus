namespace Onefocus.Common.Utilities;

public static class GuidExtensions
{
    public static bool IsNullOrEmpty(this Guid? value)
    {
        return !value.HasValue || value.Value == Guid.Empty;
    }

    public static bool IsEmpty(this Guid value)
    {
        return value == Guid.Empty;
    }

    public static bool IsEmpty(this Guid? value)
    {
        return value == Guid.Empty;
    }
}

