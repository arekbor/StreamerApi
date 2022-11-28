using Hangfire;
using StreamerApi.Entities;
using StreamerApi.Exceptions;
using System.Text.RegularExpressions;
using VideoLibrary;

namespace StreamerApi.AdditionalServices
{
    public class Functions:IFunctions
    {
        private readonly StreamerDbContext _streamerDbContext;
        private readonly IConfiguration _configuration;
        public Functions(StreamerDbContext streamerDbContext, IConfiguration configuration)
        {
            _streamerDbContext = streamerDbContext;
            _configuration = configuration;
        }
        public bool IsYoutube(string url)
        {
            var youtubeMatch = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)")
            .Match(url);
            if (!youtubeMatch.Success)
            {
                return true;
            }
            return false;
        }
        public bool IsNumeric(string token)
        {
            var IsNumeric = Regex
                .IsMatch(token, @"^\d+$");
            if (IsNumeric)
            {
                return true;
            }
            return false;
        }
        public bool IsTokenTaken(string token)
        {
            var anyToken = _streamerDbContext
                .streamerDbContext
                .Any(x => x.Token == token);
            if (anyToken)
            {
                return true;
            }
            return false;
        }
        public YouTubeVideo ConvertVideo(string url)
        {
            try
            {
                var youTube = YouTube.Default;
                var video = youTube.GetVideoAsync(url).GetAwaiter().GetResult();
                return video;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Video has unavailable stream"))
                    throw new ClientException("Film ma niedostępny strumień");
                if (e.Message.Contains("Error caused by Youtube"))
                    throw new ClientException("Błąd z powodu ograniczonego dostępu");
                if (e.Message.Contains("This is live stream so unavailable stream"))
                    throw new ClientException("Nie można przekonwertować transmisji na żywo");
                if (e.Message.Contains("Player json has no found"))
                    throw new ClientException("Nie znaleziono odtwarzacza");
                return null;
            }
        }
        public bool IsRankCorrect(int rank)
        {
            var anyRank = _streamerDbContext
                .rankDbContext
                .Any(x => x.Level == rank);
            if (anyRank)
            {
                return true;
            }
            return false;
        }
        public bool SecondsInRange(int rank, int seconds)
        {
            var level = _streamerDbContext
                .rankDbContext
                .FirstOrDefault(x => x.Level == rank);

            if (seconds >= level.MaxSeconds)
            {
                return true;
            }
            return false;
        }
        public bool DownloadVideo(string token, YouTubeVideo video)
        {
            try
            {
                File.WriteAllBytesAsync(_configuration["SaveFilesPath"] + token + ".mp4", video.GetBytesAsync().GetAwaiter().GetResult());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IsDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                return false;
            }
            return true;
        }
        public string IsVideoExists(string fileName)
        {
            var video = _streamerDbContext.streamerDbContext
                .FirstOrDefault(x => x.FileName == fileName);
            if (video != null)
            {
                return video.Token;
            }
            return null;
        }
        public bool IsUserIsBlacklisted(ulong steamid)
        {
            var check = _streamerDbContext.blacklists
                .Any(x => x.SteamId == steamid);
            if (check)
                return true;
            else
                return false;
        }
    }
}

