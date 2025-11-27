using Onefocus.Common.Results;
using Onefocus.Search.Application.Contracts;

namespace Onefocus.Search.Application.Interfaces.Services;

public interface ISearchSchemaService
{
    Task<Result> UpsertIndexMappings(SearchSchemaDto searchSchemaDto, CancellationToken cancellationToken);
}
