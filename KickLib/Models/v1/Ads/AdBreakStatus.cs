using Newtonsoft.Json;

namespace KickLib.Models.v1.Ads;

/// <summary>
///     Represents the ad break status information for a channel.
/// </summary>
public class AdBreakStatus
{
    /// <summary>
    ///     Whether the channel is enrolled in ads.
    /// </summary>
    [JsonProperty(PropertyName = "opted_in")]
    public bool OptedIn { get; set; }

    /// <summary>
    ///     Whether ads are currently blocked for the channel.
    /// </summary>
    [JsonProperty(PropertyName = "ads_blocked")]
    public bool AdsBlocked { get; set; }

    /// <summary>
    ///     Number of ad breaks remaining that can be created within the current limit period.
    /// </summary>
    [JsonProperty(PropertyName = "remaining_ad_breaks")]
    public int RemainingAdBreaks { get; set; }

    /// <summary>
    ///     Ad breaks that have been created for the channel.
    /// </summary>
    [JsonProperty(PropertyName = "ad_breaks")]
    public ICollection<AdBreakStatusItem> AdBreaks { get; set; } = [];

    /// <summary>
    ///     Ad break creation limits.
    /// </summary>
    public AdBreakLimits Limits { get; set; } = new();
}
