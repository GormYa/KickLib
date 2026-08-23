using System.Net;
using FluentAssertions;
using KickLib.Api;
using KickLib.Core;
using KickLib.Tests.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;

namespace KickLib.Tests.Api;

public class ModerationTests
{
    private const string SuccessResponse = """{"data":{},"message":"OK"}""";

    [Fact]
    public async Task BanUserAsync_WithReasonLongerThan100Characters_ReturnsFailureWithoutCallingApi()
    {
        var (moderation, handler) = CreateSut(HttpStatusCode.OK, SuccessResponse);

        var result = await moderation.BanUserAsync(1, 2, new string('a', 101));

        result.IsFailed.Should().BeTrue();
        handler.CallCount.Should().Be(0);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10081)]
    public async Task TimeoutUserAsync_WithDurationOutOfRange_ReturnsFailureWithoutCallingApi(int duration)
    {
        var (moderation, handler) = CreateSut(HttpStatusCode.OK, SuccessResponse);

        var result = await moderation.TimeoutUserAsync(1, 2, duration);

        result.IsFailed.Should().BeTrue();
        handler.CallCount.Should().Be(0);
    }

    [Fact]
    public async Task BanUserAsync_SendsExpectedPayloadWithoutDuration()
    {
        var (moderation, handler) = CreateSut(HttpStatusCode.OK, SuccessResponse);

        await moderation.BanUserAsync(broadcasterUserId: 111, userIdToBan: 222, reason: "Spam");

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        var body = JObject.Parse(handler.LastRequestBody!);
        body["broadcaster_user_id"]!.Value<int>().Should().Be(111);
        body["user_id"]!.Value<int>().Should().Be(222);
        body["reason"]!.Value<string>().Should().Be("Spam");
        body.ContainsKey("duration").Should().BeFalse();
    }

    [Fact]
    public async Task TimeoutUserAsync_SendsExpectedPayloadWithDuration()
    {
        var (moderation, handler) = CreateSut(HttpStatusCode.OK, SuccessResponse);

        await moderation.TimeoutUserAsync(broadcasterUserId: 111, userIdToBan: 222, duration: 30);

        var body = JObject.Parse(handler.LastRequestBody!);
        body["duration"]!.Value<int>().Should().Be(30);
    }

    [Fact]
    public async Task BanUserAsync_On403_AddsMissingScopeError()
    {
        var (moderation, _) = CreateSut(HttpStatusCode.Forbidden, """{"message":"Forbidden"}""");

        var result = await moderation.BanUserAsync(1, 2);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "Missing scope: moderation:ban");
    }

    [Fact]
    public async Task UnbanUserAsync_SendsDeleteWithExpectedPayload()
    {
        var (moderation, handler) = CreateSut(HttpStatusCode.OK, SuccessResponse);

        await moderation.UnbanUserAsync(broadcasterUserId: 111, userIdToUnban: 222);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Delete);
        var body = JObject.Parse(handler.LastRequestBody!);
        body["broadcaster_user_id"]!.Value<int>().Should().Be(111);
        body["user_id"]!.Value<int>().Should().Be(222);
    }

    private static (Moderation Moderation, FakeHttpMessageHandler Handler) CreateSut(HttpStatusCode statusCode, string responseBody)
    {
        var handler = new FakeHttpMessageHandler(statusCode, responseBody);
        var factory = new FakeHttpClientFactory(handler);
        var settings = new ApiSettings { AccessToken = "test-token" };

        var moderation = new Moderation(settings, new NoOpKickOAuthGenerator(), factory, NullLogger<Moderation>.Instance);

        return (moderation, handler);
    }
}
