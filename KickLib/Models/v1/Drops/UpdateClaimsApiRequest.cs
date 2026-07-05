using Newtonsoft.Json;

namespace KickLib.Models.v1.Drops;

internal class UpdateClaimsApiRequest
{
    public ICollection<ClaimStatusUpdateApiRequest> Claims { get; set; } = [];

    internal static UpdateClaimsApiRequest FromRequest(ICollection<ClaimStatusUpdate> claims)
    {
        return new UpdateClaimsApiRequest
        {
            Claims = claims.Select(claim => new ClaimStatusUpdateApiRequest
            {
                ClaimId = claim.ClaimId,
                ExternalStatus = claim.ExternalStatus
            }).ToList()
        };
    }
}

internal class ClaimStatusUpdateApiRequest
{
    [JsonProperty(PropertyName = "claim_id")]
    public string ClaimId { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "external_status")]
    public string ExternalStatus { get; set; } = string.Empty;
}
