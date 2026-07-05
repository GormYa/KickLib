using Newtonsoft.Json;

namespace KickLib.Models.v1.Drops;

/// <summary>
///     Represents a Drops campaign reward claim.
/// </summary>
public class ClaimResponse
{
    /// <summary>
    ///     Unique identifier of the claim.
    /// </summary>
    [JsonProperty(PropertyName = "claim_id")]
    public string ClaimId { get; set; } = string.Empty;

    /// <summary>
    ///     Unique identifier of the user who claimed the reward.
    /// </summary>
    [JsonProperty(PropertyName = "user_id")]
    public int UserId { get; set; }

    /// <summary>
    ///     Unique identifier of the campaign the claim belongs to.
    /// </summary>
    [JsonProperty(PropertyName = "campaign_id")]
    public string CampaignId { get; set; } = string.Empty;

    /// <summary>
    ///     Unique identifier of the reward that was claimed.
    /// </summary>
    [JsonProperty(PropertyName = "reward_id")]
    public string RewardId { get; set; } = string.Empty;

    /// <summary>
    ///     Identifier of the reward in your external system, if one was set on the reward.
    /// </summary>
    [JsonProperty(PropertyName = "external_id")]
    public string? ExternalId { get; set; }

    /// <summary>
    ///     Fulfillment status of the claim in your external system.
    /// </summary>
    [JsonProperty(PropertyName = "external_status")]
    public string? ExternalStatus { get; set; }

    /// <summary>
    ///     When the claim was created.
    /// </summary>
    [JsonProperty(PropertyName = "created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    ///     When the claim was last updated.
    /// </summary>
    [JsonProperty(PropertyName = "updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
