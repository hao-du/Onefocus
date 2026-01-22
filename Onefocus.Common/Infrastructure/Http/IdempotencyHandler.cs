namespace Onefocus.Common.Infrastructure.Http;

public sealed class IdempotencyHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Method != HttpMethod.Get &&
            !request.Headers.Contains("Idempotency-Key"))
        {
            request.Headers.Add(
                "Idempotency-Key",
                Guid.CreateVersion7().ToString());
        }

        return base.SendAsync(request, cancellationToken);
    }
}
