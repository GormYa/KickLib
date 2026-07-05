using KickLib.Models.v1.Categories;
using Newtonsoft.Json;

namespace KickLib.Models.v1.Livestreams;

/// <summary>
///     Response when getting livestreams via the V2 API.
/// </summary>
public class LivestreamResponseV2
{
    /// <summary>
    ///     Broadcaster user associated with the livestream.
    /// </summary>
    [JsonProperty(PropertyName = "broadcaster_user")]
    public LivestreamBroadcasterUser BroadcasterUser { get; set; } = new();

    /// <summary>
    ///     Livestream category.
    /// </summary>
    public CategoryResponse Category { get; set; } = new();

    /// <summary>
    ///     Channel hosting the livestream.
    /// </summary>
    public LivestreamChannel Channel { get; set; } = new();

    /// <summary>
    ///     Has livestream mature content?
    /// </summary>
    [JsonProperty(PropertyName = "has_mature_content")]
    public bool HasMatureContent { get; set; }

    /// <summary>
    ///     Unique identifier of the livestream (GUID).
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     BCP 47 language code of the livestream (e.g. "en").
    /// </summary>
    [JsonProperty(PropertyName = "language_code")]
    public string? LanguageCode { get; set; }

    /// <summary>
    ///     When the livestream started.
    /// </summary>
    [JsonProperty(PropertyName = "started_at")]
    public DateTimeOffset? StartedAt { get; set; }

    /// <summary>
    ///     Tags associated with the stream.
    /// </summary>
    public ICollection<string> Tags { get; set; } = [];

    /// <summary>
    ///     Livestream thumbnail URL.
    /// </summary>
    public string? Thumbnail { get; set; }

    /// <summary>
    ///     Livestream title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     Current livestream viewer count.
    /// </summary>
    [JsonProperty(PropertyName = "viewer_count")]
    public int ViewerCount { get; set; }
}
