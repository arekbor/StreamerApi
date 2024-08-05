namespace StreamerApi.Entities
{
    public class StreamerSeeder
    {
        private readonly StreamerDbContext _streamerDbContext;
        private readonly IConfiguration _configuration;
        public StreamerSeeder(StreamerDbContext streamerDbContext, IConfiguration configuration)
        {
            _streamerDbContext = streamerDbContext;
            _configuration = configuration;
        }
        public void Seed()
        {
            if (_streamerDbContext.Database.CanConnect())
            {
                if (!_streamerDbContext.rankDbContext.Any())
                {
                    var ranks = new List<Rank>()
                    {
                        new Rank()
                        {
                            Level = -1,
                            Name = "default",
                            MaxSeconds = 5
                        },
                        new Rank()
                        {
                            Level = 0,
                            Name = "null",
                            MaxSeconds = 0
                        },
                        new Rank()
                        {
                            Level = 1,
                            Name = "user",
                            MaxSeconds = int.Parse(_configuration["RankSettings:User"])
                        },
                        new Rank()
                        {
                            Level = 2,
                            Name = "player",
                            MaxSeconds = int.Parse(_configuration["RankSettings:Player"])
                        },
                        new Rank()
                        {
                            Level = 3,
                            Name = "vip",
                            MaxSeconds = int.Parse(_configuration["RankSettings:Vip"])
                        },
                        new Rank()
                        {
                            Level = 4,
                            Name = "gefrajter",
                            MaxSeconds = int.Parse(_configuration["RankSettings:Gefrajter"])
                        },
                        new Rank()
                        {
                            Level = 5,
                            Name = "rekrut",
                            MaxSeconds = int.Parse(_configuration["RankSettings:Rekrut"])
                        },
                        new Rank()
                        {
                            Level = 6,
                            Name = "operator",
                            MaxSeconds = int.Parse(_configuration["RankSettings:Operator"])
                        },
                        new Rank()
                        {
                            Level = 7,
                            Name = "moderator",
                            MaxSeconds = int.Parse(_configuration["RankSettings:Moderator"])
                        },
                        new Rank()
                        {
                            Level = 8,
                            Name = "viceadmin",
                            MaxSeconds = int.Parse(_configuration["RankSettings:Viceadmin"])
                        },
                        new Rank()
                        {
                            Level = 9,
                            Name = "admin",
                            MaxSeconds = int.Parse(_configuration["RankSettings:Admin"])
                        },
                        new Rank()
                        {
                            Level = 10,
                            Name = "Superadmin",
                            MaxSeconds = int.Parse(_configuration["RankSettings:Superadmin"])
                        },
                        new Rank()
                        {
                            Level = 11,
                            Name = "zarzad",
                            MaxSeconds = int.Parse(_configuration["RankSettings:Zarzad"])
                        },
                        new Rank()
                        {
                            Level = 12,
                            Name = "superadmin",
                            MaxSeconds = int.Parse(_configuration["RankSettings:Owner"])
                        }
                    };
                    _streamerDbContext.AddRange(ranks);
                    _streamerDbContext.SaveChanges();
                }
            }
        }
    }
}
