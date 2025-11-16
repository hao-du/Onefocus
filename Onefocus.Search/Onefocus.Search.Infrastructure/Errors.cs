using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Onefocus.Common.Results;

namespace Onefocus.Search.Infrastructure;

public static class Errors
{
    public static readonly Error IndexIsRequired = new("IndexIsRequired", "Index is required.");
    public static readonly Error EntityIdIsNotSpecified = new("EntityIdIsNotSpecified", "EntityId is not specified.");
}