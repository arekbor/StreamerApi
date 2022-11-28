using SteamWebAPI2.Utilities;
using StreamerApi.Entities;
using StreamerApi.Services;

namespace StreamerApi.AdditionalServices
{
    public static class HangfireStreamer
    {
        public static void CreateSteamStatsJob(string steam, string ytUrl, string vidTitle, DateTime dateTime) {
            if (String.IsNullOrEmpty(steam)) {
                throw new ArgumentNullException(nameof(CreateSteamStatsJob));
            }
            var configuration = ConfigurationHelper.config;
            var webInterfaceFactory = new SteamWebInterfaceFactory(configuration["SteamApi"]);
            var steamInterface = webInterfaceFactory.CreateSteamWebInterface<SteamWebAPI2.Interfaces.SteamUser>(new HttpClient());

            if (!ulong.TryParse(steam, out ulong steamulong)) {
                throw new Exception(nameof(ulong.TryParse));
            }

            var playerSummaryResponse = steamInterface.GetPlayerSummaryAsync(steamulong)
                .GetAwaiter()
                .GetResult();

            var playerSummaryData = playerSummaryResponse.Data;

            
            using (var context = new StreamerDbContext(configuration)) {

                var exists = context
                    .steamStats
                    .FirstOrDefault(x => x.SteamId.Equals(steam));

                if (exists is null)
                {
                    SteamStats steamStats = new SteamStats
                    {
                        Username = playerSummaryData.Nickname,
                        AvatarUrl = playerSummaryData.AvatarUrl,
                        ProfileUrl = playerSummaryData.ProfileUrl,
                        YoutubeUrl = ytUrl,
                        YouTubeName = vidTitle,
                        DateTime = dateTime,
                        SteamId = steam
                    };
                    context.steamStats.Add(steamStats);
                }
                else {
                    exists.YoutubeUrl = ytUrl;
                    exists.YouTubeName = vidTitle;
                    exists.DateTime = dateTime;
                    context.steamStats.Update(exists);
                }
                context.SaveChanges();
            }
        }
    }
}
