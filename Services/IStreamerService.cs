using StreamerApi.Models;

namespace StreamerApi.Services
{
    public interface IStreamerService
    {
        public void Create(string token, int rank, string steam, string url);
        public Task<FileStream> Get(string token);
        public int RemoveFiles();
        public List<RankDto> GetRankData();
        Task<IEnumerable<BlacklistDto>> GetBlacklist();
        Task<Pager<List<SteamStatsDto>>>  PaginateStats(PaginationFilter filter);
    }
}
