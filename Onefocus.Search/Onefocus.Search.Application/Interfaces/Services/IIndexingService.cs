using Onefocus.Common.Results;
using Onefocus.Search.Application.Contracts;

namespace Onefocus.Search.Application.Interfaces.Services;

public interface IIndexingService
{
    Task<Result> AddIndex(IReadOnlyList<SearchIndexDto> envelopeDtos, CancellationToken cancellationToken = default);
}