using System.Net;
using FluentAssertions;
using KickLib.Api;
using KickLib.Core;
using KickLib.Tests.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;

namespace KickLib.Tests.Api;

public class ChannelsTests
{
    private const string TwoChannelsResponse =
        """{"data":[{"broadcaster_user_id":111,"slug":"first"},{"broadcaster_user_id":222,"slug":"second"}],"message":"OK"}""";

    [Fact]
    public async Task GetChannelAsync_ById_SendsExpectedQueryParameter()
    {
        var (channels, handler) = CreateSut(HttpStatusCode.OK, TwoChannelsResponse);

        await channels.GetChannelAsync(111);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.Query.Should().Contain("broadcaster_user_id=111");
    }

    [Fact]
    public async Task GetChannelAsync_ById_ReturnsFirstItemFromCollection()
    {
        var (channels, _) = CreateSut(HttpStatusCode.OK, TwoChannelsResponse);

        var result = await channels.GetChannelAsync(111);

        result.IsSuccess.Should().BeTrue();
        result.Value.BroadcasterUserId.Should().Be(111);
        result.Value.Slug.Should().Be("first");
    }

    [Fact]
    public async Task GetChannelAsync_On403_AddsMissingScopeError()
    {
        var (channels, _) = CreateSut(HttpStatusCode.Forbidden, """{"message":"Forbidden"}""");

        var result = await channels.GetChannelAsync(111);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "Missing scope: channel:read");
    }

    [Fact]
    public async Task GetChannelsAsync_BySlugs_SendsDeduplicatedQueryParameters()
    {
        var (channels, handler) = CreateSut(HttpStatusCode.OK, TwoChannelsResponse);

        await channels.GetChannelsAsync(["first", "second", "first"]);

        var query = handler.LastRequest!.RequestUri!.Query;
        query.Should().Contain("slug=first");
        query.Should().Contain("slug=second");
        query.Split("slug=").Should().HaveCount(3); // 1 prefix split + 2 "slug=" occurrences

    }

    private static (Channels Channels, FakeHttpMessageHandler Handler) CreateSut(HttpStatusCode statusCode, string responseBody)
    {
        var handler = new FakeHttpMessageHandler(statusCode, responseBody);
        var factory = new FakeHttpClientFactory(handler);
        var settings = new ApiSettings { AccessToken = "test-token" };

        var channels = new Channels(settings, new NoOpKickOAuthGenerator(), factory, NullLogger<Channels>.Instance);

        return (channels, handler);
    }
}
