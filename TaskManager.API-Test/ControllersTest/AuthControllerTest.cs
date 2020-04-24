using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.API.Controllers;
using TaskManager.API.Data.Repository;
using TaskManager.API.Data.Repository.UserRepo;
using TaskManager.API.Dto;
using TaskManager.API.Dto.User;
using TaskManager.API.Helpers;
using TaskManager.API.Helpers.GenerateToken;
using TaskManager.API.Model;
using Xunit;

namespace TaskManager.API_Test.ControllersTest
{
    public class AuthControllerTest
    {

        private readonly Mock<IConfiguration> configMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IRepositoryWrapper> wrapperMock;

        public AuthControllerTest()
        {
            configMock = new Mock<IConfiguration>();
            mapperMock = new Mock<IMapper>();
            wrapperMock = new Mock<IRepositoryWrapper>();
        }

        [Fact]
        public async Task LoginFindUserUnauthorizedStatus()
        {
            //Arrange
            var userForLogin = new UserForLogin { Username = "test" };
            var userManager = MockIdentity.GetMockUserManager();

            AuthController controller = new AuthController(userManager.Object,
                MockIdentity.GetMockSignInManager().Object, configMock.Object, mapperMock.Object,
                wrapperMock.Object);

            userManager.Setup(um => um.FindByNameAsync("test"))
                .Returns(Task.FromResult((User)null));

            //Act
            var action = await controller.Login(userForLogin) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task LoginCheckPasswordUnauthorizedStatus()
        {
            //Arrange
            var userForLogin = new UserForLogin { Username = "test", Password = "test" };
            var userManager = MockIdentity.GetMockUserManager();
            var signInManager = MockIdentity.GetMockSignInManager();
            var user = new User { Id = 1, UserName = "test" };

            AuthController controller = new AuthController(userManager.Object,
                signInManager.Object, configMock.Object, mapperMock.Object,
                wrapperMock.Object);

            userManager.Setup(um => um.FindByNameAsync("test"))
                .Returns(Task.FromResult(user)).Verifiable();

            signInManager.Setup(sm => sm.CheckPasswordSignInAsync(user, userForLogin.Password, It.IsAny<bool>()))
                .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed)).Verifiable();

            //Act
            var action = await controller.Login(userForLogin) as UnauthorizedResult;

            //Assert
            userManager.Verify(um => um.FindByNameAsync("test"), Times.Once);
            signInManager.Verify(sm => sm.CheckPasswordSignInAsync(user, userForLogin.Password, It.IsAny<bool>()), Times.Once);
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task LoginOkStatus()
        {
            //Arrange
            var userForLogin = new UserForLogin { Username = "test", Password = "test" };
            var user = new User { UserName = "test", Nickname = "test", Id = 1 };
            var userManager = MockIdentity.GetMockUserManager();
            var signInManager = MockIdentity.GetMockSignInManager();
            var configurationSection = new Mock<IConfigurationSection>();

            AuthController controller = new AuthController(userManager.Object,
                signInManager.Object, configMock.Object, mapperMock.Object,
                wrapperMock.Object);

            Authorization.GetIdentity(controller);

            userManager.Setup(um => um.FindByNameAsync(user.UserName))
                 .Returns(Task.FromResult(user));

            signInManager.Setup(sm => sm.CheckPasswordSignInAsync(user, userForLogin.Password, false))
                .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));

            mapperMock.Setup(m => m.Map<UserForReturnNickname>(user))
                .Returns(new UserForReturnNickname { Nickname = "test" });

         
            configurationSection.Setup(a => a.Value).Returns("VeryLongKeyForTest");
            configMock.Setup(a => a.GetSection("AppSettings:Token")).Returns(configurationSection.Object);



            //Act
            var action = await controller.Login(userForLogin) as OkObjectResult;
            var item = action as Object;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.NotNull(item);
        }

        [Fact]
        public async Task RegisterIncorrectRepeatPasswordBadRequestStatus()
        {
            //Arrange
            var userForRegister = new UserForRegister { Username = "test", Password = "test", RepeatPassword = "test1" };

            AuthController controller = new AuthController(MockIdentity.GetMockUserManager().Object,
                MockIdentity.GetMockSignInManager().Object, configMock.Object, mapperMock.Object,
                wrapperMock.Object);

            //Act
            var action = await controller.Register(userForRegister) as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task RegisterOkStatus()
        {
            //Arrange
            var userForRegister = new UserForRegister { Username = "test", Password = "test", RepeatPassword = "test" };
            var userManager = MockIdentity.GetMockUserManager();
            var signInManager = MockIdentity.GetMockSignInManager();
            var user = new User { UserName = "test" };

            AuthController controller = new AuthController(userManager.Object,
                signInManager.Object, configMock.Object, mapperMock.Object,
                wrapperMock.Object);


            userManager.Setup(um => um.CreateAsync(user, "test"))
            .Returns(Task.FromResult(IdentityResult.Success));

            wrapperMock.Setup(u => u.UserRepository.GetLastUser()).Returns(Task.FromResult(user));

            mapperMock.Setup(m => m.Map<User>(userForRegister)).Returns(user);

            //Act
            var action = await controller.Register(userForRegister) as OkResult;

            //Assert
            Assert.Equal(200, action.StatusCode);   
       }

        [Fact]
        public async Task RegisterLoginAlreadyExistsBadRequestStatus()
        {
            //Arrange
            var userForRegister = new UserForRegister { Username = "test", Password = "test", RepeatPassword = "test" };
            var userManager = MockIdentity.GetMockUserManager();
            var signInManager = MockIdentity.GetMockSignInManager();
            var user = new User { UserName = "test" };

            AuthController controller = new AuthController(userManager.Object,
                signInManager.Object, configMock.Object, mapperMock.Object,
                wrapperMock.Object);


            userManager.Setup(um => um.CreateAsync(user, "test"))
            .Returns(Task.FromResult(IdentityResult.Failed()));

            wrapperMock.Setup(u => u.UserRepository.GetLastUser())
                .Returns(Task.FromResult(new User { Id = 1}));

            mapperMock.Setup(m => m.Map<User>(userForRegister)).Returns(user);

            //Act
            var action = await controller.Register(userForRegister) as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
        }
    }
}
