using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Videos
{
    public class ChannelVideoResponse
    {
        public Guid Id { get; set; }
        
        public int Duration { get; set; }
        
        [JsonProperty(PropertyName = "is_live")]
        public bool IsLive { get; set; }
        
        [JsonProperty(PropertyName = "is_mature")]
        public bool IsMature { get; set; }
        
        public string Language { get; set; }

        [JsonProperty(PropertyName = "start_time")]
        public DateTime StartTime { get; set; }
        
        [JsonProperty(PropertyName = "end_time")]
        public DateTime? EndTime { get; set; }

        [JsonProperty(PropertyName = "viewer_count")]
        public long Views { get; set; }
        
        public string Title { get; set; }
        
        public string Tier { get; set; }
        
        public string Status { get; set; }
        
        public string[] Tags { get; set; }

        public VideoThumbnailResponse Thumbnail { get; set; }
        
        public VideoCategoryWebResponse Category { get; set; }
        
        public VideoChannelWebResponse Channel { get; set; }
    }
}