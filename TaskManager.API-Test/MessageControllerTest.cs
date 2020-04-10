using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
using TaskManager.API.Model;
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

        [Fact]
        public async Task AddMessageNotFoundStatus()
        {
            //Arrange
            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
                userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act
            var action = await controller.AddMessage(1, It.IsAny<string>(), It.IsAny<MessageForAdd>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);       
        }

        [Fact]
        public async Task AddMessageBadRequestStatus()
        {
            //Arrange
            var nick = "nick";
            User user = new User { Nickname = nick, Id = 1 };
            Message message = new Message { MessageId = 1 };

            userRepositoryMock.Setup(u => u.GetUserByNick(nick)).Returns(() => Task.Run(() =>
            { return user; }));

            mapperMock.Setup(m => m.Map<Message>(It.IsAny<MessageForAdd>())).Returns(message);

            mainRepositoryMock.Setup(m => m.Add(message));

            mainRepositoryMock.Setup(m => m.SaveAll()).Returns(() => Task.Run(() => { return false; }));

            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
                userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act
            var action = await controller.AddMessage(1, nick, It.IsAny<MessageForAdd>()) as BadRequestObjectResult; ;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task AddMessageOkStatus()
        {
            //Arrange
            var nick = "nick";
            User user = new User { Nickname = nick, Id = 1 };
            Message message = new Message { MessageId = 1 };

            userRepositoryMock.Setup(u => u.GetUserByNick(nick)).Returns(() => Task.Run(() =>
            { return user; }));

            mapperMock.Setup(m => m.Map<Message>(It.IsAny<MessageForAdd>())).Returns(message);

            mainRepositoryMock.Setup(m => m.Add(message)).Verifiable();

            mainRepositoryMock.Setup(m => m.SaveAll()).Returns(() => Task.Run(() => { return true; }));

            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
                userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act
            var action = await controller.AddMessage(1, nick, It.IsAny<MessageForAdd>()) as OkResult; ;

            //Assert
            Assert.Equal(200, action.StatusCode);
        }

        [Fact]
        public async Task GetReceivedMessagesUnauthorizedStatus()
        {
            //Arrange
            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
              userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act 
            var action = await controller.GetReceivedMessages(2, 0) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task GetReceivedMessagesOkStatus()
        {
            //Arrange
            IEnumerable<Message> messages = new List<Message> { new Message { MessageId = 1, RecipientId = 1},
                new Message { MessageId = 2, RecipientId = 1} };

            IEnumerable<MessageForReturnReceivedMessages> messagesForReturn = new List<MessageForReturnReceivedMessages>
            {
                new MessageForReturnReceivedMessages {MessageId = 1},
                new MessageForReturnReceivedMessages {MessageId = 2}
            };

            messageRepositoryMock.Setup(m => m.GetReceivedMessages(1, 0)).Returns(() => Task.Run(() =>
            {
                return messages;
            }));

            mapperMock.Setup(m => m.Map<IEnumerable<MessageForReturnReceivedMessages>>(messages)).Returns(messagesForReturn);

            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
              userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act 
            var action = await controller.GetReceivedMessages(1, 0) as OkObjectResult;
            var items = action.Value as IEnumerable<MessageForReturnReceivedMessages>;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public async Task GetSendedMessagesUnauthorizedStatus()
        {
            //Arrange
            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
              userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act 
            var action = await controller.GetSendedMessages(2, 0) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task GetSendedMessagesOkStatus()
        {
            //Arrange
            IEnumerable<Message> messages = new List<Message> { new Message { MessageId = 1, RecipientId = 1},
                new Message { MessageId = 2, RecipientId = 1} };

            IEnumerable<MessageForReturnSendedMessages> messagesForReturn = new List<MessageForReturnSendedMessages>
            {
                new MessageForReturnSendedMessages {MessageId = 1},
                new MessageForReturnSendedMessages {MessageId = 2}
            };

            messageRepositoryMock.Setup(m => m.GetSendedMessages(1, 0)).Returns(() => Task.Run(() =>
            {
                return messages;
            }));

            mapperMock.Setup(m => m.Map<IEnumerable<MessageForReturnSendedMessages>>(messages)).Returns(messagesForReturn);

            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
              userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act 
            var action = await controller.GetSendedMessages(1, 0) as OkObjectResult;
            var items = action.Value as IEnumerable<MessageForReturnSendedMessages>;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public async Task GetMessageUnauthorizedStatus()
        {
            //Arrange
            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
                userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act
            var action = await controller.GetMessage(1, 2) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task GetMessageNotFoundStatus()
        {
            //Arrange           
            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
                userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act
            var action = await controller.GetMessage(1, 1) as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task GetMessageOkStatus()
        {
            //Arrange
            Message message = new Message() { MessageId = 1, Content = "test"};
            MessageForReturnDetailMessage messageForReturn = new MessageForReturnDetailMessage { Content = "test" };

            messageRepositoryMock.Setup(m => m.GetMessage(1)).Returns(() => Task.Run(() =>
            {
                return message;
            }));

            mapperMock.Setup(m => m.Map<MessageForReturnDetailMessage>(message)).Returns(messageForReturn);

            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
                userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act
            var action = await controller.GetMessage(1, 1) as OkObjectResult;
            var item = action.Value as MessageForReturnDetailMessage;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.Equal("test", item.Content);
        }

        [Fact]
        public async Task DeleteMessageUnauthorizedStatus()
        {
            //Arrange
            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
             userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act
            var action = await controller.DeleteMessage(1, 2, "test") as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task DeleteMessageNotFoundStatus()
        {
            //Arrange
            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
             userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act
            var action = await controller.DeleteMessage(1, 1, "test") as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task DeleteMessageNoContentStatusRecipientDelete()
        {
            //Arrange
            Message message = new Message { MessageId = 1, RecipientDeleted = false, SenderDeleted = false };

            messageRepositoryMock.Setup(m => m.GetMessage(1)).Returns(() => Task.Run(() =>
            {
                return message;
            }));

            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
             userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act
            var action = await controller.DeleteMessage(1, 1, "recipient") as NoContentResult;

            //Assert
            Assert.Equal(204, action.StatusCode);
            Assert.True(message.RecipientDeleted);
            Assert.False(message.SenderDeleted);
        }

        [Fact]
        public async Task DeleteMessageNoContentStatusSenderDelete()
        {
            //Arrange
            Message message = new Message { MessageId = 1, RecipientDeleted = false, SenderDeleted = false };

            messageRepositoryMock.Setup(m => m.GetMessage(1)).Returns(() => Task.Run(() =>
            {
                return message;
            }));

            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
             userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act
            var action = await controller.DeleteMessage(1, 1, "sender") as NoContentResult;

            //Assert
            Assert.Equal(204, action.StatusCode);
            Assert.False(message.RecipientDeleted);
            Assert.True(message.SenderDeleted);
        }

        [Fact]
        public async Task DeleteMessageOkStatus()
        {
            //Arrange
            Message message = new Message { MessageId = 1, RecipientDeleted = true, SenderDeleted = true };

            messageRepositoryMock.Setup(m => m.GetMessage(1)).Returns(() => Task.Run(() =>
            {
                return message;
            }));

            mainRepositoryMock.Setup(m => m.Delete(message)).Verifiable();

            mainRepositoryMock.Setup(m => m.SaveAll()).Returns(() => Task.Run(() =>
          {
              return true;
          }));

            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
             userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act
            var action = await controller.DeleteMessage(1, 1, "any") as OkResult;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.True(message.RecipientDeleted);
            Assert.True(message.SenderDeleted);
        }

        [Fact]
        public async Task DeleteMessageBadRequestStatus()
        {
            //Arrange
            Message message = new Message { MessageId = 1, RecipientDeleted = true, SenderDeleted = true };

            messageRepositoryMock.Setup(m => m.GetMessage(1)).Returns(() => Task.Run(() =>
            {
                return message;
            }));

            mainRepositoryMock.Setup(m => m.Delete(message)).Verifiable();

            mainRepositoryMock.Setup(m => m.SaveAll()).Returns(() => Task.Run(() =>
            {
                return false;
            }));

            MessageController controller = new MessageController(mainRepositoryMock.Object, messageRepositoryMock.Object,
             userRepositoryMock.Object, mapperMock.Object);

            TestIdentity.GetIdentity(controller);

            //Act
            var action = await controller.DeleteMessage(1, 1, "any") as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
        }

    }
}
