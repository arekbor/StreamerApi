using StreamerApi.Exceptions;

namespace StreamerApi.Middleware
{
    public class ExceptionsMiddleware : IMiddleware
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ExceptionsMiddleware(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (ClientException e)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(e.Message);
            }
            catch(ForbiddenException e)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(e.Message);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                if (!_webHostEnvironment.IsDevelopment())
                {
                    await context.Response.WriteAsync($"Internal server error");
                }
                else
                {
                    await context.Response.WriteAsJsonAsync("Internal server error"+ e);
                }
            }
        }
    }
}
