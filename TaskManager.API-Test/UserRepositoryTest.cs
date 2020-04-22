using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.Data;
using TaskManager.API.Data.Repository.UserRepo;
using TaskManager.API.Model;
using Xunit;

namespace TaskManager.API_Test
{
    public class UserRepositoryTest
    {
        [Fact]
        public async Task GetLastUser()
        {
            var options = new DbContextOptionsBuilder<TestDataContext>()
                .UseInMemoryDatabase(databaseName: "Get_Last_User")
                .Options;

            using (var context = new TestDataContext(options))
            {
                context.Users.Add(new User { Id = 1, UserName = "test1" });
                context.Users.Add(new User { Id = 2, UserName = "test2" });
                context.Users.Add(new User { Id = 3, UserName = "test3" });
                context.SaveChanges();
            }

            using (var context = new TestDataContext(options))
            {
                var userRepository = new UserRepository(context);
                var action = await userRepository.GetLastUser();
                Assert.Equal(3, action.Id);
            }
        }

        [Fact]
        public async Task GetUserByNick()
        {
            var options = new DbContextOptionsBuilder<TestDataContext>()
                .UseInMemoryDatabase(databaseName: "Get_User_By_Nick")
                .Options;

            using (var context = new TestDataContext(options))
            {
                context.Users.Add(new User { Id = 1, UserName = "test1", Nickname = "test" });
                context.SaveChanges();
            }

            using (var context = new TestDataContext(options))
            {
                var userRepository = new UserRepository(context);
                var action = await userRepository.GetUserByNick("test");
                Assert.Equal(1, action.Id);
                Assert.Equal("test1", action.UserName);
            }
        }


    }
}
