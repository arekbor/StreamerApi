using VideoLibrary;

namespace StreamerApi.AdditionalServices
{
    public interface IFunctions
    {
        public bool IsYoutube(string url);
        public bool IsNumeric(string token);
        public bool IsTokenTaken(string token);
        public YouTubeVideo ConvertVideo(string url);
        public bool IsRankCorrect(int rank);
        public bool SecondsInRange(int rank, int seconds);
        public bool DownloadVideo(string token, YouTubeVideo video);
        public bool IsDirectoryExists(string path);
        public string IsVideoExists(string fileName);
        public bool IsUserIsBlacklisted(ulong steamid);
    }
}
