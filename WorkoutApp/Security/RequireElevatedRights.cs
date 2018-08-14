using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Services;

namespace WorkoutApp.Security
{
    public class RequireElevatedRights : AuthorizationHandler<RequireElevatedRights>, IAuthorizationRequirement
    {
        private string _adminObjId;
        private IHttpContextAccessor _httpContextAccessor;

        public RequireElevatedRights(string adminObjid, IHttpContextAccessor httpContextAccessor)
        {
            _adminObjId = adminObjid;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequireElevatedRights requirement)
        {
            string currentAzureADObjId = "";

            if (_httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Any())
                currentAzureADObjId = _httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").FirstOrDefault().Value;

            if (_adminObjId == currentAzureADObjId)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
