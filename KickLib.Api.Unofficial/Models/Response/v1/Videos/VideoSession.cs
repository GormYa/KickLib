using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Videos;

public class VideoSession
{
    [JsonProperty(PropertyName = "auto_ads_enabled")]
    public bool AutoAdsEnabled { get; set; }

    [JsonProperty(PropertyName = "creator_id")]
    public string CreatorId { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "video_aspect")]
    public string VideoAspect { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "video_cdn")]
    public string VideoCdn { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "video_channel_tier")]
    public string VideoChannelTier { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "video_content_type")]
    public string VideoContentType { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "video_duration")]
    public int VideoDuration { get; set; }

    [JsonProperty(PropertyName = "video_encoding_variant")]
    public string VideoEncodingVariant { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "video_encryption_type")]
    public string VideoEncryptionType { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "video_id")]
    public string VideoId { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "video_language_code")]
    public string VideoLanguageCode { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "video_origin")]
    public string VideoOrigin { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "video_series")]
    public string VideoSeries { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "video_stream_monetised")]
    public string VideoStreamMonetised { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "video_stream_status")]
    public string VideoStreamStatus { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "video_title")]
    public string VideoTitle { get; set; } = string.Empty;
}