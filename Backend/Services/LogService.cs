namespace StreamerApi.Services
{
    public class LogService : ILogService
    {
        private readonly IConfiguration _configuration;
        public LogService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Log(string msg, LogLevel logLevel)
        {
            var fileName = $"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day}.txt";

            File.AppendAllText($"{_configuration["LogFilesPath"]}{fileName}", $"{DateTime.Now.ToString("G")}: {logLevel} - {msg}"+Environment.NewLine);
        }
    }
}
