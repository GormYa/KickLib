using Newtonsoft.Json;

namespace KickLib.Models.v1.Ads;

/// <summary>
///     Represents a single ad break entry on a channel.
/// </summary>
public class AdBreakStatusItem
{
    /// <summary>
    ///     Unique identifier (UUID) of the ad break.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Status of the ad break (e.g. "inserted").
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    ///     Duration of the ad break, in seconds.
    /// </summary>
    [JsonProperty(PropertyName = "break_duration_seconds")]
    public int BreakDurationSeconds { get; set; }

    /// <summary>
    ///     When the ad break was created.
    /// </summary>
    [JsonProperty(PropertyName = "created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    ///     Source that triggered the ad break (e.g. "public_api").
    /// </summary>
    public string Source { get; set; } = string.Empty;
}
