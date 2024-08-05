namespace StreamerApi.Models
{
    public class RateLimitDto
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string ClientIp { get; set; }
    }
}
