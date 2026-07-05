namespace KickLib.Models.v1.Livestreams;

/// <summary>
///     Request object for retrieving V2 livestreams.
/// </summary>
public class GetLivestreamsRequest
{
    /// <summary>
    ///     Maximum number of livestreams to retrieve (min: 1, max: 1000, default: 100).
    /// </summary>
    public int? Limit { get; set; }

    /// <summary>
    ///     Cursor value to get the next page of results.
    /// </summary>
    public string? Cursor { get; set; }

    /// <summary>
    ///     Filter for livestreams in specific categories. A maximum of 25 category IDs can be provided.
    /// </summary>
    public ICollection<long>? CategoryIds { get; set; }

    /// <summary>
    ///     Filter for livestreams in specific languages. Must be valid BCP 47 language tags (e.g. "en", "de"). A maximum of 25 language codes can be provided.
    /// </summary>
    public ICollection<string>? LanguageCodes { get; set; }
}
