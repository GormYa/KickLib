using FluentAssertions;
using KickLib.Models.v1.EventSubscriptions;
using KickLib.Webhooks;
using KickLib.Webhooks.Payloads;

namespace KickLib.Tests;

public class EventParserTests : BaseKickLibTests
{
    public EventParserTests() : base("Data.WebhookPayloads")
    {
    }
    
    public static IEnumerable<object[]> ParserInputData =>
        new List<object[]>
        {
            new object[] { "ChatMessageSentEventPayload", EventType.ChatMessageSent, typeof(ChatMessageSentEvent) },
            new object[] { "ChannelFollowedEventPayload", EventType.ChannelFollowed, typeof(ChannelFollowedEvent) },
            new object[] { "ChannelGiftedSubscriptionEventPayload", EventType.ChannelSubscriptionGifts, typeof(ChannelGiftedSubscriptionEvent) },
            new object[] { "ChannelNewSubscriptionEventPayload", EventType.ChannelSubscriptionNew, typeof(ChannelNewSubscriptionEvent) },
            new object[] { "ChannelSubscriptionRenewalEventPayload", EventType.ChannelSubscriptionRenewal, typeof(ChannelSubscriptionRenewalEvent) },
            new object[] { "ChannelRewardRedemptionUpdatedEventPayload", EventType.ChannelRewardRedemptionUpdated, typeof(ChannelRewardRedemptionUpdatedEvent) },
            new object[] { "LivestreamStatusUpdatedEventPayload_Live", EventType.LivestreamStatusUpdated, typeof(LivestreamStatusUpdatedEvent) },
            new object[] { "LivestreamMetadataUpdatedEventPayload", EventType.LivestreamMetadataUpdated, typeof(LivestreamMetadataUpdatedEvent) },
            new object[] { "ModerationUserBannedEventPayload", EventType.ModerationUserBanned, typeof(ModerationUserBannedEvent) },
            new object[] { "KicksGiftedEventPayload", EventType.KicksGifted, typeof(KicksGiftedEvent) }
        };
    
    [Fact]
    public void WebhookEventParser_ParseChatPayload()
    {
        var payload = GetPayload("ChatMessageSentEventPayload");
        var webhookEvent = WebhookEventParser.ParseChatMessageSentEvent(payload);
        
        webhookEvent.Should().NotBeNull();
        webhookEvent.MessageId.Should().NotBeNull();
        webhookEvent.Broadcaster.Should().NotBeNull();
        webhookEvent.Sender.Should().NotBeNull();
        webhookEvent.Content.Should().NotBeNull();
    }
    
    [Fact]
    public void WebhookEventParser_ParseLivestreamMetadataUpdatedPayload()
    {
        var payload = GetPayload("LivestreamMetadataUpdatedEventPayload");
        var webhookEvent = WebhookEventParser.ParseLivestreamMetadataUpdatedEvent(payload);
        
        webhookEvent.Should().NotBeNull();
        webhookEvent.Broadcaster.Should().NotBeNull();
        webhookEvent.Metadata.Should().NotBeNull();
        webhookEvent.Metadata.Title.Should().NotBeNull();
        webhookEvent.Metadata.Category.Should().NotBeNull();
        webhookEvent.Metadata.Category.Id.Should().BeGreaterThan(0);
        webhookEvent.Metadata.Category.Name.Should().NotBeNull();
        webhookEvent.Metadata.Category.Thumbnail.Should().NotBeNull();
        webhookEvent.Metadata.Language.Should().NotBeNull();
        webhookEvent.Metadata.Language.Should().Be("en");
        webhookEvent.Metadata.HasMatureContent.Should().BeTrue();
    }
    
    [Theory]
    [MemberData(nameof(ParserInputData))]
    public void WebhookEventParser_ParseCorrectType(string payloadResource, EventType eventType, Type eventObjectType)
    {
        var payload = GetPayload(payloadResource);

        var webhookEvent = WebhookEventParser.Parse(eventType, payload);
        
        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
        webhookEvent.Should().BeOfType(eventObjectType);
    }
    
    [Theory]
    [MemberData(nameof(ParserInputData))]
    public void WebhookEventParser_TryParse_ReturnsCorrectValue(string payloadResource, EventType eventType, Type eventObjectType)
    {
        var payload = GetPayload(payloadResource);

        var success = WebhookEventParser.TryParse(eventType, payload, out var webhookEvent);
        
        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
        success.Should().BeTrue();
        webhookEvent.Should().BeOfType(eventObjectType);
    }

    [Fact]
    public void WebhookEventInfo_ValidateKickSignature()
    {
        var payload = GetPayload("ValidationPayload");

        const string signature = "fpZCxfE8lojfMhDPvSpmEjHbJH4+6OFVSLStKgiTxH7QXQw/M3sdWWl0o/pxBz0vA9xXP8x3l+z7WNkT3C+6K7MkEZBtvv+88IAgWyJ2uTLKJtuFn5FIIQKTv1tAqOeFIp1A56DJR9eJ/yzG+flj9RwSNcvMPXBHS3X5jisBiKhYrqUUAW6HYuYKMq5cTcxb1IX0hyN5jEkFv2BuWAIlriyVztdXBX1aHENBxCSf1qbFzQ26VCaZNCOGPpLS+4kHzuU8Zkju+o4nAUm+DIC8c1CjYfPIwu/tZb2HPGklXt1ZMQXpnP+F/Oo+NaW8Z0fBl1ZG8wanIVjPClkoDR4QZQ==";
        const string subscriptionId = "01JQ79DGGK8C9117GJN8EHCYGG";
        const string messageId = "01JQR5KV0QC94HMETWYNBWRW4Z";
        const string timestamp = "2025-04-01T07:56:19Z";
        
        var eventInfo = new WebhookEventInfo(
            WebhookEventTypes.LivestreamStatusUpdated,
            1,
            timestamp,
            signature,
            subscriptionId,
            messageId
        );

        eventInfo.ValidateSender(payload).Should().BeTrue();
    }

    [Fact]
    public void WebhookEventInfo_ValidateKickSignature_WithTamperedPayload_ReturnsFalse()
    {
        var payload = GetPayload("ValidationPayload") + " ";

        var eventInfo = CreateValidEventInfo();

        eventInfo.ValidateSender(payload).Should().BeFalse();
    }

    [Fact]
    public void WebhookEventInfo_ValidateKickSignature_WithWrongMessageId_ReturnsFalse()
    {
        var payload = GetPayload("ValidationPayload");

        var eventInfo = CreateValidEventInfo(messageId: "01JQR5KV0QC94HMETWYNBWRW4X");

        eventInfo.ValidateSender(payload).Should().BeFalse();
    }

    [Fact]
    public void WebhookEventInfo_ValidateKickSignature_WithWrongTimestamp_ReturnsFalse()
    {
        var payload = GetPayload("ValidationPayload");

        var eventInfo = CreateValidEventInfo(timestamp: "2025-04-01T07:56:20Z");

        eventInfo.ValidateSender(payload).Should().BeFalse();
    }

    [Fact]
    public void WebhookEventInfo_ValidateKickSignature_WithMalformedSignature_ReturnsFalse()
    {
        var payload = GetPayload("ValidationPayload");

        var eventInfo = CreateValidEventInfo(signature: "not-a-valid-base64-signature!!!");

        eventInfo.ValidateSender(payload).Should().BeFalse();
    }

    [Fact]
    public void WebhookEventInfo_ValidateKickSignature_WithWrongSignature_ReturnsFalse()
    {
        var payload = GetPayload("ValidationPayload");

        // Valid base64, but not a signature for this payload.
        var eventInfo = CreateValidEventInfo(signature: Convert.ToBase64String(new byte[256]));

        eventInfo.ValidateSender(payload).Should().BeFalse();
    }

    [Fact]
    public void WebhookEventInfo_ValidateKickSignature_WithInvalidCustomPublicKey_Throws()
    {
        var payload = GetPayload("ValidationPayload");

        var eventInfo = CreateValidEventInfo();

        var act = () => eventInfo.ValidateSender(payload, "not-a-valid-pem-key");

        act.Should().Throw<Exception>();
    }

    private static WebhookEventInfo CreateValidEventInfo(
        string? timestamp = null,
        string? signature = null,
        string? messageId = null)
    {
        const string defaultSignature = "fpZCxfE8lojfMhDPvSpmEjHbJH4+6OFVSLStKgiTxH7QXQw/M3sdWWl0o/pxBz0vA9xXP8x3l+z7WNkT3C+6K7MkEZBtvv+88IAgWyJ2uTLKJtuFn5FIIQKTv1tAqOeFIp1A56DJR9eJ/yzG+flj9RwSNcvMPXBHS3X5jisBiKhYrqUUAW6HYuYKMq5cTcxb1IX0hyN5jEkFv2BuWAIlriyVztdXBX1aHENBxCSf1qbFzQ26VCaZNCOGPpLS+4kHzuU8Zkju+o4nAUm+DIC8c1CjYfPIwu/tZb2HPGklXt1ZMQXpnP+F/Oo+NaW8Z0fBl1ZG8wanIVjPClkoDR4QZQ==";
        const string defaultSubscriptionId = "01JQ79DGGK8C9117GJN8EHCYGG";
        const string defaultMessageId = "01JQR5KV0QC94HMETWYNBWRW4Z";
        const string defaultTimestamp = "2025-04-01T07:56:19Z";

        return new WebhookEventInfo(
            WebhookEventTypes.LivestreamStatusUpdated,
            1,
            timestamp ?? defaultTimestamp,
            signature ?? defaultSignature,
            defaultSubscriptionId,
            messageId ?? defaultMessageId
        );
    }
}