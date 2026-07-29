using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Videos;

public class VideoPlayer
{
    [JsonProperty(PropertyName = "datazoom_sdk")]
    public SdkConfig DatazoomSdk { get; set; } = new();

    [JsonProperty(PropertyName = "google_ads_sdk")]
    public SdkConfig GoogleAdsSdk { get; set; } = new();

    [JsonProperty(PropertyName = "mux_sdk")]
    public SdkConfig MuxSdk { get; set; } = new();

    [JsonProperty(PropertyName = "pal_sdk")]
    public SdkConfig PalSdk { get; set; } = new();

    [JsonProperty(PropertyName = "player")]
    public PlayerDetails Player { get; set; } = new();
}