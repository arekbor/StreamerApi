using Microsoft.AspNetCore.Authorization;
using StreamerApi.Data;
using StreamerApi.Exceptions;
using StreamerApi.Services;

namespace StreamerApi.Auhtorization
{
    
    public class IPVerifyRequirement: IAuthorizationRequirement
    {

    }
    public class IPVerifyRequirementHandler : AuthorizationHandler<IPVerifyRequirement>
    {
        private IHttpContextAccessor _httpContextAccessor;
        private ILogService _logService;
        public IPVerifyRequirementHandler(
            IHttpContextAccessor httpContextAccessor, 
            ILogService logService)
        {
            _httpContextAccessor = httpContextAccessor;
            _logService = logService;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IPVerifyRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                _logService.Log($"{nameof(httpContext)} is null in HandleRequirementAsync function", LogLevel.Critical);
                return Task.CompletedTask;
            }
            
            var address = httpContext.Connection.RemoteIpAddress;
            if (!AllowedAddresses.allowedIp.Contains(address?.ToString()))
                throw new ForbiddenException("Forbidden");
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
