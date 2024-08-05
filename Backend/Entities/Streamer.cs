using System.ComponentModel.DataAnnotations;

namespace StreamerApi.Entities
{
    public class Streamer
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileLenght { get; set; }
        public string FileSize { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedTime { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
        public string Steam { get; set; }
        public int Rank { get; set; }
    }
}
