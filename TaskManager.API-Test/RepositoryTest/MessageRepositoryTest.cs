using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.Data;
using TaskManager.API.Data.Repository.MessageRepo;
using TaskManager.API.Model;
using Xunit;

namespace TaskManager.API_Test.RepositoryTest
{
    public class MessageRepositoryTest
    {
        [Fact]
        public async Task GetMessageTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetMessage")
                .Options;

            using (var context = new DataContext(options))
            {
                var user1 = new User { Id = 1 };
                var user2 = new User { Id = 2 };
                var id = 1;

                context.Add(new Message
                {
                    Content = "test", MessageId = 1, Recipient = user1, Sender = user2
                });
                context.Add(new Message
                {
                    Content = "test", MessageId = 2, Recipient = user2, Sender = user1
                });
                context.SaveChanges();

                var repository = new MessageRepository(context);

                //Act
                var action = await repository.GetMessage(id);

                //Assert
                Assert.Equal("test", action.Content);
                Assert.Equal(id, action.RecipientId);
            }
        }

        [Fact]
        public async Task GetReceivedMessagesTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetReceivedMessages")
                .Options;

            using (var context = new DataContext(options))
            {
                var user1 = new User { Id = 1 };
                var user2 = new User { Id = 2 };

                context.Add(new Message
                {
                    Content = "test", MessageId = 1,  Recipient = user1, Sender = user2
                });
                context.Add(new Message
                {
                    Content = "test", MessageId = 2, Recipient = user1, Sender = user2
                });
                context.Add(new Message
                {
                    Content = "test", MessageId = 3, Recipient = user2, Sender = user1
                });
                context.SaveChanges();

                var repository = new MessageRepository(context);

                //Act
                var action = await repository.GetReceivedMessages(user1.Id, 0);

                //Assert
                Assert.Equal(2, action.Count());
            }
        }

        [Fact]
        public async Task GetSendedMessagesTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetSendedMessages")
                .Options;

            using (var context = new DataContext(options))
            {
                var user1 = new User { Id = 1 };
                var user2 = new User { Id = 2 };

                context.Add(new Message
                {
                    Content = "test", MessageId = 1, Recipient = user1, Sender = user2
                });
                context.Add(new Message
                {
                    Content = "test", MessageId = 2, Recipient = user1, Sender = user2
                });
                context.Add(new Message
                {
                    Content = "test", MessageId = 3, Recipient = user2, Sender = user1
                });
                context.SaveChanges();

                var repository = new MessageRepository(context);

                //Act
                var action = await repository.GetSendedMessages(user2.Id, 0);

                //Assert
                Assert.Equal(2, action.Count());
            }
        }

        [Fact]
        public void AddTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Add")
                .Options;

            using (var context = new DataContext(options))
            {
                var user1 = new User { Id = 1 };
                var user2 = new User { Id = 2 };

                var message = new Message
                {
                    Content = "test",
                    Recipient = user2,
                    Sender = user1
                };

                var repository = new MessageRepository(context);

                //Act
                repository.Add(message);
                context.SaveChanges();
                var addedMessage = context.Messages.Find(1);

                //Assert
                Assert.Equal(1, addedMessage.MessageId);
                Assert.Equal("test", addedMessage.Content);
            }
        }

        [Fact]
        public void DeleteTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Delete")
                .Options;

            using (var context = new DataContext(options))
            {
                var user1 = new User { Id = 1 };
                var user2 = new User { Id = 2 };

                var message = new Message
                {
                    Content = "test",
                    MessageId = 1,
                    Recipient = user2,
                    Sender = user1
                };
                context.Messages.Add(message);
                context.SaveChanges();

                var repository = new MessageRepository(context);

                //Act
                repository.Delete(message);
                context.SaveChanges();
                var addedMessage = context.Messages.Find(1);

                //Assert
                Assert.Null(addedMessage);
            }
        }
    }
}
