
namespace StreamerApi.Services
{
    public interface ILogService
    {
        void Log(string msg, LogLevel logLevel);
    }
}