using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Onefocus.Common.Results;

namespace Onefocus.Search.Infrastructure;

public static class Errors
{
    public static readonly Error IndexIsRequired = new("IndexIsRequired", "Index is required.");
    public static readonly Error InvalidIndexCreation = new("InvalidIndexCreation", "Create index failed.");
    public static readonly Error InvalidMappingsUpdate = new("InvalidMappingsUpdate", "Update mappings failed.");
    public static readonly Error InvalidSettingsUpdate = new("InvalidSettingsUpdate", "Update settings failed.");
    public static readonly Error EntityIdIsNotSpecified = new("EntityIdIsNotSpecified", "EntityId is not specified.");
    public static readonly Error IndexError = new("IndexError", "Index entities failed.");
}