using KickLib.Models.v1;
using KickLib.Models.v1.Drops;

namespace KickLib.Api.Interfaces;

/// <summary>
///     Drops APIs allow an app associated with an organization to retrieve and update Drops campaign reward claims.
///     Only OAuth apps associated with an organization can access these endpoints.
/// </summary>
public interface IDrops
{
    /// <summary>
    ///     Retrieve Drops reward claims, optionally filtered, using cursor-based pagination.
    /// </summary>
    /// <param name="request">Filter and pagination options. Pass <c>null</c> to fetch the first page without filters.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<PaginatedResponse<ICollection<ClaimResponse>>>> GetClaimsAsync(
        GetClaimsRequest? request = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Update the external status of one or more claims. Up to 100 claims can be updated in a single request.
    /// </summary>
    /// <param name="claims">Claim status updates to apply.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<bool>> UpdateClaimsAsync(
        ICollection<ClaimStatusUpdate> claims,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
}
