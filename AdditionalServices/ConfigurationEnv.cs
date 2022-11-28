namespace StreamerApi.AdditionalServices
{
    public class ConfigurationEnv
    {
        public string ConnectionDB { get; set; }
        public string ConnectionDBHangfire { get; set; }
        public string PathFiles { get; set; }
        public string PathLogs { get; set; }
        public string SteamKey { get; set; }
        public string PaginationLimit { get; set; }

    }
}
