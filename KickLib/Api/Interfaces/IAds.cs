using KickLib.Models.v1.Ads;

namespace KickLib.Api.Interfaces;

/// <summary>
///     Ads APIs allow an app to manage ad breaks and ads enrollment for a channel.
/// </summary>
public interface IAds
{
    /// <summary>
    ///     Creates an ad break on the authenticated broadcaster's channel.
    /// </summary>
    /// <remarks>
    ///     Required scope: ads:write
    /// </remarks>
    /// <param name="breakDurationSeconds">Duration of the ad break, in seconds (7-300).</param>
    /// <param name="id">Unique identifier (UUID) for the ad break. If not provided, a new one will be generated.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<AdBreak>> CreateAdBreakAsync(
        int breakDurationSeconds,
        Guid? id = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets ad break status for the authenticated broadcaster's channel.
    /// </summary>
    /// <remarks>
    ///     Required scope: ads:read or ads:write
    /// </remarks>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<AdBreakStatus>> GetAdBreakStatusAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Enrolls the authenticated broadcaster's channel in ads.
    /// </summary>
    /// <remarks>
    ///     Required scope: ads:write
    /// </remarks>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result> EnrollInAdsAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default);
}
