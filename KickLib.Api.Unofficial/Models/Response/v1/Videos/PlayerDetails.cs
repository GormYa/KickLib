using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Videos;

public class PlayerDetails
{
    [JsonProperty(PropertyName = "player_name")]
    public string PlayerName { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "player_software")]
    public string PlayerSoftware { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "player_software_version")]
    public string PlayerSoftwareVersion { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "player_version")]
    public string PlayerVersion { get; set; } = string.Empty;
}