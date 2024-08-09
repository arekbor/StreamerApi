namespace StreamerApi.Models
{
    public class Response<T>
    {
        public Response() { }
        public Response(T items) {
            
        }
        public T Items { get; set; }
    }
}
