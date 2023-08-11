using System.IO.Compression;
using AutoMapper;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using SteamWebAPI2.Utilities;
using StreamerApi.AdditionalServices;
using StreamerApi.Entities;
using StreamerApi.Exceptions;
using StreamerApi.Models;

namespace StreamerApi.Services
{
    public class StreamerService : IStreamerService
    {
        private readonly StreamerDbContext _streamerDbContext;
        private readonly IFunctions _functions;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        public StreamerService(StreamerDbContext streamerDbContext, IMapper mapper, IFunctions functions, IConfiguration configuration, ILogService logService)
        {
            _streamerDbContext = streamerDbContext;
            _functions = functions;
            _configuration = configuration;
            _logService = logService;
            _mapper = mapper;
        }
        public async Task <Pager<List<SteamStatsDto>>> PaginateStats(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var data = await _streamerDbContext
                .steamStats
                .OrderByDescending(x => x.DateTime)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();


            var totalRecords = await _streamerDbContext.steamStats.CountAsync();

            var mapped = _mapper.Map<List<SteamStatsDto>>(data);

            var pagedReponse = PaginationHelper.CreatePagedReponse<SteamStatsDto>
                (mapped, validFilter, totalRecords);

            return pagedReponse;
        }

        public void Create(string token, int rank, string steam, string url)
        {
            ulong steamId;
            if (!_functions.IsDirectoryExists(_configuration["SaveFilesPath"]))
                throw new ClientException("Brak docelowej ścieżki");

            if (!_functions.IsRankCorrect(rank) || rank == 0 || rank == -1)
                throw new ClientException("Nieprawidłowa ranga");

            if (!_functions.IsNumeric(token))
                throw new ClientException("Nieprawidłowy token");

            if (_functions.IsTokenTaken(token)) 
                throw new ClientException("Token jest już zarezerwowany");

            if (!_functions.IsNumeric(steam))
                throw new ClientException("Niepoprawne steam id");

            if (_functions.IsYoutube(url))
                throw new ClientException("Niepoprawny adres URL ścieżki");

            var video = _functions.ConvertVideo(url);

            if (_functions.IsVideoExists(video.Title) != null)
                return;
                
            if (video == null)
                throw new ClientException("Błąd podczas konwersji wideo");

            if (_functions.SecondsInRange(rank, (int)(video.Info.LengthSeconds / 60)))
                throw new ClientException($"Przekroczony limit długości.");

            if (ulong.TryParse(steam, out steamId))
            {
                if (!_functions.IsUserIsBlacklisted(steamId))
                {
                    if (_functions.DownloadVideo(token, video))
                    {
                        double fileSize = (double)video.ContentLength;
                        double resultSize = (fileSize / (1024 * 1024));
                        string videoLenght = Math.Round((decimal)video.Info.LengthSeconds / 60, 2).ToString();
                        DateTime dateTime = DateTime.Now;

                        var streamer = new Streamer()
                        {
                            FileName = video.Title,
                            FileLenght = videoLenght,
                            FileSize = Math.Round(resultSize, 2).ToString(),
                            CreatedTime = dateTime,
                            Url = url,
                            Token = token,
                            Steam = steam,
                            Rank = rank
                        };
                        _streamerDbContext.Add(streamer);
                        _streamerDbContext.SaveChanges();
                        
                        _logService.Log("Video downloaded successfully in Create function in StreamerService", LogLevel.Information);
                        BackgroundJob.Enqueue(() => HangfireStreamer.CreateSteamStatsJob
                        (steam, url, video.Title, dateTime));
                    }
                    else
                    {
                        throw new ClientException("Błąd podczas zapisu pliku wideo");
                    }
                }
                else
                {
                    throw new ClientException("Jesteś na czarnej liście");
                }
            }
            else
            {
                throw new ClientException("Błąd podczas parsowania steamid");
            }
        }
        public async Task<FileStream> Get(string token)
        {
            var streamer = await _streamerDbContext
                .streamerDbContext
                .FirstOrDefaultAsync(x => x.Token == token);

            if (streamer == null)
                throw new ClientException("Nie znaleziono pliku");

            var fileStream = new FileStream($"{_configuration["SaveFilesPath"]}{token}.mp4", FileMode.Open, FileAccess.Read, FileShare.Read, 1024);

            return fileStream;
        }
        public List<RankDto> GetRankData()
        {
            var data = _streamerDbContext
                .rankDbContext
                .Skip(2)
                .OrderBy(x => x.MaxSeconds)
                .ToList();
            var dataDto = _mapper.Map<List<RankDto>>(data);
            var fullNames = _configuration.GetSection("RankNames").Get<string[]>();
            int i = 0;
            foreach (var item in dataDto)
            {
                item.FullName = fullNames[i++];
            }
            return dataDto;
        }
        public int RemoveFiles()
        {
            var data = _streamerDbContext
                .streamerDbContext.ToList();
            DirectoryInfo dir = new DirectoryInfo(_configuration["SaveFilesPath"]);
            var count = dir.GetFiles().Count();
            foreach (var item in dir.GetFiles())
            {
                item.Delete();
            }
            _streamerDbContext.RemoveRange(data);
            _streamerDbContext.SaveChanges();
            
            return count;
        }
        public async Task<IEnumerable<BlacklistDto>> GetBlacklist()
        {
            var webInterfaceFactory = new SteamWebInterfaceFactory(_configuration["SteamApi"]);
            var steamInterface = webInterfaceFactory.CreateSteamWebInterface<SteamWebAPI2.Interfaces.SteamUser>(new HttpClient());
            var blacklist = await _streamerDbContext
                .blacklists.ToListAsync();

            if (blacklist.Count() == 0)
                return null;

            var paginatedSteamsId = blacklist.Select(x => x.SteamId).ToList();
            var playerSummariesResponse = await steamInterface.GetPlayerSummariesAsync(paginatedSteamsId);
            var playerSummariesData = playerSummariesResponse.Data;

            var data = new List<BlacklistDto>();
            foreach (var item in blacklist)
            {
                var steam = playerSummariesData.FirstOrDefault(x => x.SteamId == item.SteamId);
                if (steam is null)
                    throw new ClientException("Fail playerSummariesData");
                var st = new BlacklistDto()
                {
                    Nickname = steam.Nickname,
                    AvatarUrl = steam.AvatarUrl,
                    ProfileUrl = steam.ProfileUrl
                };
                data.Add(st);
            }
            return data;
        }
    }
}
