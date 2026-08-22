using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1.Ads;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc cref="IAds" />
public class Ads : ApiBase, IAds
{
    private const string ApiUrlPart = "ads";

    /// <inheritdoc />
    public Ads(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger<Ads> logger)
        : base(settings, oauthGenerator, clientFactory, logger)
    {
    }

    /// <inheritdoc />
    public async Task<Result<AdBreak>> CreateAdBreakAsync(
        int breakDurationSeconds,
        Guid? id = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (breakDurationSeconds is < 7 or > 300)
        {
            return Result.Fail<AdBreak>("Break duration must be between 7 and 300 seconds.");
        }

        var payload = new CreateAdBreakPayload
        {
            Id = id ?? Guid.NewGuid(),
            BreakDurationSeconds = breakDurationSeconds
        };

        // v1/ads/ad-break
        var url = $"{ApiUrlPart}/ad-break";
        var result = await PostAsync<AdBreak, CreateAdBreakPayload>(url, ApiVersion.v1, payload, accessToken, cancellationToken)
            .ConfigureAwait(false);

        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.AdsWrite}");
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<Result<AdBreakStatus>> GetAdBreakStatusAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        // v1/ads/ad-break-status
        var url = $"{ApiUrlPart}/ad-break-status";
        var result = await GetAsync<AdBreakStatus>(url, ApiVersion.v1, null, accessToken, cancellationToken)
            .ConfigureAwait(false);

        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.AdsRead} or {KickScopes.AdsWrite}");
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<Result> EnrollInAdsAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        // v1/ads/enroll
        var url = $"{ApiUrlPart}/enroll";
        var result = await PostAsync<object>(url, ApiVersion.v1, accessToken, cancellationToken)
            .ConfigureAwait(false);

        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.AdsWrite}");
        }

        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        return Result.Ok().WithSuccesses(result.Successes);
    }
}
