using StreamerApi.Models;

namespace StreamerApi.Services
{
    public interface IStreamerService
    {
        public void Create(string token, int rank, string steam, string url);
        public FileStream Get(string token);
        public int RemoveFiles();
        public List<RankDto> GetRankData();
        Task<IEnumerable<BlacklistDto>> GetBlacklist();
        Task <IEnumerable<SteamStatsDto>> PaginateStats(int page, int limit);
    }
}
