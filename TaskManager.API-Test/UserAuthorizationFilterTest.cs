using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using TaskManager.API.Helpers.Filters;
using TaskManager.API.Model;
using Xunit;

namespace TaskManager.API_Test
{
    public class UserAuthorizationFilterTest
    {
        private ClaimsPrincipal UserClaims()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.NameIdentifier, "1"),
                 new Claim(ClaimTypes.Name, "test")
            }, "TestAuthentication"));

            return user;
        }

        [Fact]
        public void UserAuthorizationUnauthorized()
        {
            //Arrange
            var httpContext = new DefaultHttpContext { User = UserClaims() };
            var routeData = new RouteData();
            routeData.Values.Add("userId", "2");
            var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());
            var authorizationFilterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
            var userAuthorizationFilter = new UserAuthorizationFilter();

            //Act
            userAuthorizationFilter.OnAuthorization(authorizationFilterContext);

            //Assert
            Assert.NotNull(authorizationFilterContext.Result);
            var result = Assert.IsType<UnauthorizedResult>(authorizationFilterContext.Result);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public void UserAuthorizationAuthorized()
        {
            //Arrange
            var httpContext = new DefaultHttpContext { User = UserClaims() };
            var routeData = new RouteData();
            routeData.Values.Add("userId", "1");
            var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());
            var authorizationFilterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
            var userAuthorizationFilter = new UserAuthorizationFilter();

            //Act
            userAuthorizationFilter.OnAuthorization(authorizationFilterContext);

            //Assert
            var result = authorizationFilterContext.Result;
            Assert.Null(result);
        }
    }
}
