namespace KickLib.Models.v1.Livestreams;

/// <summary>
///     Channel information returned in a V2 livestream response.
/// </summary>
public class LivestreamChannel
{
    /// <summary>
    ///     Slug identifier of the channel hosting the livestream.
    /// </summary>
    public string Slug { get; set; } = string.Empty;
}
