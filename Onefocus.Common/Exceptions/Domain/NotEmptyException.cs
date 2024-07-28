namespace Onefocus.Common.Exceptions.Domain;

public sealed class NotEmptyException : DomainException
{
    public NotEmptyException(string message) : base(message) { }
}

