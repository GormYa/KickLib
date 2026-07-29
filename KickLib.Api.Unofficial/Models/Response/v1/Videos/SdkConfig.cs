using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Videos;

public class SdkConfig
{
    [JsonProperty(PropertyName = "initiate_sdk")]
    public bool InitiateSdk { get; set; }

    [JsonProperty(PropertyName = "sdk_available")]
    public bool SdkAvailable { get; set; }
}