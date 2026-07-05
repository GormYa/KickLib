using KickLib.Models.v1;
using KickLib.Models.v1.Livestreams;

namespace KickLib.Api.Interfaces;

/// <summary>
///     Interact with livestreams on Kick.com.
/// </summary>
public interface ILivestreams
{
    /// <summary>
    ///     Get current Kick Livestreams based on parameters.
    /// </summary>
    /// <param name="broadcasterId">Limit results to specific broadcaster (returns single result).</param>
    /// <param name="categoryId">Limit results to specific category.</param>
    /// <param name="language">Limit results to specific language.</param>
    /// <param name="limit">Number of results to return (default: 25, maximum: 100).</param>
    /// <param name="sort">Result sorting.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    [Obsolete("V1 livestreams endpoint is deprecated and will be removed in a future version. Use the V2 overloads (cursor-based) instead.")]
    Task<Result<ICollection<LivestreamResponse>>> GetLivestreamsAsync(
        int? broadcasterId = null,
        int? categoryId = null,
        string? language = null,
        int? limit = null,
        LivestreamSorting? sort = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get current Kick Livestreams based on parameters.
    /// </summary>
    /// <param name="broadcasterIds">Limit results to specific broadcasters.</param>
    /// <param name="categoryId">Limit results to specific category.</param>
    /// <param name="language">Limit results to specific language.</param>
    /// <param name="limit">Number of results to return (default: 25, maximum: 100).</param>
    /// <param name="sort">Result sorting.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    [Obsolete("V1 livestreams endpoint is deprecated and will be removed in a future version. Use the V2 overloads (cursor-based) instead.")]
    Task<Result<ICollection<LivestreamResponse>>> GetLivestreamsAsync(
        ICollection<int> broadcasterIds,
        int? categoryId = null,
        string? language = null,
        int? limit = null,
        LivestreamSorting? sort = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get current Kick Livestreams using cursor-based pagination.
    ///     Use <paramref name="cursor"/> from a previous response to fetch the next page.
    /// </summary>
    /// <param name="cursor">Pagination cursor from the previous response. Pass <c>null</c> to start from the beginning.</param>
    /// <param name="limit">Number of results to return (min: 1, max: 1000, default: 100).</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<PaginatedResponse<ICollection<LivestreamResponseV2>>>> GetLivestreamsAsync(
        string? cursor,
        int? limit = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get current Kick Livestreams using cursor-based pagination with full filter support.
    ///     Supports filtering by <c>CategoryIds</c> and <c>LanguageCodes</c> on the request object.
    /// </summary>
    /// <param name="request">Request object containing filter and pagination options.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<PaginatedResponse<ICollection<LivestreamResponseV2>>>> GetLivestreamsAsync(
        GetLivestreamsRequest request,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get livestream information for currently authorised user.
    /// </summary>
    /// <returns>Returns <c>null</c> if user is not livestreaming.</returns>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<LivestreamResponse?>> GetLivestreamAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get the active livestreams for the given user (broadcaster) IDs.
    /// </summary>
    /// <param name="userIds">Broadcaster User IDs. Between 1 and 100 user IDs can be provided.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ICollection<LivestreamResponseV2>>> GetLivestreamsByUserIdsAsync(
        ICollection<long> userIds,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
}
