using AutoMapper;
using StreamerApi.Entities;

namespace StreamerApi.Models;

public class StreamerProfile:Profile
{
    public StreamerProfile()
    {
        CreateMap<Rank, RankDto>();

        CreateMap<RateLimitDto, RateLimit>();

        CreateMap<SteamStats, SteamStatsDto>()
            .ReverseMap();
    }
}

