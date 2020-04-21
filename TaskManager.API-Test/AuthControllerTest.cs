using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.API.Controllers;
using TaskManager.API.Dto;
using TaskManager.API.Dto.User;
using TaskManager.API.Helpers;
using TaskManager.API.Helpers.GenerateToken;
using TaskManager.API.Model;
using Xunit;

namespace TaskManager.API_Test
{
    public class AuthControllerTest
    {

        private readonly Mock<IConfiguration> configMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<ITokenGenerator> tokenGeneratorMock;

        public AuthControllerTest()
        {
            configMock = new Mock<IConfiguration>();
            mapperMock = new Mock<IMapper>();
            tokenGeneratorMock = new Mock<ITokenGenerator>();
        }

        [Fact]
        public async Task LoginFindUserUnauthorizedStatus()
        {
            //Arrange
            var userForLogin = new UserForLogin { Username = "test" };
            var userManager = MockIdentity.GetMockUserManager();

            AuthController controller = new AuthController(userManager.Object,
                MockIdentity.GetMockSignInManager().Object, configMock.Object, mapperMock.Object, tokenGeneratorMock.Object);

            userManager.Setup(um => um.FindByNameAsync("test"))
                .Returns(Task.FromResult((User)null));

            //Act
            var action = await controller.Login(userForLogin) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task LoginOkStatus()
        {
            //Arrange
            var userForLogin = new UserForLogin { Username = "test", Password = "test"};
            var user = new User { UserName = "test", Nickname = "test", Id = 1 };
            var userManager = MockIdentity.GetMockUserManager();
            var signInManager = MockIdentity.GetMockSignInManager();

            AuthController controller = new AuthController(userManager.Object,
                signInManager.Object, configMock.Object, mapperMock.Object, tokenGeneratorMock.Object);

            userManager.Setup(um => um.FindByNameAsync(user.UserName))
                 .Returns(Task.FromResult(user));

            signInManager.Setup(sm => sm.CheckPasswordSignInAsync(user, userForLogin.Password, false))
                .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));

            mapperMock.Setup(m => m.Map<UserForReturnNickname>(user))
                .Returns(new UserForReturnNickname {Nickname = "test"});

            tokenGeneratorMock.Setup(t => t.GenerateJwtToken(user, configMock.Object)).Returns("token");

            //Act
            var action = await controller.Login(userForLogin) as OkObjectResult;

            //Assert
            Assert.Equal(200, action.StatusCode);
        }

    }
}
