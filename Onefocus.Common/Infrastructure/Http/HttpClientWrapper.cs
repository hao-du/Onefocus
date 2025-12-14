using Microsoft.Extensions.Logging;
using Onefocus.Common.Results;
using Onefocus.Common.Utilities;
using System.Net.Http.Json;

namespace Onefocus.Common.Infrastructure.Http;

public sealed class HttpClientWrapper(IHttpClientFactory factory, ILogger<HttpClientWrapper> logger) : IHttpClientWrapper
{
    private HttpClient Create(string name) => factory.CreateClient(name);

    public async Task<Result<T>> GetAsync<T>(string clientName, string url, CancellationToken cancellationToken = default)
    {
        var client = Create(clientName);

        var response = await client.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return GetResponseError<T>(url, response);
        }

        var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken);
        if (result == null)
        {
            return GetEmptyResponseError<T>(url);
        }

        return Result.Success(result);
    }

    public async Task<Result<TResponse>> PostAsync<TRequest, TResponse>(string clientName, string url, TRequest body, CancellationToken cancellationToken = default)
    {
        var client = Create(clientName);

        var response = await client.PostAsJsonAsync(url, body, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return GetResponseError<TResponse>(url, response);
        }

        var result = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
        if (result == null)
        {
            return GetEmptyResponseError<TResponse>(url);
        }

        return Result.Success(result);
    }

    public async Task<Result> PostAsync<TRequest>(string clientName, string url, TRequest body, CancellationToken cancellationToken = default)
    {
        var client = Create(clientName);

        var response = await client.PostAsJsonAsync(url, body, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return GetResponseError(url, response);
        }
        return Result.Success();
    }

    public async Task<Result<TResponse>> PutAsync<TRequest, TResponse>(string clientName, string url, TRequest body, CancellationToken cancellationToken = default)
    {
        var client = Create(clientName);

        var response = await client.PutAsJsonAsync(url, body, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return GetResponseError<TResponse>(url, response);
        }

        var result = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
        if (result == null)
        {
            return GetEmptyResponseError<TResponse>(url);
        }

        return Result.Success(result);
    }

    public async Task<Result> PutAsync<TRequest>(string clientName, string url, TRequest body, CancellationToken cancellationToken = default)
    {
        var client = Create(clientName);

        var response = await client.PutAsJsonAsync(url, body, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return GetResponseError(url, response);
        }
        return Result.Success();
    }


    private Result GetResponseError(string url, HttpResponseMessage response)
    {
        logger.LogError("GET request to {Url} failed with status code {StatusCode} - reason {ReasonPhrase}", url, response.StatusCode, response.ReasonPhrase);
        return Result.Failure(response.GetError());
    }

    private Result<T> GetResponseError<T>(string url, HttpResponseMessage response)
    {
        logger.LogError("GET request to {Url} failed with status code {StatusCode} - reason {ReasonPhrase}", url, response.StatusCode, response.ReasonPhrase);
        return Result.Failure<T>(response.GetError());
    }

    private Result<T> GetEmptyResponseError<T>(string url)
    {
        logger.LogError("GET request to {Url} returned empty response", url);
        return Result.Failure<T>("EmptyResponse", "The response content was empty.");
    }
}

