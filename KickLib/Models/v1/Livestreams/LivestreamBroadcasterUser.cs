using Newtonsoft.Json;

namespace KickLib.Models.v1.Livestreams;

/// <summary>
///     Broadcaster user information returned in a V2 livestream response.
/// </summary>
public class LivestreamBroadcasterUser
{
    /// <summary>
    ///     Unique identifier of the broadcaster.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Broadcaster's profile picture URL.
    /// </summary>
    [JsonProperty(PropertyName = "profile_picture")]
    public string ProfilePicture { get; set; } = string.Empty;

    /// <summary>
    ///     Broadcaster's username.
    /// </summary>
    public string Username { get; set; } = string.Empty;
}
