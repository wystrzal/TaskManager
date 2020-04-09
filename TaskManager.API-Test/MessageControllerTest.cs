using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.API.Controllers;
using TaskManager.API.Data.Repository;
using TaskManager.API.Data.Repository.MessageRepo;
using TaskManager.API.Data.Repository.UserRepo;
using TaskManager.API.Dto.Message;
using Xunit;

namespace TaskManager.API_Test
{
    public class MessageControllerTest
    {
        readonly Mock<IMainRepository> mainRepositoryMock;
        readonly Mock<IMessageRepository> messageRepositoryMock;
        readonly Mock<IUserRepository> userRepositoryMock;
        readonly Mock<IMapper> mapperMock;


        public MessageControllerTest()
        {
            mainRepositoryMock = new Mock<IMainRepository>();
            messageRepositoryMock = new Mock<IMessageRepository>();
            userRepositoryMock = new Mock<IUserRepository>();
            mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task AddMessageUnauthorizedStatus()
        {
            //Arrange
            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
                userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act
            var action = await controller.AddMessage(2, It.IsAny<string>(), It.IsAny<MessageForAdd>()) as UnauthorizedResult;
                

            //Assert
            Assert.Equal(401, action.StatusCode);
        }
    }
}
