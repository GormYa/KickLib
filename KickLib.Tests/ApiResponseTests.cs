using FluentAssertions;
using KickLib.Core;
using KickLib.Models;
using KickLib.Models.v1.Ads;
using KickLib.Models.v1.Auth;
using KickLib.Models.v1.Categories;
using KickLib.Models.v1.ChannelRewards;
using KickLib.Models.v1.ChannelRewards.Redemptions;
using KickLib.Models.v1.Channels;
using KickLib.Models.v1.Chat;
using KickLib.Models.v1.Drops;
using KickLib.Models.v1.EventSubscriptions;
using KickLib.Models.v1.Livestreams;
using KickLib.Models.v1.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace KickLib.Tests;

/// <summary>
///     Tests for deserializing API responses.
/// </summary>
public class ApiResponseTests : BaseKickLibTests
{
    public ApiResponseTests() : base("Data.ApiResponses")
    {
    }
    
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Converters = new List<JsonConverter>
        {
            new StringEnumConverter(typeof(LowerCaseNamingStrategy))
        }
    };
    
    [Theory]
    [InlineData("GetCategoriesResponse", typeof(CategoryResponse))]
    [InlineData("GetCategoryResponse", typeof(CategoryResponse))]
    public void CorrectlyDeserialize_CategoriesResponses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);
        
        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
        ((CategoryResponse)deserializedObject).Id.Should().Be(101);
        ((CategoryResponse)deserializedObject).Name.Should().Be("Old School Runescape");
        ((CategoryResponse)deserializedObject).Thumbnail.Should().NotBeEmpty();
        
        if (payloadResource == "GetCategoryResponse")
        {
            ((CategoryResponse)deserializedObject).ViewerCount.Should().Be(1000);
            ((CategoryResponse)deserializedObject).Tags.Should().HaveCount(2);
        }
    }
    
    [Theory]
    [InlineData("GetClaimResponse", typeof(ClaimResponse))]
    public void CorrectlyDeserialize_ClaimResponses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);

        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);

        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
        var response = (ClaimResponse)deserializedObject!;
        response.ClaimId.Should().Be("claim_01HZ8X9K2M4N6P8Q0R2S4T6V8W");
        response.UserId.Should().Be(123);
        response.CampaignId.Should().Be("campaign_01HZ8X9K2M4N6P8Q0R2S4T6V8W");
        response.RewardId.Should().Be("reward_01HZ8X9K2M4N6P8Q0R2S4T6V8W");
        response.ExternalId.Should().Be("ext-reward-1");
        response.ExternalStatus.Should().Be("fulfilled");
    }

    [Fact]
    public void CorrectlyDeserialize_ClaimsPaginatedResponse()
    {
        var payload = GetPayload("GetClaimsPaginatedResponse");

        var deserializedObject = JsonConvert.DeserializeObject<DataWrapper<ClaimsPageResponse>>(payload, SerializerSettings)!;

        deserializedObject.Should().NotBeNull();
        deserializedObject.Data.Should().NotBeNull();
        deserializedObject.Data!.Claims.Should().HaveCount(1);
        deserializedObject.Data!.Cursor.Should().Be("01K0TDDR08Q5ZNWXK92SDH7SDH");
    }

    [Theory]
    [InlineData("GetPublicKeyResponse", typeof(PublicKeyResponse))]
    [InlineData("IntrospectTokenResponse", typeof(TokenIntrospectResponse))]
    [InlineData("IntrospectTokenResponse_Inactive", typeof(TokenIntrospectResponse))]
    public void CorrectlyDeserialize_AuthorizationResponses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);
        
        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
    }
    
    [Theory]
    [InlineData("SendChatMessageResponse", typeof(SendChatMessageResponse))]
    public void CorrectlyDeserialize_ChatResponses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);
        
        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
    }
    
    [Theory]
    [InlineData("SubscribeToEventResponse", typeof(SubscribeToEventResponse))]
    public void CorrectlyDeserialize_EventSubscriptionResponses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);
        
        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
    }
    
    [Theory]
    [InlineData("GetChannelsResponse", typeof(ChannelResponse))]
    public void CorrectlyDeserialize_ChannelsResponses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);
        
        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
        ((ChannelResponse)deserializedObject).Stream.CustomTags.Should().NotBeNull();
        ((ChannelResponse)deserializedObject).Stream.CustomTags.Should().HaveCount(2);
        ((ChannelResponse)deserializedObject).ActiveSubscribers.Should().Be(150);
        ((ChannelResponse)deserializedObject).ActiveGiftedSubscribers.Should().Be(40);
        ((ChannelResponse)deserializedObject).CanceledSubscribers.Should().Be(25);
    }

    [Theory]
    [InlineData("GetLivestreamResponse", typeof(LivestreamResponse))]
    public void CorrectlyDeserialize_LivestreamResponses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);
        
        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
        ((LivestreamResponse)deserializedObject).CustomTags.Should().NotBeNull();
        ((LivestreamResponse)deserializedObject).CustomTags.Should().HaveCount(2);
        ((LivestreamResponse)deserializedObject).ProfilePicture.Should().Be("https://kick.com/img/default-profile-pictures/default2.jpeg");
    }
    
    [Theory]
    [InlineData("GetLivestreamsV2Response", typeof(LivestreamResponseV2))]
    public void CorrectlyDeserialize_LivestreamV2Responses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);

        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);

        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
        var response = (LivestreamResponseV2)deserializedObject!;
        response.Id.Should().Be("123e4567-e89b-12d3-a456-426614174000");
        response.Title.Should().Be("My Stream Title");
        response.LanguageCode.Should().Be("en");
        response.ViewerCount.Should().Be(100);
        response.HasMatureContent.Should().BeTrue();
        response.Tags.Should().HaveCount(2);
        response.BroadcasterUser.Should().NotBeNull();
        response.BroadcasterUser.Id.Should().Be(123);
        response.BroadcasterUser.Username.Should().Be("streamer_123");
        response.BroadcasterUser.ProfilePicture.Should().Be("https://example.com/profile.jpg");
        response.Channel.Should().NotBeNull();
        response.Channel.Slug.Should().Be("streamer-123");
        response.Category.Should().NotBeNull();
        response.Category.Name.Should().Be("Old School Runescape");
    }

    [Fact]
    public void CorrectlyDeserialize_LivestreamsV2PaginatedResponse()
    {
        var payload = GetPayload("GetLivestreamsV2PaginatedResponse");

        var deserializedObject = JsonConvert.DeserializeObject<DataWrapper<ICollection<LivestreamResponseV2>>>(payload, SerializerSettings)!;

        deserializedObject.Should().NotBeNull();
        deserializedObject.Data.Should().NotBeNull();
        deserializedObject.Data!.Should().HaveCount(1);
        deserializedObject.Pagination.Should().NotBeNull();
        deserializedObject.Pagination!.NextCursor.Should().Be("cursor_abc123");
    }

    [Fact]
    public void CorrectlyDeserialize_LivestreamsByUserIdsResponse()
    {
        var payload = GetPayload("GetLivestreamsByUserIdsResponse");

        var deserializedObject = JsonConvert.DeserializeObject<DataWrapper<ICollection<LivestreamResponseV2>>>(payload, SerializerSettings)!;

        deserializedObject.Should().NotBeNull();
        deserializedObject.Data.Should().NotBeNull();
        deserializedObject.Data!.Should().HaveCount(1);
        deserializedObject.Pagination.Should().BeNull();

        var response = deserializedObject.Data!.First();
        response.BroadcasterUser.Id.Should().Be(123);
        response.BroadcasterUser.Username.Should().Be("streamer_123");
        response.Title.Should().Be("My Stream Title");
    }

    [Theory]
    [InlineData("GetUsersResponse", typeof(UserResponse))]
    public void CorrectlyDeserialize_UsersResponses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);
        
        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
    }
    
    [Fact]
    public void CorrectlyDeserialize_GetChannelRewards()
    {
        var payload = GetPayload("GetChannelRewardResponse");
        
        var deserializedObject = JsonConvert.DeserializeObject<ChannelReward>(payload, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Id.Should().Be("01HZ8X9K2M4N6P8Q0R2S4T6V8W0Y2Z4");
        deserializedObject.BackgroundColor.Should().Be("#00e701");
        deserializedObject.Cost.Should().Be(100);
        deserializedObject.Description.Should().Be("Request a song by providing a URL");
        deserializedObject.IsEnabled.Should().BeTrue();
        deserializedObject.IsPaused.Should().BeFalse();
        deserializedObject.IsUserInputRequired.Should().BeTrue();
        deserializedObject.ShouldRedemptionsSkipRequestQueue.Should().BeTrue();
        deserializedObject.Title.Should().Be("Song Request");
    }
    
    [Fact]
    public void CorrectlyDeserialize_GetChannelRewardsRedemptions()
    {
        var payload = GetPayload("GetChannelRewardRedemptionsResponse");
        
        var deserializedObject = JsonConvert.DeserializeObject<ChannelRewardRedemption>(payload, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        
        // Test Redemptions collection
        deserializedObject.Redemptions.Should().NotBeNull();
        deserializedObject.Redemptions.Should().HaveCount(1);
        
        var redemption = deserializedObject.Redemptions.First();
        redemption.Id.Should().Be("01KCTMJ2X3DXT814CVX8FR9T1D");
        redemption.RedeemedAt.Should().Be(DateTimeOffset.Parse("2025-12-19T06:26:36Z"));
        redemption.Status.Should().Be(RedemptionStatus.Pending);
        redemption.UserInput.Should().Be("sorry mr strimer");
        
        // Test Redeemer
        redemption.Redeemer.Should().NotBeNull();
        redemption.Redeemer.UserId.Should().Be(123);
        
        // Test Reward
        deserializedObject.Reward.Should().NotBeNull();
        deserializedObject.Reward.Id.Should().Be("01KCTMJ2X3DXT814CVX8FR9T1D");
        deserializedObject.Reward.Title.Should().Be("Unban Request");
        deserializedObject.Reward.Description.Should().Be("Request to have your ban lifted");
        deserializedObject.Reward.Cost.Should().Be(100);
        deserializedObject.Reward.CanManage.Should().BeTrue();
        deserializedObject.Reward.IsDeleted.Should().BeFalse();
    }
    
    [Fact]
    public void CorrectlyDeserialize_AdBreak()
    {
        var payload = GetPayload("GetAdBreakResponse");

        var deserializedObject = JsonConvert.DeserializeObject<AdBreak>(payload, SerializerSettings);

        deserializedObject.Should().NotBeNull();
        deserializedObject!.Id.Should().Be(Guid.Parse("123e4567-e89b-12d3-a456-426614174000"));
        deserializedObject.RemainingAdBreaks.Should().Be(2);
    }

    [Fact]
    public void CorrectlyDeserialize_AdBreakStatus()
    {
        var payload = GetPayload("GetAdBreakStatusResponse");

        var deserializedObject = JsonConvert.DeserializeObject<AdBreakStatus>(payload, SerializerSettings);

        deserializedObject.Should().NotBeNull();
        deserializedObject!.OptedIn.Should().BeTrue();
        deserializedObject.AdsBlocked.Should().BeFalse();
        deserializedObject.RemainingAdBreaks.Should().Be(2);
        deserializedObject.Limits.Max.Should().Be(3);
        deserializedObject.Limits.PeriodSeconds.Should().Be(3600);
        deserializedObject.AdBreaks.Should().HaveCount(1);

        var adBreak = deserializedObject.AdBreaks.First();
        adBreak.Id.Should().Be(Guid.Parse("123e4567-e89b-12d3-a456-426614174000"));
        adBreak.Status.Should().Be("inserted");
        adBreak.BreakDurationSeconds.Should().Be(30);
        adBreak.Source.Should().Be("public_api");
        adBreak.CreatedAt.Should().Be(DateTimeOffset.Parse("2026-01-02T03:04:05Z"));
    }

    [Fact]
    public void CorrectlyDeserialize_WrapperObject()
    {
        var payload = GetPayload("WrapperResponse");
        
        var deserializedObject = JsonConvert.DeserializeObject<DataWrapper<TestClass>>(payload, SerializerSettings)!;
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType<DataWrapper<TestClass>>();
    }

    private class TestClass
    {
        public required string Test { get; set; }
    }
}