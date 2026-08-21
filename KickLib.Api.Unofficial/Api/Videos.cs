using KickLib.Api.Unofficial.Core;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models.Response;
using KickLib.Api.Unofficial.Models.Response.v1.Videos;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial.Api
{
    public class Videos : BaseApi
    {
        private readonly IApiCaller _client;
        private const string ApiUrlPart = "video/";

        public Videos(IApiCaller client, ILogger logger = null)
            : base(client, logger)
        {
            _client = client;
        }
    
        /// <summary>
        ///     Gets specific video details.
        /// </summary>
        /// <param name="videoUid">Video unique identifier (UUID).</param>
        [Obsolete("This method may not work anymore, use method using ChannelID and Video UUID")]
        public Task<VideoResponse> GetVideoAsync(Guid videoUid)
        {
            if (videoUid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(videoUid));
            }

            var urlPart = $"{ApiUrlPart}{videoUid}";
            return GetAsync<VideoResponse>(urlPart, ApiVersion.V1);
        }
        
        /// <summary>
        ///     Gets specific video details.
        /// </summary>
        /// <param name="channelId">Channel ID under which video exists.</param>
        public async Task<ICollection<ChannelVideoResponse>> GetChannelVideosAsync(int channelId)
        {
            if (channelId < 0)
            {
                throw new ArgumentException($"Channel ID must be positive value, but was {channelId}.");
            }

            var urlPart = $"channels/{channelId}/videos";
            var result = await GetAsync<DataWrapper<ICollection<ChannelVideoResponse>>>(urlPart, ApiVersion.WebV1);
            return result?.Data;
        }
        
        /// <summary>
        ///     Gets specific video details.
        /// </summary>
        /// <param name="channelId">Channel ID under which video exists.</param>
        /// <param name="videoId">Video unique identifier (UUID).</param>
        public async Task<ChannelVideoResponse> GetVideoAsync(int channelId, Guid videoId)
        {
            if (videoId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(videoId));
            }
            
            if (channelId < 0)
            {
                throw new ArgumentException($"Channel ID must be positive value, but was {channelId}.");
            }

            var urlPart = $"channels/{channelId}/videos/{videoId}";
            var result = await GetAsync<DataWrapper<ChannelVideoResponse>>(urlPart, ApiVersion.WebV1);
            return result?.Data;
        }
        
        /// <summary>
        ///     Gets specific video details.
        /// </summary>
        /// <param name="channelId">Channel ID under which video exists.</param>
        /// <param name="videoId">Video unique identifier (UUID).</param>
        public async Task<VideoPlayback> GetVideoPlaybackAsync(int channelId, Guid videoId)
        {
            if (videoId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(videoId));
            }
            
            if (channelId < 0)
            {
                throw new ArgumentException($"Channel ID must be positive value, but was {channelId}.");
            }

            await _client.RefreshTokensAsync(true);
            
            var session = await GetAsync<DataWrapper<KickUserSession>>("user/session", ApiVersion.WebV1);
            if (session?.Data is null)
            {
                return null;
            }
            
            var urlPart = $"stream/{videoId}/playback";

            var payload = new
            {
                video_player = new
                {
                    player = new
                    {
                        player_name = "web_mobile"
                    }
                },
                video_session = new
                {
                    page_type = "video"
                },
                user_session = new
                {
                    session_id = session.Data.SessionId,
                    player_device_id = Guid.NewGuid(),
                    browser_lang = "en"
                }
            };
            
            var result = await PostAsync<VideoPlayback>(urlPart, ApiVersion.WebV1, payload);
            if (result?.PlaybackUrl is null)
            {
                return result;
            }
            
            result.Source = await GetAsync<VideoSource>(result.PlaybackUrl.VodSession, ApiVersion.Plain);
            
            return result;
        }
    }
}