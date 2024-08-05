using System.ComponentModel.DataAnnotations;

namespace StreamerApi.Models
{
    public class SteamStatsDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string ProfileUrl { get; set; }
        public string YoutubeUrl { get; set; }
        public string YouTubeName { get; set; }
        public DateTime DateTime { get; set; }
    }
}
