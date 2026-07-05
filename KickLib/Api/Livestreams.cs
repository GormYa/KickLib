using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1;
using KickLib.Models.v1.Livestreams;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc cref="ILivestreams" />
public class Livestreams : ApiBase, ILivestreams
{
    private const string ApiUrlPart = "livestreams";
    private const string UsersLivestreamsUrlPart = "users/livestreams";

    /// <inheritdoc />
    public Livestreams(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger<Livestreams> logger)
        : base(settings, oauthGenerator, clientFactory, logger)
    {
    }

    /// <inheritdoc />
    [Obsolete("V1 livestreams endpoint is deprecated and will be removed in a future version. Use the V2 overloads (cursor-based) instead.")]
    public Task<Result<ICollection<LivestreamResponse>>> GetLivestreamsAsync(
        int? broadcasterId = null,
        int? categoryId = null,
        string? language = null,
        int? limit = null,
        LivestreamSorting? sort = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var broadcasterIds = broadcasterId is null
            ? []
            : new List<int> { broadcasterId.Value };

        return GetLivestreamsAsync(broadcasterIds, categoryId, language, limit, sort, accessToken, cancellationToken);
    }

    /// <inheritdoc />
    [Obsolete("V1 livestreams endpoint is deprecated and will be removed in a future version. Use the V2 overloads (cursor-based) instead.")]
    public Task<Result<ICollection<LivestreamResponse>>> GetLivestreamsAsync(
        ICollection<int> broadcasterIds,
        int? categoryId = null,
        string? language = null,
        int? limit = null,
        LivestreamSorting? sort = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        if (broadcasterIds?.Any() == true)
        {
            foreach (var id in broadcasterIds.Distinct())
            {
                query.Add(new("broadcaster_user_id", id.ToString()));
            }
        }

        if (categoryId.HasValue)
        {
            query.Add(new("category_id", categoryId.ToString()!));
        }

        if (!string.IsNullOrWhiteSpace(language))
        {
            query.Add(new("language", language));
        }

        if (limit.HasValue)
        {
            if (limit < 1 || limit > 100)
            {
                return Task.FromResult(Result.Fail<ICollection<LivestreamResponse>>("Limit must be value between 1 and 100!"));
            }

            query.Add(new("limit", limit.ToString()!));
        }

        if (sort.HasValue)
        {
            var sortValue = sort switch
            {
                LivestreamSorting.ByViewerCount => "viewer_count",
                LivestreamSorting.ByStartTime => "started_at",
                _ => throw new ArgumentOutOfRangeException(nameof(sort), sort, null)
            };

            query.Add(new("sort", sortValue));
        }

        // v1/livestreams
        return GetAsync<ICollection<LivestreamResponse>>(ApiUrlPart, ApiVersion.v1, query, accessToken, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Result<PaginatedResponse<ICollection<LivestreamResponseV2>>>> GetLivestreamsAsync(
        string? cursor,
        int? limit = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        if (!string.IsNullOrWhiteSpace(cursor))
        {
            query.Add(new("cursor", cursor));
        }

        if (limit.HasValue)
        {
            if (limit < 1 || limit > 1000)
            {
                return Task.FromResult(Result.Fail<PaginatedResponse<ICollection<LivestreamResponseV2>>>("Limit must be value between 1 and 1000!"));
            }

            query.Add(new("limit", limit.Value.ToString()));
        }

        return GetLivestreamsInternalAsync(query, accessToken, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Result<PaginatedResponse<ICollection<LivestreamResponseV2>>>> GetLivestreamsAsync(
        GetLivestreamsRequest request,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentException("Request cannot be null.", nameof(request));
        }

        if (request.Limit.HasValue && (request.Limit < 1 || request.Limit > 1000))
        {
            return Task.FromResult(Result.Fail<PaginatedResponse<ICollection<LivestreamResponseV2>>>("Limit must be value between 1 and 1000!"));
        }

        if (request.CategoryIds != null && request.CategoryIds.Distinct().Count() > 25)
        {
            return Task.FromResult(Result.Fail<PaginatedResponse<ICollection<LivestreamResponseV2>>>("A maximum of 25 category IDs can be provided!"));
        }

        if (request.LanguageCodes != null && request.LanguageCodes.Distinct().Count() > 25)
        {
            return Task.FromResult(Result.Fail<PaginatedResponse<ICollection<LivestreamResponseV2>>>("A maximum of 25 language codes can be provided!"));
        }

        var query = new List<KeyValuePair<string, string>>();
        if (request.Limit.HasValue)
        {
            query.Add(new("limit", request.Limit.Value.ToString()));
        }

        if (!string.IsNullOrWhiteSpace(request.Cursor))
        {
            query.Add(new("cursor", request.Cursor));
        }

        if (request.CategoryIds != null && request.CategoryIds.Any())
        {
            query.AddRange(request.CategoryIds.Distinct().Select(id => new KeyValuePair<string, string>("category_id", id.ToString())));
        }

        if (request.LanguageCodes != null && request.LanguageCodes.Any())
        {
            query.AddRange(request.LanguageCodes.Distinct().Select(code => new KeyValuePair<string, string>("language_code", code)));
        }

        return GetLivestreamsInternalAsync(query, accessToken, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Result<LivestreamResponse?>> GetLivestreamAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        // v1/livestreams/stats
        var urlPart = $"{ApiUrlPart}/stats";

        var result = await GetAsync<ICollection<LivestreamResponse>>(urlPart, ApiVersion.v1, null, accessToken, cancellationToken)
            .ConfigureAwait(false);

        if (result.IsFailed)
        {
            return Result.Fail<LivestreamResponse?>(result.Errors);
        }

        return Result.Ok(
            result.Value.Any() ? result.Value.First() : null)
            .WithSuccesses(result.Successes);
    }

    /// <inheritdoc />
    public Task<Result<ICollection<LivestreamResponseV2>>> GetLivestreamsByUserIdsAsync(
        ICollection<long> userIds,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (userIds is null || !userIds.Any())
        {
            return Task.FromResult(Result.Fail<ICollection<LivestreamResponseV2>>("At least one user_id is required!"));
        }

        var distinctIds = userIds.Distinct().ToList();
        if (distinctIds.Count > 100)
        {
            return Task.FromResult(Result.Fail<ICollection<LivestreamResponseV2>>("A maximum of 100 user IDs can be provided!"));
        }

        var query = distinctIds
            .Select(id => new KeyValuePair<string, string>("user_id", id.ToString()))
            .ToList();

        // v1/users/livestreams
        return GetAsync<ICollection<LivestreamResponseV2>>(UsersLivestreamsUrlPart, ApiVersion.v1, query, accessToken, cancellationToken);
    }

    private async Task<Result<PaginatedResponse<ICollection<LivestreamResponseV2>>>> GetLivestreamsInternalAsync(
        List<KeyValuePair<string, string>> query,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        // v2/livestreams
        var result = await GetAsync<ICollection<LivestreamResponseV2>>(ApiUrlPart, ApiVersion.v2, query, accessToken, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
        {
            return Result.Fail<PaginatedResponse<ICollection<LivestreamResponseV2>>>(result.Errors);
        }

        var pagination = result.Successes.OfType<ResponseMetadata>().FirstOrDefault()?.GetPagination();

        return Result.Ok(new PaginatedResponse<ICollection<LivestreamResponseV2>>
        {
            Data = result.Value,
            NextCursor = pagination?.NextCursor
        });
    }
}
