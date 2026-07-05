using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1;
using KickLib.Models.v1.Drops;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc cref="IDrops" />
public class Drops : ApiBase, IDrops
{
    private const string ApiUrlPart = "drops/claims";

    /// <inheritdoc />
    public Drops(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger<Drops> logger)
        : base(settings, oauthGenerator, clientFactory, logger)
    {
    }

    /// <inheritdoc />
    public Task<Result<PaginatedResponse<ICollection<ClaimResponse>>>> GetClaimsAsync(
        GetClaimsRequest? request = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (request?.Limit is < 1 or > 1000)
        {
            return Task.FromResult(Result.Fail<PaginatedResponse<ICollection<ClaimResponse>>>("Limit must be value between 1 and 1000!"));
        }

        var query = new List<KeyValuePair<string, string>>();
        if (request != null)
        {
            if (!string.IsNullOrWhiteSpace(request.CampaignId))
            {
                query.Add(new("campaign_id", request.CampaignId));
            }

            if (request.Limit.HasValue)
            {
                query.Add(new("limit", request.Limit.Value.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(request.Cursor))
            {
                query.Add(new("cursor", request.Cursor));
            }

            if (request.UserId.HasValue)
            {
                query.Add(new("user_id", request.UserId.Value.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(request.ClaimId))
            {
                query.Add(new("claim_id", request.ClaimId));
            }

            if (!string.IsNullOrWhiteSpace(request.ExternalStatus))
            {
                query.Add(new("external_status", request.ExternalStatus));
            }
        }

        return GetClaimsInternalAsync(query, accessToken, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Result<bool>> UpdateClaimsAsync(
        ICollection<ClaimStatusUpdate> claims,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (claims is null || !claims.Any())
        {
            return Task.FromResult(Result.Fail<bool>("At least one claim is required!"));
        }

        var claimsList = claims.ToList();
        if (claimsList.Count > 100)
        {
            return Task.FromResult(Result.Fail<bool>("A maximum of 100 claims can be provided!"));
        }

        var payload = UpdateClaimsApiRequest.FromRequest(claimsList);

        // v1/drops/claims
        return PatchAsync(ApiUrlPart, ApiVersion.v1, payload, accessToken, cancellationToken);
    }

    private async Task<Result<PaginatedResponse<ICollection<ClaimResponse>>>> GetClaimsInternalAsync(
        List<KeyValuePair<string, string>> query,
        string? accessToken,
        CancellationToken cancellationToken)
    {
        // v1/drops/claims
        var result = await GetAsync<ClaimsPageResponse>(ApiUrlPart, ApiVersion.v1, query, accessToken, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
        {
            return Result.Fail<PaginatedResponse<ICollection<ClaimResponse>>>(result.Errors);
        }

        return Result.Ok(new PaginatedResponse<ICollection<ClaimResponse>>
        {
            Data = result.Value.Claims,
            NextCursor = result.Value.Cursor
        });
    }
}
