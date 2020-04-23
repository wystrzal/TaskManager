using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.Data;
using TaskManager.API.Data.Repository.ProjectRepo;
using TaskManager.API.Model;
using Xunit;

namespace TaskManager.API_Test.RepositoryTest
{
    public class ProjectRepositoryTest
    {

        [Fact]
        public async Task GetInvitationsToProjectTask()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetInvitationsToProject")
                .Options;

            using (var context = new DataContext(options))
            {
                var project1 = new Project { Name = "test", Owner = 2, ProjectId = 1, Type = "group" };
                var project2 = new Project { Name = "test", Owner = 2, ProjectId = 2, Type = "group" };

                context.UserProjects.Add(new UserProject
                {
                    UserId = 1, Status = "invited",  Project = project1
                });
                context.UserProjects.Add(new UserProject
                {
                    UserId = 1, Status = "invited", Project = project2
                });
                context.SaveChanges();

                var repository = new ProjectRepository(context);

                //Act
                var action = await repository.GetInvitationsToProject(1);

                //Assert
                Assert.Equal(2, action.Count());
            }
        }
        [Fact]
        public async Task GetProjectTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetProject")
                .Options;

            using (var context = new DataContext(options))
            {
                var user = new User { Id = 1 };
                var userProject = new List<UserProject>()
                { new UserProject { ProjectId = 1, User = user} };

                var task = new List<PTask>() 
                { new PTask { PTaskId = 1 } };

                context.Projects.Add(new Project
                {
                    ProjectId = 1, UserProjects = userProject, PTasks = task, Name = "test"
                });
                context.SaveChanges();

                var repository = new ProjectRepository(context);

                //Act
                var action = await repository.GetProject(1);

                //Assert
                Assert.Equal("test", action.Name);
            }
        }

        [Fact]
        public async Task GetProjectsAllTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetProjectsAll")
                .Options;

            using (var context = new DataContext(options))
            {
                var project1 = new Project { ProjectId = 1, Type = "group" };
                var project2 = new Project { ProjectId = 2, Type = "group" };
                context.UserProjects.Add(new UserProject { Project = project1, UserId = 1, Status = "active" });
                context.UserProjects.Add(new UserProject { Project = project2, UserId = 1, Status = "active" });
                context.SaveChanges();

                var repository = new ProjectRepository(context);

                //Act
                var action = await repository.GetProjects(1, "all", 0);

                //Assert
                Assert.Equal(2, action.Count());
            }
        }

        [Fact]
        public async Task GetProjectsWithTypeTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetProjectsType")
                .Options;

            using (var context = new DataContext(options))
            {
                var project1 = new Project { ProjectId = 1, Type = "group" };
                var project2 = new Project { ProjectId = 2, Type = "group" };
                context.UserProjects.Add(new UserProject { Project = project1, UserId = 1, Status = "active" });
                context.UserProjects.Add(new UserProject { Project = project2, UserId = 1, Status = "active" });
                context.SaveChanges();

                var repository = new ProjectRepository(context);

                //Act
                var action = await repository.GetProjects(1, "group", 0);

                //Assert
                Assert.Equal(2, action.Count());
            }
        }
    }
}
