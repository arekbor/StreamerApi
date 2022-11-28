using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StreamerApi.Services;

namespace StreamerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamerController: ControllerBase
    {
        private readonly IStreamerService _streamerService;
        private readonly ILogService _logService;
        public StreamerController(IStreamerService streamerService,ILogService logService)
        {
            _streamerService = streamerService;
            _logService = logService;
        }
        [HttpGet]
        public IActionResult Create(string token, int rank,string steam,string url)
        {
            _streamerService.Create(token,rank,steam,url);
            _logService.Log($"created file: {token}, {rank},{steam},{url}", LogLevel.Information);
            return NoContent();
        }
        [HttpGet("{token}")]
        public IActionResult Get(string token)
        {
            var streamer = _streamerService.Get(token);
            _logService.Log($"returned file {streamer.Name}", LogLevel.Information);
            Response.Headers.Remove("Cache-Control");
            Response.Headers.Add("Accept-Ranges", "bytes");
            return File(streamer, "audio/mp3");
        }
        [HttpGet("removedata")]
        public IActionResult RemoveData()
        {
            var count = _streamerService.RemoveFiles();
            _logService.Log($"removed {count} files", LogLevel.Information);
            return Ok($"Removed {count} files");
        }
        [HttpGet("rankdata")]
        public IActionResult GetRanks()
        {
            var data = _streamerService.GetRankData();
            _logService.Log($"returned ranks table", LogLevel.Information);
            return Ok(data);
        }
        [HttpGet("blacklist")]
        public async Task<IActionResult> GetBlacklistData()
        {
            var result = await _streamerService.GetBlacklist();
            return Ok(result);
        }
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats(int page, int limit) {
            var result = await _streamerService.PaginateStats(page, limit);
            return Ok(result);
        }
    }
}