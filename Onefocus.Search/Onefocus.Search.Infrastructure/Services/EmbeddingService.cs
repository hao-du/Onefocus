using Microsoft.Extensions.Logging;
using Onefocus.Common.Constants;
using Onefocus.Common.Infrastructure.Http;
using Onefocus.Common.Results;
using Onefocus.Common.Utilities;
using Onefocus.Search.Application.Contracts;
using Onefocus.Search.Application.Interfaces.Services;

namespace Onefocus.Search.Infrastructure.Services;

public class EmbeddingService(IHttpClientWrapper httpClient, ILogger<SearchIndexService> logger) : IEmbeddingService
{
    public async Task<Result<GetEmbeddingsResponseDto>> GetEmbeddings(GetEmbeddingsRequestDto request, CancellationToken cancellationToken = default)
    {
        var getEmbeddingResponseResult = await httpClient.PostAsync<GetEmbeddingsRequestDto, GetEmbeddingsResponseDto>(
            HttpClientNames.Embedding,
            "/embed",
            request,
            cancellationToken
        );

        if (getEmbeddingResponseResult.IsFailure)
        {
            logger.LogErrors(getEmbeddingResponseResult.Errors);
        }

        return getEmbeddingResponseResult;
    }
}
