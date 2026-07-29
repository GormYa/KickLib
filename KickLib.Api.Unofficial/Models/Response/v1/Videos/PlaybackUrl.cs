using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Videos;

public class PlaybackUrl
{
    public string Dvr { get; set; } = string.Empty;

    public string Live { get; set; } = string.Empty;

    public string Thumbnail { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "video_aspect")]
    public string VideoAspect { get; set; } = string.Empty;

    public string Vod { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "vod_session")]
    public string VodSession { get; set; } = string.Empty;
}