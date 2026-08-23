using FluentResults;
using KickLib.Auth;

namespace KickLib.Tests.Infrastructure;

/// <summary>
///     Stub used when an Api class is constructed with a pre-set access token and should
///     never need to reach out to the OAuth generator. Throws if actually invoked.
/// </summary>
internal class NoOpKickOAuthGenerator : IKickOAuthGenerator
{
    public Uri GetAuthorizationUri(string redirectUri, string clientId, ICollection<string> scopes, out string verifier, string? state = null)
        => throw new NotSupportedException();

    public Task<Result<KickTokenResponse>> ExchangeCodeForTokenAsync(string code, string clientId, string clientSecret, string redirectUrl, string state, string? verifier = null)
        => throw new NotSupportedException();

    public Task<Result<KickAppTokenResponse>> GenerateAppAccessTokenAsync(string clientId, string clientSecret)
        => throw new NotSupportedException();

    public Task<Result<KickTokenResponse>> RefreshAccessTokenAsync(string refreshToken, string clientId, string clientSecret)
        => throw new NotSupportedException();

    public Task<Result<bool>> RevokeAccessTokenAsync(string tokenToRevoke)
        => throw new NotSupportedException();

    public Task<Result<bool>> RevokeRefreshTokenAsync(string tokenToRevoke)
        => throw new NotSupportedException();

    public Task<Result<bool>> RevokeTokenAsync(string tokenToRevoke, bool isAccessToken)
        => throw new NotSupportedException();
}
