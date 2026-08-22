using Newtonsoft.Json;

namespace KickLib.Models.v1.Ads;

internal class CreateAdBreakPayload
{
    public Guid Id { get; set; }

    [JsonProperty(PropertyName = "break_duration_seconds")]
    public int BreakDurationSeconds { get; set; }
}
