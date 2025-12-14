using Onefocus.Common.Results;

namespace Onefocus.Common.Infrastructure.Http;

public interface IHttpClientWrapper
{
    Task<Result<T>> GetAsync<T>(string clientName, string url, CancellationToken ct = default);

    Task<Result<TResponse>> PostAsync<TRequest, TResponse>(string clientName, string url, TRequest body, CancellationToken ct = default);

    Task<Result> PostAsync<TRequest>(string clientName, string url, TRequest body, CancellationToken ct = default);

    Task<Result<TResponse>> PutAsync<TRequest, TResponse>(string clientName, string url, TRequest body, CancellationToken ct = default);

    Task<Result> PutAsync<TRequest>(string clientName, string url, TRequest body, CancellationToken ct = default);
}

