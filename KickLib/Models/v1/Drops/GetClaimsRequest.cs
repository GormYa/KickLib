namespace KickLib.Models.v1.Drops;

/// <summary>
///     Request object for retrieving Drops claims.
/// </summary>
public class GetClaimsRequest
{
    /// <summary>
    ///     Filter claims by campaign.
    /// </summary>
    public string? CampaignId { get; set; }

    /// <summary>
    ///     Maximum number of claims to retrieve (default: 10, max: 1000).
    /// </summary>
    public int? Limit { get; set; }

    /// <summary>
    ///     Pagination cursor (based on claim_id) to fetch the next page of results.
    /// </summary>
    public string? Cursor { get; set; }

    /// <summary>
    ///     Filter claims by user.
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    ///     Filter by a specific claim.
    /// </summary>
    public string? ClaimId { get; set; }

    /// <summary>
    ///     Filter claims by their external status.
    /// </summary>
    public string? ExternalStatus { get; set; }
}
