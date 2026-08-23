using System.Net;
using FluentAssertions;
using KickLib.Api;
using KickLib.Core;
using KickLib.Tests.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;

namespace KickLib.Tests.Api;

public class AdsTests
{
    private const string SuccessAdBreakResponse =
        """{"data":{"id":"123e4567-e89b-12d3-a456-426614174000","remaining_ad_breaks":2},"message":"Accepted"}""";

    private const string SuccessAdBreakStatusResponse =
        """{"data":{"opted_in":true,"ads_blocked":false,"remaining_ad_breaks":2,"ad_breaks":[],"limits":{"max":3,"period_seconds":3600}},"message":"OK"}""";

    [Theory]
    [InlineData(6)]
    [InlineData(301)]
    public async Task CreateAdBreakAsync_WithDurationOutOfRange_ReturnsFailureWithoutCallingApi(int duration)
    {
        var (ads, handler) = CreateSut(HttpStatusCode.OK, SuccessAdBreakResponse);

        var result = await ads.CreateAdBreakAsync(duration);

        result.IsFailed.Should().BeTrue();
        handler.CallCount.Should().Be(0);
    }

    [Fact]
    public async Task CreateAdBreakAsync_WithoutId_GeneratesNewGuidInRequestPayload()
    {
        var (ads, handler) = CreateSut(HttpStatusCode.OK, SuccessAdBreakResponse);

        await ads.CreateAdBreakAsync(30);

        var body = JObject.Parse(handler.LastRequestBody!);
        Guid.TryParse(body["id"]!.Value<string>(), out _).Should().BeTrue();
    }

    [Fact]
    public async Task CreateAdBreakAsync_WithExplicitId_SendsThatIdInRequestPayload()
    {
        var (ads, handler) = CreateSut(HttpStatusCode.OK, SuccessAdBreakResponse);
        var id = Guid.Parse("11111111-1111-1111-1111-111111111111");

        await ads.CreateAdBreakAsync(30, id);

        var body = JObject.Parse(handler.LastRequestBody!);
        body["id"]!.Value<string>().Should().Be(id.ToString());
    }

    [Fact]
    public async Task CreateAdBreakAsync_SendsPostToCorrectUrlWithBreakDuration()
    {
        var (ads, handler) = CreateSut(HttpStatusCode.OK, SuccessAdBreakResponse);

        await ads.CreateAdBreakAsync(45);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.ToString().Should().EndWith("v1/ads/ad-break");

        var body = JObject.Parse(handler.LastRequestBody!);
        body["break_duration_seconds"]!.Value<int>().Should().Be(45);
    }

    [Fact]
    public async Task CreateAdBreakAsync_OnSuccess_ReturnsDeserializedAdBreak()
    {
        var (ads, _) = CreateSut(HttpStatusCode.OK, SuccessAdBreakResponse);

        var result = await ads.CreateAdBreakAsync(30);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(Guid.Parse("123e4567-e89b-12d3-a456-426614174000"));
        result.Value.RemainingAdBreaks.Should().Be(2);
    }

    [Fact]
    public async Task CreateAdBreakAsync_On403_AddsMissingScopeError()
    {
        var (ads, _) = CreateSut(HttpStatusCode.Forbidden, """{"message":"Forbidden"}""");

        var result = await ads.CreateAdBreakAsync(30);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "Missing scope: ads:write");
    }

    [Fact]
    public async Task GetAdBreakStatusAsync_SendsGetToCorrectUrl()
    {
        var (ads, handler) = CreateSut(HttpStatusCode.OK, SuccessAdBreakStatusResponse);

        await ads.GetAdBreakStatusAsync();

        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.ToString().Should().EndWith("v1/ads/ad-break-status");
    }

    [Fact]
    public async Task GetAdBreakStatusAsync_OnSuccess_ReturnsDeserializedStatus()
    {
        var (ads, _) = CreateSut(HttpStatusCode.OK, SuccessAdBreakStatusResponse);

        var result = await ads.GetAdBreakStatusAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.OptedIn.Should().BeTrue();
        result.Value.RemainingAdBreaks.Should().Be(2);
    }

    [Fact]
    public async Task GetAdBreakStatusAsync_On403_AddsCombinedMissingScopeError()
    {
        var (ads, _) = CreateSut(HttpStatusCode.Forbidden, """{"message":"Forbidden"}""");

        var result = await ads.GetAdBreakStatusAsync();

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "Missing scope: ads:read or ads:write");
    }

    [Fact]
    public async Task EnrollInAdsAsync_SendsPostWithoutBody()
    {
        var (ads, handler) = CreateSut(HttpStatusCode.OK, """{"data":{},"message":"OK"}""");

        await ads.EnrollInAdsAsync();

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.ToString().Should().EndWith("v1/ads/enroll");
        handler.LastRequestBody.Should().BeNull();
    }

    [Fact]
    public async Task EnrollInAdsAsync_OnSuccess_ReturnsOk()
    {
        var (ads, _) = CreateSut(HttpStatusCode.OK, """{"data":{},"message":"OK"}""");

        var result = await ads.EnrollInAdsAsync();

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task EnrollInAdsAsync_On403_AddsMissingScopeError()
    {
        var (ads, _) = CreateSut(HttpStatusCode.Forbidden, """{"message":"Forbidden"}""");

        var result = await ads.EnrollInAdsAsync();

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "Missing scope: ads:write");
    }

    private static (Ads Ads, FakeHttpMessageHandler Handler) CreateSut(HttpStatusCode statusCode, string responseBody)
    {
        var handler = new FakeHttpMessageHandler(statusCode, responseBody);
        var factory = new FakeHttpClientFactory(handler);
        var settings = new ApiSettings { AccessToken = "test-token" };

        var ads = new Ads(settings, new NoOpKickOAuthGenerator(), factory, NullLogger<Ads>.Instance);

        return (ads, handler);
    }
}
