using Newtonsoft.Json;

namespace KickLib.Models.v1.Ads;

/// <summary>
///     Represents a created ad break.
/// </summary>
public class AdBreak
{
    /// <summary>
    ///     Unique identifier (UUID) of the ad break.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Number of ad breaks remaining that can be created within the current limit period.
    /// </summary>
    [JsonProperty(PropertyName = "remaining_ad_breaks")]
    public int RemainingAdBreaks { get; set; }
}
