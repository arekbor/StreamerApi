namespace StreamerApi.Models
{
    public class Response<T>
    {
        public Response() { }
        public Response(T data) {
            
        }
        public T Data { get; set; }
    }
}
