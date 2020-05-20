using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.Controllers;
using TaskManager.API.Data.Repository;
using TaskManager.API.Dto.User;
using TaskManager.API.Model;
using Xunit;

namespace TaskManager.API_Test.ControllersTest
{
    public class UserControllerTest
    {
        private readonly Mock<IRepositoryWrapper> wrapperMock;
        private readonly Mock<IMapper> mapperMock;

        public UserControllerTest()
        {
            wrapperMock = new Mock<IRepositoryWrapper>();
            mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetUserNotFoundStatus()
        {
            //Arrange
            var userManager = MockIdentity.GetMockUserManager();
            
            UserController controller = new UserController(wrapperMock.Object, mapperMock.Object,
                userManager.Object);

            userManager.Setup(um => um.FindByIdAsync("1")).Returns(Task.FromResult((User)null));

            //Act
            var action = await controller.GetUser(1) as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task GetUserOkStatus()
        {
            //Arrange
            var user = new User { Id = 1, PhotoId = 1 };
            var userForReturn = new UserForReturn { PhotoId = 1 };
            var userManager = MockIdentity.GetMockUserManager();

            UserController controller = new UserController(wrapperMock.Object, mapperMock.Object,
                userManager.Object);

            userManager.Setup(um => um.FindByIdAsync("1")).Returns(Task.FromResult(user));

            mapperMock.Setup(m => m.Map<UserForReturn>(user)).Returns(userForReturn);

            //Act
            var action = await controller.GetUser(1) as OkObjectResult;
            var item = action.Value as UserForReturn;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.Equal(1, item.PhotoId);
        }

        [Fact]
        public async Task ChangePhotoNotFoundStatus()
        {
            //Arrange
            var userManager = MockIdentity.GetMockUserManager();

            UserController controller = new UserController(wrapperMock.Object, mapperMock.Object,
                userManager.Object);

            userManager.Setup(um => um.FindByIdAsync("1")).Returns(Task.FromResult((User)null));

            //Act
            var action = await controller.ChangePhoto(1, 1) as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }
    }
}
