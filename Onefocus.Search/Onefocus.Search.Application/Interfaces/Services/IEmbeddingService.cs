using Onefocus.Common.Results;
using Onefocus.Search.Application.Contracts;

namespace Onefocus.Search.Application.Interfaces.Services;

public interface IEmbeddingService
{
    Task<Result<GetEmbeddingsResponseDto>> GetEmbeddings(GetEmbeddingsRequestDto request, CancellationToken cancellationToken = default);
}
