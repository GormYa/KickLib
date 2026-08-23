using FluentAssertions;
using KickLib.Models.v1.ChannelRewards;

namespace KickLib.Tests.Models.ChannelRewards;

public class CreateChannelRewardRequestTests
{
    [Fact]
    public void Constructor_WithValidValues_SetsProperties()
    {
        var request = new CreateChannelRewardRequest(100, "Song Request");

        request.Cost.Should().Be(100);
        request.Title.Should().Be("Song Request");
        request.IsEnabled.Should().BeTrue();
        request.IsUserInputRequired.Should().BeFalse();
        request.ShouldRedemptionsSkipRequestQueue.Should().BeFalse();
        request.BackgroundColor.Should().BeNull();
        request.Description.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidTitle_ThrowsArgumentNullException(string? title)
    {
        var act = () => new CreateChannelRewardRequest(100, title!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_WithTitleLongerThan50Characters_ThrowsArgumentOutOfRangeException()
    {
        var act = () => new CreateChannelRewardRequest(100, new string('a', 51));

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WithTitleAt50Characters_DoesNotThrow()
    {
        var act = () => new CreateChannelRewardRequest(100, new string('a', 50));

        act.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithCostLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        var act = () => new CreateChannelRewardRequest(0, "Title");

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData("#00e701")]
    [InlineData("#FFFFFF")]
    public void BackgroundColor_WithValidHexColor_DoesNotThrow(string color)
    {
        var request = new CreateChannelRewardRequest(100, "Title");

        var act = () => request.BackgroundColor = color;

        act.Should().NotThrow();
        request.BackgroundColor.Should().Be(color);
    }

    [Fact]
    public void BackgroundColor_WithNull_DoesNotThrow()
    {
        var request = new CreateChannelRewardRequest(100, "Title") { BackgroundColor = "#00e701" };

        var act = () => request.BackgroundColor = null;

        act.Should().NotThrow();
        request.BackgroundColor.Should().BeNull();
    }

    [Theory]
    [InlineData("red")]
    [InlineData("#zzzzzz")]
    [InlineData("#12345")]
    [InlineData("00e701")]
    public void BackgroundColor_WithInvalidFormat_ThrowsArgumentOutOfRangeException(string color)
    {
        var request = new CreateChannelRewardRequest(100, "Title");

        var act = () => request.BackgroundColor = color;

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Description_WithMoreThan200Characters_ThrowsArgumentOutOfRangeException()
    {
        var request = new CreateChannelRewardRequest(100, "Title");

        var act = () => request.Description = new string('a', 201);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Description_WithExactly200Characters_DoesNotThrow()
    {
        var request = new CreateChannelRewardRequest(100, "Title");

        var act = () => request.Description = new string('a', 200);

        act.Should().NotThrow();
    }

    [Fact]
    public void Description_WithNull_DoesNotThrow()
    {
        var request = new CreateChannelRewardRequest(100, "Title") { Description = "Some description" };

        var act = () => request.Description = null;

        act.Should().NotThrow();
        request.Description.Should().BeNull();
    }
}
