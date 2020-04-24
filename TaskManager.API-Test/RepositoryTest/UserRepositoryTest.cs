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

namespace TaskManager.API_Test.RepositoryTest
{

    //To start test change in UserRepository (TestDataContext context to TestDataContext dataContext);
    public class UserRepositoryTest
    {
        [Fact]
        public async Task GetLastUserTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Get_Last_User")
                .Options;

            using (var context = new DataContext(options))
            {
                context.Users.Add(new User { Id = 1, UserName = "test1" });
                context.Users.Add(new User { Id = 2, UserName = "test2" });
                context.Users.Add(new User { Id = 3, UserName = "test3" });
                context.SaveChanges();

                var userRepository = new UserRepository(context);
       
                //Act
                var action = await userRepository.GetLastUser();

                //Assert
                Assert.Equal(3, action.Id);
            }
        }

        [Fact]
        public async Task GetProjectUsersTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetProjectUsers")
                .Options;

            using (var context = new DataContext(options))
            {
                var user1 = new User { Id = 1 };
                var user2 = new User { Id = 2 };
                context.UserProjects.Add(new UserProject { ProjectId = 1, User = user1 });
                context.UserProjects.Add(new UserProject { ProjectId = 1,  User = user2 });
                context.SaveChanges();

                var repository = new UserRepository(context);

                //Act
                var action = await repository.GetProjectUsers(1);

                //Assert
                Assert.Equal(2, action.Count());
            }
        }

        [Fact]
        public async Task GetUserByNickTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Get_User_By_Nick")
                .Options;

            using (var context = new DataContext(options))
            {
                context.Users.Add(new User { Id = 1, UserName = "test1", Nickname = "test" });
                context.SaveChanges();
 
                var userRepository = new UserRepository(context);

                //Act
                var action = await userRepository.GetUserByNick("test");

                //Assert
                Assert.Equal(1, action.Id);
                Assert.Equal("test1", action.UserName);
            }
        }


    }
}
