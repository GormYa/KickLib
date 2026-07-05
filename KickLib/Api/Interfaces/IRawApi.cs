namespace KickLib.Api.Interfaces;

/// <summary>
///     Escape hatch for calling Kick API endpoints that don't have a dedicated KickLib API surface yet.
///     Reuses the same authentication, versioning, and error-handling plumbing as the rest of KickLib -
///     you only need to supply the URL part, API version, and request/response types.
/// </summary>
public interface IRawApi
{
    /// <summary>
    ///     Perform a GET request against a Kick API endpoint.
    /// </summary>
    /// <param name="urlPart">URL part relative to the versioned base (e.g. "livestreams"), or a full URL.</param>
    /// <param name="version">API version to target.</param>
    /// <param name="queryParams">Optional query string parameters.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<TResponse>> GetAsync<TResponse>(
        string urlPart,
        ApiVersion version,
        List<KeyValuePair<string, string>>? queryParams = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    /// <summary>
    ///     Perform a POST request without a request payload.
    /// </summary>
    /// <param name="urlPart">URL part relative to the versioned base (e.g. "livestreams"), or a full URL.</param>
    /// <param name="version">API version to target.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<TResponse>> PostAsync<TResponse>(
        string urlPart,
        ApiVersion version,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    /// <summary>
    ///     Perform a POST request with a JSON-serialized request payload.
    /// </summary>
    /// <param name="urlPart">URL part relative to the versioned base (e.g. "livestreams"), or a full URL.</param>
    /// <param name="version">API version to target.</param>
    /// <param name="input">Request payload to serialize as the request body.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<TResponse>> PostAsync<TResponse, TRequest>(
        string urlPart,
        ApiVersion version,
        TRequest input,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TResponse : class
        where TRequest : class;

    /// <summary>
    ///     Perform a PATCH request with a JSON-serialized request payload, for endpoints that respond with no content (e.g. 204).
    /// </summary>
    /// <param name="urlPart">URL part relative to the versioned base (e.g. "livestreams"), or a full URL.</param>
    /// <param name="version">API version to target.</param>
    /// <param name="input">Request payload to serialize as the request body.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<bool>> PatchAsync<TRequest>(
        string urlPart,
        ApiVersion version,
        TRequest input,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TRequest : class;

    /// <summary>
    ///     Perform a PATCH request with a JSON-serialized request payload, deserializing the response body.
    /// </summary>
    /// <param name="urlPart">URL part relative to the versioned base (e.g. "livestreams"), or a full URL.</param>
    /// <param name="version">API version to target.</param>
    /// <param name="input">Request payload to serialize as the request body.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<TResponse>> PatchAsync<TResponse, TRequest>(
        string urlPart,
        ApiVersion version,
        TRequest input,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    /// <summary>
    ///     Perform a DELETE request without a query string or request payload.
    /// </summary>
    /// <param name="urlPart">URL part relative to the versioned base (e.g. "livestreams"), or a full URL.</param>
    /// <param name="version">API version to target.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<bool>> DeleteAsync(
        string urlPart,
        ApiVersion version,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
}
