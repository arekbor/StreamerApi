using Microsoft.EntityFrameworkCore;

namespace StreamerApi.Entities
{
    public class StreamerDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<Streamer> streamerDbContext { get; set; }
        public DbSet<Rank> rankDbContext { get; set; }
        public DbSet<Blacklist> blacklists { get; set; }
        public DbSet<SteamStats> steamStats { get; set; }
        public StreamerDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration["ConnectionStrings:Default"], o => o.SetPostgresVersion(9, 6));
            }

        }
    }
}
