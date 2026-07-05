using KickLib.Api.Interfaces;
using KickLib.Auth;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc cref="IRawApi" />
public class RawApi : ApiBase, IRawApi
{
    /// <inheritdoc />
    public RawApi(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger<RawApi> logger)
        : base(settings, oauthGenerator, clientFactory, logger)
    {
    }

    /// <inheritdoc />
    public new Task<Result<TResponse>> GetAsync<TResponse>(
        string urlPart,
        ApiVersion version,
        List<KeyValuePair<string, string>>? queryParams = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return base.GetAsync<TResponse>(urlPart, version, queryParams, accessToken, cancellationToken);
    }

    /// <inheritdoc />
    public new Task<Result<TResponse>> PostAsync<TResponse>(
        string urlPart,
        ApiVersion version,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return base.PostAsync<TResponse>(urlPart, version, accessToken, cancellationToken);
    }

    /// <inheritdoc />
    public new Task<Result<TResponse>> PostAsync<TResponse, TRequest>(
        string urlPart,
        ApiVersion version,
        TRequest input,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TResponse : class
        where TRequest : class
    {
        return base.PostAsync<TResponse, TRequest>(urlPart, version, input, accessToken, cancellationToken);
    }

    /// <inheritdoc />
    public new Task<Result<bool>> PatchAsync<TRequest>(
        string urlPart,
        ApiVersion version,
        TRequest input,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TRequest : class
    {
        return base.PatchAsync(urlPart, version, input, accessToken, cancellationToken);
    }

    /// <inheritdoc />
    public new Task<Result<TResponse>> PatchAsync<TResponse, TRequest>(
        string urlPart,
        ApiVersion version,
        TRequest input,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return base.PatchAsync<TResponse, TRequest>(urlPart, version, input, accessToken, cancellationToken);
    }

    /// <inheritdoc />
    public new Task<Result<bool>> DeleteAsync(
        string urlPart,
        ApiVersion version,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(urlPart, version, accessToken, cancellationToken);
    }
}
