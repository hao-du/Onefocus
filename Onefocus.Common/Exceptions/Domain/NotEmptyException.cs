namespace Onefocus.Common.Exceptions.Domain;

public sealed class NotEmptyException(string message) : DomainException(message)
{
}

