namespace StreamerApi.Entities
{
    public class RateLimit
    {
        public DateTime DateTime { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string ClientIp { get; set; }
    }
}
