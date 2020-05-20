using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http;
using System.Security.Claims;

namespace TaskManager.API.Helpers.Filters
{
    public class UserAuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var routeData = context.RouteData.Values["userId"].ToString();
            int userId = int.Parse(routeData);

            if (userId != int.Parse(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
