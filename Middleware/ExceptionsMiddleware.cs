using StreamerApi.Exceptions;
using StreamerApi.Services;

namespace StreamerApi.Middleware
{
    public class ExceptionsMiddleware : IMiddleware
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogService _logService;
        public ExceptionsMiddleware(
            IWebHostEnvironment webHostEnvironment, 
            ILogService logService)
        {
            _webHostEnvironment = webHostEnvironment;
            _logService = logService;
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
                _logService.Log(e.Message, LogLevel.Information);
                await context.Response.WriteAsync(e.Message);
            }
            catch(ForbiddenException e)
            {
                context.Response.StatusCode = 403;
                _logService.Log(e.Message, LogLevel.Information);
                await context.Response.WriteAsync(e.Message);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                _logService.Log(e.Message, LogLevel.Warning);
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
