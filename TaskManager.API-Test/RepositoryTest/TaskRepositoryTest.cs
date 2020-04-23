using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.Data;
using TaskManager.API.Data.Repository.TaskRepo;
using TaskManager.API.Model;
using Xunit;

namespace TaskManager.API_Test.RepositoryTest
{
    public class TaskRepositoryTest
    {

        [Fact]
        public async Task GetTaskTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetTask")
                .Options;

            using (var context = new DataContext(options))
            {
                context.Add(new PTask { Name = "test", PTaskId = 1 });
                context.SaveChanges();

                var repository = new TaskRepository(context);

                //Act
                var action = await repository.GetTask(1);

                //Assert
                Assert.Equal("test", action.Name);
            }
        }

        [Fact]
        public async Task GetTasksTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetTasks")
                .Options;

            using (var context = new DataContext(options))
            {
                var time = DateTime.Now.AddDays(1);
                var user1 = new User { Id = 1 };
                var user2 = new User { Id = 2 };
                var userProject1 = new List<UserProject>() { new UserProject { ProjectId = 1, User = user1 } };
                var userProject2 = new List<UserProject>() { new UserProject { ProjectId = 2, User = user2 } };
                var project = new Project { ProjectId = 1, UserProjects = userProject1 };
                context.Add(new PTask { Name = "test", PTaskId = 1, Project = project, TimeToEnd = time });
                context.Add(new PTask { Name = "test", PTaskId = 2, Project = project, TimeToEnd = time });
                context.SaveChanges();
                   
                var repository = new TaskRepository(context);

                //Act
                var action = await repository.GetTasks(1, 0);

                //Assert
                Assert.Equal(2, action.Count());
            }
        }

        [Fact]
        public async Task GetImportantTasksTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetImportantTasks")
                .Options;

            using (var context = new DataContext(options))
            {
                var time = DateTime.Now.AddDays(1);
                var project = new Project { ProjectId = 1 };
                context.Add(new PTask { PTaskId = 1, Project = project, Owner = 1, Priority = "High", 
                TimeToEnd = time });
                context.Add(new PTask { PTaskId = 2, Project = project, Owner = 1, Priority = "High",
                    TimeToEnd = time });
                context.SaveChanges();

                var repository = new TaskRepository(context);

                //Act
                var action = await repository.GetImportantTasks(1, 0);

                //Assert
                Assert.Equal(2, action.Count());
            }
        }
    }
}
