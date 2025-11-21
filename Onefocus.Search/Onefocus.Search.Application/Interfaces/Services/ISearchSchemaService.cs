using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Common.Results;
using Onefocus.Common.Search.Schema;

namespace Onefocus.Search.Application.Interfaces.Services;

public interface ISearchSchemaService
{
    Task<Result> UpsertIndexMappings(MappingSchema schemaMessage, CancellationToken cancellationToken);
}
