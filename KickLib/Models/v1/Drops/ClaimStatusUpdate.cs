namespace KickLib.Models.v1.Drops;

/// <summary>
///     Represents an update to a claim's external status.
/// </summary>
public class ClaimStatusUpdate
{
    /// <summary>
    ///     Unique identifier of the claim to update.
    /// </summary>
    public string ClaimId { get; set; } = string.Empty;

    /// <summary>
    ///     New fulfillment status to record for the claim in your external system.
    /// </summary>
    public string ExternalStatus { get; set; } = string.Empty;
}
