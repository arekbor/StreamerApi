using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;
using StreamerApi.Models;
using StreamerApi.Services;

namespace StreamerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamerController: ControllerBase
    {
        private readonly IStreamerService _streamerService;
        private readonly ILogService _logService;
        private readonly IConfiguration _configuration;
        public StreamerController(IStreamerService streamerService,ILogService logService, IConfiguration configuration)
        {
            _streamerService = streamerService;
            _logService = logService;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Create(string token = "", int rank =12,string steam = "",string url = "")
        {
            _streamerService.Create(token,rank,steam,url);
            _logService.Log($"created file: {token}, {rank},{steam},{url}", LogLevel.Information);
            return NoContent();
        }
        [HttpGet("{token}")]
        public async Task<IActionResult> Get(string token)
        {
            var streamer = await _streamerService.Get(token);
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
        public async Task<IActionResult> GetStats([FromQuery] PaginationFilter filter) {
           var response = await _streamerService.PaginateStats(filter);
           return Ok(response);
        }

        [HttpGet("downloadLogs")]
        public IActionResult DownloadLogs()
        {
            string zipFileName = $"{DateTime.UtcNow:mm_DD_yyyy_hh_MM}.zip";
            string zipFilePath = $"{_configuration["ZipFilesPath"]}/{zipFileName}";
            
            ZipFile.CreateFromDirectory(_configuration["LogFilesPath"]!, zipFilePath);
            
            byte[] fileBytes = System.IO.File.ReadAllBytes(zipFilePath);
            return File(fileBytes, "application/zip", zipFileName);
        }
    }
}