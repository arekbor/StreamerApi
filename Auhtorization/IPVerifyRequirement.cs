using Microsoft.AspNetCore.Authorization;
using StreamerApi.Data;
using StreamerApi.Exceptions;

namespace StreamerApi.Auhtorization
{
    
    public class IPVerifyRequirement: IAuthorizationRequirement
    {

    }
    public class IPVerifyRequirementHandler : AuthorizationHandler<IPVerifyRequirement>
    {
        private IHttpContextAccessor _httpContextAccessor;
        public IPVerifyRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IPVerifyRequirement requirement)
        {
            var address = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            if (!AllowedAddresses.allowedIp.Contains(address.ToString()))
                throw new ForbiddenException("Forbidden");
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
