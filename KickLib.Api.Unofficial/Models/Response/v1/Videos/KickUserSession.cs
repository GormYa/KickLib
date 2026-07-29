using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Videos;

public class KickUserSession
{
    [JsonProperty(PropertyName = "session_id")]
    public string SessionId { get; set; }
}