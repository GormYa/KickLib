using System.Net;
using FluentAssertions;
using KickLib.Api;
using KickLib.Api.Interfaces;
using KickLib.Core;
using KickLib.Models.v1.ChannelRewards;
using KickLib.Tests.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;

namespace KickLib.Tests.Api;

public class ChannelRewardsTests
{
    private const string SuccessResponse =
        """{"data":{"id":"01HZ8X9K2M4N6P8Q0R2S4T6V8W","background_color":"#00e701","cost":100,"description":"desc","is_enabled":true,"is_paused":false,"is_user_input_required":true,"should_redemptions_skip_request_queue":true,"title":"Song Request"},"message":"OK"}""";

    [Fact]
    public async Task CreateChannelRewardAsync_SendsExpectedSnakeCasePayload()
    {
        var (channelRewards, handler) = CreateSut(HttpStatusCode.OK, SuccessResponse);
        var request = new CreateChannelRewardRequest(100, "Song Request")
        {
            BackgroundColor = "#00e701",
            Description = "Request a song",
            IsUserInputRequired = true,
            ShouldRedemptionsSkipRequestQueue = true
        };

        await channelRewards.CreateChannelRewardAsync(request);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        var body = JObject.Parse(handler.LastRequestBody!);
        body["title"]!.Value<string>().Should().Be("Song Request");
        body["cost"]!.Value<int>().Should().Be(100);
        body["background_color"]!.Value<string>().Should().Be("#00e701");
        body["description"]!.Value<string>().Should().Be("Request a song");
        body["is_user_input_required"]!.Value<bool>().Should().BeTrue();
        body["should_redemptions_skip_request_queue"]!.Value<bool>().Should().BeTrue();
    }

    [Fact]
    public async Task CreateChannelRewardAsync_OnSuccess_ReturnsDeserializedReward()
    {
        var (channelRewards, _) = CreateSut(HttpStatusCode.OK, SuccessResponse);

        var result = await channelRewards.CreateChannelRewardAsync("Song Request", 100);

        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be("Song Request");
        result.Value.Cost.Should().Be(100);
    }

    [Fact]
    public async Task CreateChannelRewardAsync_On403_AddsMissingScopeError()
    {
        var (channelRewards, _) = CreateSut(HttpStatusCode.Forbidden, """{"message":"Forbidden"}""");

        var result = await channelRewards.CreateChannelRewardAsync("Title", 10);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "Missing scope: channel:rewards:write");
    }

    [Fact]
    public async Task CreateChannelRewardAsync_TwoArgOverload_ResolvesUnambiguouslyThroughInterface()
    {
        var (channelRewards, handler) = CreateSut(HttpStatusCode.OK, SuccessResponse);
        IChannelRewards asInterface = channelRewards;

        var result = await asInterface.CreateChannelRewardAsync("Song Request", 100);

        result.IsSuccess.Should().BeTrue();
        var body = JObject.Parse(handler.LastRequestBody!);
        body["is_enabled"]!.Value<bool>().Should().BeTrue();
    }

    [Fact]
    public async Task DeleteChannelRewardAsync_SendsDeleteToCorrectUrl()
    {
        var (channelRewards, handler) = CreateSut(HttpStatusCode.OK, """{"data":{},"message":"OK"}""");

        await channelRewards.DeleteChannelRewardAsync("reward-123");

        handler.LastRequest!.Method.Should().Be(HttpMethod.Delete);
        handler.LastRequest.RequestUri!.ToString().Should().EndWith("v1/channels/rewards/reward-123");
    }

    private static (ChannelRewards ChannelRewards, FakeHttpMessageHandler Handler) CreateSut(HttpStatusCode statusCode, string responseBody)
    {
        var handler = new FakeHttpMessageHandler(statusCode, responseBody);
        var factory = new FakeHttpClientFactory(handler);
        var settings = new ApiSettings { AccessToken = "test-token" };

        var channelRewards = new ChannelRewards(settings, new NoOpKickOAuthGenerator(), factory, NullLogger<ChannelRewards>.Instance);

        return (channelRewards, handler);
    }
}
