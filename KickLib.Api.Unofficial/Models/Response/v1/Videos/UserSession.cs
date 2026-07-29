using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Videos;

public class UserSession
{
    [JsonProperty(PropertyName = "page_type")]
    public string PageType { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "player_device_id")]
    public string PlayerDeviceId { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "player_name")]
    public string PlayerName { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "player_remote_played")]
    public bool PlayerRemotePlayed { get; set; }

    [JsonProperty(PropertyName = "player_resettable_consent_type")]
    public string PlayerResettableConsentType { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "player_resettable_id")]
    public string PlayerResettableId { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "session_id")]
    public string SessionId { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "viewer_connection_type")]
    public string ViewerConnectionType { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "viewer_tier")]
    public string ViewerTier { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "viewer_user_id")]
    public string ViewerUserId { get; set; } = string.Empty;
}