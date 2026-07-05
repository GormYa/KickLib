namespace KickLib.Models.v1.Drops;

/// <summary>
///     Wire-format wrapper for the nested "data" object returned by the Drops claims endpoint,
///     which holds the claims array and pagination cursor together rather than as separate top-level fields.
/// </summary>
internal class ClaimsPageResponse
{
    public ICollection<ClaimResponse> Claims { get; set; } = [];

    public string? Cursor { get; set; }
}
