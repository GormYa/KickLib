using Newtonsoft.Json;

namespace KickLib.Models.v1.Ads;

/// <summary>
///     Represents ad break creation limits.
/// </summary>
public class AdBreakLimits
{
    /// <summary>
    ///     Maximum number of ad breaks allowed within the period.
    /// </summary>
    public int Max { get; set; }

    /// <summary>
    ///     Length of the limiting period, in seconds.
    /// </summary>
    [JsonProperty(PropertyName = "period_seconds")]
    public int PeriodSeconds { get; set; }
}
