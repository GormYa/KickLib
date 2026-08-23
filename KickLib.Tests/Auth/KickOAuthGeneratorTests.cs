using System.Text;
using FluentAssertions;
using KickLib.Auth;

namespace KickLib.Tests.Auth;

public class KickOAuthGeneratorTests
{
    private readonly KickOAuthGenerator _generator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetAuthorizationUri_WithInvalidClientId_ThrowsArgumentException(string? clientId)
    {
        var act = () => _generator.GetAuthorizationUri("https://localhost/callback", clientId!, [KickScopes.UserRead], out _);

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetAuthorizationUri_WithInvalidRedirectUri_ThrowsArgumentException(string? redirectUri)
    {
        var act = () => _generator.GetAuthorizationUri(redirectUri!, "client-id", [KickScopes.UserRead], out _);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetAuthorizationUri_WithNullScopes_ThrowsArgumentNullException()
    {
        var act = () => _generator.GetAuthorizationUri("https://localhost/callback", "client-id", null!, out _);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetAuthorizationUri_WithEmptyScopes_ThrowsArgumentException()
    {
        var act = () => _generator.GetAuthorizationUri("https://localhost/callback", "client-id", [], out _);

        act.Should().Throw<ArgumentException>().WithMessage("*Scopes cannot be empty*");
    }

    [Fact]
    public void GetAuthorizationUri_ReturnsUriWithExpectedQueryParameters()
    {
        var uri = _generator.GetAuthorizationUri(
            "https://localhost/callback",
            "my-client-id",
            [KickScopes.UserRead, KickScopes.ChannelRead],
            out var verifier,
            state: "custom-state");

        uri.ToString().Should().StartWith(KickOAuthGenerator.AuthorizeUrl);
        uri.Query.Should().Contain("client_id=my-client-id");
        uri.Query.Should().Contain("response_type=code");
        uri.Query.Should().Contain("redirect_uri=https://localhost/callback");
        uri.Query.Should().Contain("state=custom-state");
        uri.Query.Should().Contain($"scope={KickScopes.UserRead}%20{KickScopes.ChannelRead}");
        uri.Query.Should().Contain("code_challenge_method=S256");
        uri.Query.Should().Contain("code_challenge=");
        verifier.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GetAuthorizationUri_DeduplicatesScopes()
    {
        var uri = _generator.GetAuthorizationUri(
            "https://localhost/callback",
            "my-client-id",
            [KickScopes.UserRead, KickScopes.UserRead],
            out _,
            state: "state");

        var scopeParam = uri.Query.Split("&scope=")[1].Split('&')[0];
        scopeParam.Should().Be(KickScopes.UserRead);
    }

    [Fact]
    public void GetAuthorizationUri_GeneratesUrlSafeVerifier()
    {
        _generator.GetAuthorizationUri("https://localhost/callback", "client-id", [KickScopes.UserRead], out var verifier);

        verifier.Should().NotBeNullOrWhiteSpace();
        verifier.Should().NotContain("+");
        verifier.Should().NotContain("/");
        verifier.Should().NotContain("=");
    }

    [Fact]
    public void GetAuthorizationUri_MultipleCalls_GenerateDifferentVerifiers()
    {
        _generator.GetAuthorizationUri("https://localhost/callback", "client-id", [KickScopes.UserRead], out var verifier1);
        _generator.GetAuthorizationUri("https://localhost/callback", "client-id", [KickScopes.UserRead], out var verifier2);

        verifier1.Should().NotBe(verifier2);
    }

    [Fact]
    public void GetAuthorizationUri_WithoutExplicitState_EncodesVerifierAsState()
    {
        var uri = _generator.GetAuthorizationUri("https://localhost/callback", "client-id", [KickScopes.UserRead], out var verifier);

        var stateParam = uri.Query.Split("&state=")[1].Split("&scope=")[0];
        var decodedJson = Encoding.UTF8.GetString(Convert.FromBase64String(stateParam));

        decodedJson.Should().Contain(verifier);
    }
}
