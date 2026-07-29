using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Videos;

public class VideoPlayback
{
    [JsonProperty(PropertyName = "playback_url")]
    public PlaybackUrl PlaybackUrl { get; set; }

    [JsonProperty(PropertyName = "user_session")]
    public UserSession UserSession { get; set; }

    [JsonProperty(PropertyName = "video_player")]
    public VideoPlayer VideoPlayer { get; set; }

    [JsonProperty(PropertyName = "video_session")]
    public VideoSession VideoSession { get; set; }

    public VideoSource Source { get; set; }
}