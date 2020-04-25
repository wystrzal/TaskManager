using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.Controllers;
using TaskManager.API.Data.Repository;
using TaskManager.API.Dto.Project;
using TaskManager.API.Model;
using Xunit;

namespace TaskManager.API_Test.ControllersTest
{
    public class ProjectControllerTest
    {
        private readonly Mock<IRepositoryWrapper> wrapperMock;
        private readonly Mock<IMapper> mapperMock;

        public ProjectControllerTest()
        {
            wrapperMock = new Mock<IRepositoryWrapper>();
            mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task AddProjectUnauthorizedStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            //Act
            var action = await controller.AddProject(It.IsAny<ProjectForAdd>(), 2) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task AddProjectBadRequestStatus()
        {
            //Arrange
            var projectForAdd = new ProjectForAdd { Name = "test" };
            var project = new Project { Name = "test", ProjectId = 1 };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            mapperMock.Setup(m => m.Map<Project>(projectForAdd)).Returns(project);

            wrapperMock.Setup(w => w.ProjectRepository.Add(project));

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(false));

            //Act
            var action = await controller.AddProject(projectForAdd, 1) as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task AddProjectCreatedAtRouteStatus()
        {
            //Arrange
            var projectForAdd = new ProjectForAdd { Name = "test", Type = "personal" };
            var project = new Project { Name = "test", ProjectId = 1 };
            var userProject = new UserProject { ProjectId = 1, UserId = 1, Status = "active" };
            var projectForReturnAdded = new ProjectForReturnAdded { Name = "test", ProjectId = 1, Type = "personal" };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            mapperMock.Setup(m => m.Map<Project>(projectForAdd)).Returns(project);

            wrapperMock.Setup(w => w.ProjectRepository.Add(project));

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(true)).Verifiable();

            wrapperMock.Setup(w => w.ProjectRepository.Add(userProject));

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(true)).Verifiable();

            mapperMock.Setup(m => m.Map<ProjectForReturnAdded>(projectForAdd)).Returns(projectForReturnAdded);

            //Act
            var action = await controller.AddProject(projectForAdd, 1) as CreatedAtRouteResult;

            //Assert
            wrapperMock.Verify(w => w.SaveAll(), Times.Exactly(2));
            Assert.Equal(201, action.StatusCode);
            Assert.Equal("GetProject", action.RouteName);
        }

        [Fact]
        public async Task DeleteProjectUnauthorizedUserStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            //Act
            var action = await controller.DeleteProject(2, It.IsAny<int>()) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task DeleteProjectNotFoundStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult((Project)null));

            //Act
            var action = await controller.DeleteProject(1, 1) as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task DeleteProjectUnauthorizedOwnerStatus()
        {
            //Arrange
            var project = new Project { Owner = 2, ProjectId = 1 };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            //Act
            var action = await controller.DeleteProject(1, 1) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task DeleteProjectBadRequestStatus()
        {
            //Arrange
            var project = new Project { Owner = 1, ProjectId = 1 };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            wrapperMock.Setup(w => w.ProjectRepository.Delete(project)).Verifiable();

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(false));

            //Act
            var action = await controller.DeleteProject(1, 1) as BadRequestObjectResult;

            //Assert
            wrapperMock.Verify(w => w.ProjectRepository.Delete(project), Times.Once);
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task DeleteProjectOkStatus()
        {
            //Arrange
            var project = new Project { Owner = 1, ProjectId = 1 };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            wrapperMock.Setup(w => w.ProjectRepository.Delete(project)).Verifiable();

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(true));

            //Act
            var action = await controller.DeleteProject(1, 1) as OkResult;

            //Assert
            wrapperMock.Verify(w => w.ProjectRepository.Delete(project), Times.Once);
            Assert.Equal(200, action.StatusCode);
        }

        [Fact]
        public async Task GetProjectUnauthorizedStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            //Act
            var action = await controller.GetProject(2, It.IsAny<int>()) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task GetProjectNotFoundStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult((Project)null));

            //Act
            var action = await controller.GetProject(1, 1) as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task GetProjectOkStatus()
        {
            //Arrange
            var project = new Project { ProjectId = 1, Name = "test"};
            var projectForReturn = new ProjectForReturn { ProjectId = 1, Name = "test"};
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            var mapper = mapperMock.Setup(m => m.Map<ProjectForReturn>(project)).Returns(projectForReturn);

            //Act
            var action = await controller.GetProject(1, 1) as OkObjectResult;
            var item = action.Value as ProjectForReturn;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.Equal("test", projectForReturn.Name);
        }

        [Fact]
        public async Task GetProjectUsersUnauthorizedStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            //Act
            var action = await controller.GetProjectUsers(2, It.IsAny<int>()) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }


        [Fact]
        public async Task GetProjectUsersOkStatus()
        {
            //Arrange
            IEnumerable<User> user = new List<User>()
            {
                new User {Id = 1},
                new User {Id = 2}
            };

            IEnumerable<ProjectForReturnUsers> projectForReturnUsers = new List<ProjectForReturnUsers>()
            {
                new ProjectForReturnUsers {UserId = 1},
                new ProjectForReturnUsers {UserId = 2}
            };
            
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.UserRepository.GetProjectUsers(1)).Returns(Task.FromResult(user));

            mapperMock.Setup(m => m.Map<IEnumerable<ProjectForReturnUsers>>(user)).Returns(projectForReturnUsers);

            //Act
            var action = await controller.GetProjectUsers(1, 1) as OkObjectResult;
            var item = action.Value as IEnumerable<ProjectForReturnUsers>;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.Equal(2, item.Count());
        }

        [Fact]
        public async Task GetProjectsUnauthorizedStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            //Act
            var action = await controller.GetProjects(2, It.IsAny<string>(), It.IsAny<int>()) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task GetProjectsOkStatus()
        {
            //Arrange
            IEnumerable<Project> project = new List<Project>()
            {
                new Project {ProjectId = 1},
                new Project {ProjectId = 2}
            };

            IEnumerable<ProjectForReturn> projectForReturn = new List<ProjectForReturn>()
            {
                new ProjectForReturn {ProjectId = 1},
                new ProjectForReturn {ProjectId = 2}
            };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProjects(1, "all", 0)).Returns(Task.FromResult(project));

            mapperMock.Setup(m => m.Map<IEnumerable<ProjectForReturn>>(project)).Returns(projectForReturn);

            //Act
            var action = await controller.GetProjects(1, "all", 0) as OkObjectResult;
            var item = action.Value as IEnumerable<ProjectForReturn>;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.Equal(2, item.Count());
        }

        [Fact]
        public async Task GetInvitationsUnauthorizedStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            //Act
            var action = await controller.GetInvitations(2) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task GetInvitationsOkStatus()
        {
            //Arrange
            IEnumerable<Project> project = new List<Project>()
            {
                new Project {ProjectId = 1},
                new Project {ProjectId = 2}
            };

            IEnumerable<ProjectForReturnInvitations> projectForReturn = new List<ProjectForReturnInvitations>()
            {
                new ProjectForReturnInvitations {ProjectId = 1},
                new ProjectForReturnInvitations {ProjectId = 2}
            };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetInvitationsToProject(1)).Returns(Task.FromResult(project));

            mapperMock.Setup(m => m.Map<IEnumerable<ProjectForReturnInvitations>>(project)).Returns(projectForReturn);

            //Act
            var action = await controller.GetInvitations(1) as OkObjectResult;
            var item = action.Value as IEnumerable<ProjectForReturnInvitations>;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.Equal(2, item.Count());
        }

        [Fact]
        public async Task AddToProjectUnauthorizedUserStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            //Act
            var action = await controller.AddToProject(2, It.IsAny<int>(), It.IsAny<string>()) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }


        [Fact]
        public async Task AddToProjectNotFoundUserStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.UserRepository.GetUserByNick("test")).Returns(Task.FromResult((User)null));

            //Act
            var action = await controller.AddToProject(1, It.IsAny<int>(), "test") as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task AddToProjectNotFoundProjectStatus()
        {
            //Arrange
            var user = new User { Id = 1, Nickname = "test" };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.UserRepository.GetUserByNick("test")).Returns(Task.FromResult(user));

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult((Project)null));

            //Act
            var action = await controller.AddToProject(1, 1, "test") as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task AddToProjectUnauthorizedOwnerStatus()
        {
            //Arrange
            var user = new User { Id = 1, Nickname = "test" };
            var project = new Project { ProjectId = 1, Owner = 2 };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.UserRepository.GetUserByNick("test")).Returns(Task.FromResult(user));

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            //Act
            var action = await controller.AddToProject(1, 1, "test") as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task AddToProjectBadRequestInvitedStatus()
        {
            //Arrange
            List<UserProject> userProject = new List<UserProject>() {
                new UserProject { UserId = 2, Status = "invited" }
            };

            var user = new User { Id = 2, Nickname = "test" };
            var project = new Project { ProjectId = 1, Owner = 1, UserProjects = userProject };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.UserRepository.GetUserByNick("test")).Returns(Task.FromResult(user));

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            //Act
            var action = await controller.AddToProject(1, 1, "test") as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.Contains("currently invited", action.Value.ToString());
        }

        [Fact]
        public async Task AddToProjectBadRequestActiveStatus()
        {
            //Arrange
            List<UserProject> userProject = new List<UserProject>() {
                new UserProject { UserId = 2, Status = "active" }
            };

            var user = new User { Id = 2, Nickname = "test" };
            var project = new Project { ProjectId = 1, Owner = 1, UserProjects = userProject };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.UserRepository.GetUserByNick("test")).Returns(Task.FromResult(user));

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            //Act
            var action = await controller.AddToProject(1, 1, "test") as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.Contains("already belong", action.Value.ToString());
        }

        [Fact]
        public async Task AddToProjectBadRequestAlreadyBelongStatus()
        {
            //Arrange
            List<UserProject> userProject = new List<UserProject>() {
                new UserProject { UserId = 2, Status = null }
            };

            var addUserProject = new UserProject { ProjectId = 1, UserId = 2, Status = "invited" };
            var user = new User { Id = 2, Nickname = "test" };
            var project = new Project { ProjectId = 1, Owner = 1, UserProjects = userProject };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.UserRepository.GetUserByNick("test")).Returns(Task.FromResult(user));

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            wrapperMock.Setup(w => w.ProjectRepository.Add(userProject));

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(false));

            //Act
            var action = await controller.AddToProject(1, 1, "test") as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task AddToProjectOkStatus()
        {
            //Arrange
            List<UserProject> userProject = new List<UserProject>() {
                new UserProject { UserId = 2, Status = null }
            };

            var addUserProject = new UserProject { ProjectId = 1, UserId = 2, Status = "invited" };
            var user = new User { Id = 2, Nickname = "test" };
            var project = new Project { ProjectId = 1, Owner = 1, UserProjects = userProject };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.UserRepository.GetUserByNick("test")).Returns(Task.FromResult(user));

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            wrapperMock.Setup(w => w.ProjectRepository.Add(userProject));

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(true));

            //Act
            var action = await controller.AddToProject(1, 1, "test") as OkResult;

            //Assert
            Assert.Equal(200, action.StatusCode);
        }

        [Fact]
        public async Task DeleteFromProjectUnauthorizedStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            //Act
            var action = await controller.DeleteFromProject(2, It.IsAny<int>(), It.IsAny<int>()) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task DeleteFromProjectNotFoundProjectStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(It.IsAny<int>())).Returns(Task.FromResult((Project)null));

            //Act
            var action = await controller.DeleteFromProject(1, It.IsAny<int>(), It.IsAny<int>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task DeleteFromProjectNotFoundUserStatus()
        {
            //Arrange
            var userProjects = new List<UserProject>() { new UserProject { UserId = 2 } };
            var project = new Project { ProjectId = 1, UserProjects = userProjects };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            //Act
            var action = await controller.DeleteFromProject(1, 1, 1) as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task DeleteFromProjectBadRequestStatus()
        {
            //Arrange
            var userProjects = new List<UserProject>() { new UserProject { UserId = 2 } };
            var project = new Project { ProjectId = 1, UserProjects = userProjects };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            wrapperMock.Setup(w => w.ProjectRepository.Delete(userProjects[0])).Verifiable();

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(false));

            //Act
            var action = await controller.DeleteFromProject(1, 1, 2) as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
            wrapperMock.Verify(w => w.ProjectRepository.Delete(userProjects[0]), Times.Once);
        }

        [Fact]
        public async Task DeleteFromProjectOkStatus()
        {
            //Arrange
            var userProjects = new List<UserProject>() { new UserProject { UserId = 2 } };
            var project = new Project { ProjectId = 1, UserProjects = userProjects };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            wrapperMock.Setup(w => w.ProjectRepository.Delete(userProjects[0])).Verifiable();

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(true));

            //Act
            var action = await controller.DeleteFromProject(1, 1, 2) as OkResult;

            //Assert
            Assert.Equal(200, action.StatusCode);
            wrapperMock.Verify(w => w.ProjectRepository.Delete(userProjects[0]), Times.Once);
        }

        [Fact]
        public async Task JoinToProjectUnauthorizedStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            //Act
            var action = await controller.JoinToProject(2, It.IsAny<int>(), It.IsAny<int>()) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task JoinToProjectNotFoundStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult((Project)null));

            //Act
            var action = await controller.JoinToProject(1, 1, It.IsAny<int>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task JoinToProjectBadRequestStatus()
        {
            //Arrange
            var userProjects = new List<UserProject>() { new UserProject { UserId = 1, ProjectId = 1 } };
            var project = new Project { ProjectId = 1, UserProjects = userProjects };
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(false));

            //Act
            var action = await controller.JoinToProject(1, 1, 1) as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task JoinToProjectOkStatus()
        {
            //Arrange
            var userProjects = new List<UserProject>() { new UserProject { UserId = 1, ProjectId = 1 } };
            var project = new Project { ProjectId = 1, UserProjects = userProjects };
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(true));

            //Act
            var action = await controller.JoinToProject(1, 1, 2) as OkResult;

            //Assert
            Assert.Equal(200, action.StatusCode);
        }

        [Fact]
        public async Task JoinToProjectOkObjectStatus()
        {
            //Arrange
            var userProjects = new List<UserProject>() { new UserProject { UserId = 1, ProjectId = 1 } };
            var project = new Project { ProjectId = 1, UserProjects = userProjects };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(true));

            mapperMock.Setup(m => m.Map<ProjectForReturn>(project)).Returns(new ProjectForReturn { ProjectId = 1 });

            //Act
            var action = await controller.JoinToProject(1, 1, 1) as OkObjectResult;
            var item = action.Value as ProjectForReturn;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.Equal(1, item.ProjectId);
        }

        [Fact]
        public async Task LeaveProjectUnauthorizedStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            //Act
            var action = await controller.LeaveProject(2, It.IsAny<int>()) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task LeaveProjectNotFoundStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult((Project)null));

            //Act
            var action = await controller.LeaveProject(1, It.IsAny<int>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task LeaveProjectBadRequestStatus()
        {
            //Arrange
            var userProjects = new List<UserProject>() { new UserProject {ProjectId = 1, UserId = 1 } };
            var project = new Project { ProjectId = 1, UserProjects = userProjects };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(false));

            //Act
            var action = await controller.LeaveProject(1, 1) as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task LeaveProjectOkStatus()
        {
            //Arrange
            var userProjects = new List<UserProject>() { new UserProject { ProjectId = 1, UserId = 1 } };
            var project = new Project { ProjectId = 1, UserProjects = userProjects };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(true));

            //Act
            var action = await controller.LeaveProject(1, 1) as OkResult;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.Equal("inactive", project.UserProjects.First().Status);
        }


        [Fact]
        public async Task ChangeProjectNameUnauthorizedStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            //Act
            var action = await controller.ChangeProjectName(2, It.IsAny<int>(), It.IsAny<ProjectForChangeName>()) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task ChangeProjectNameNotFoundStatus()
        {
            //Arrange
            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult((Project)null));

            //Act
            var action = await controller.ChangeProjectName(1, 1, It.IsAny<ProjectForChangeName>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task ChangeProjectNameBadRequestStatus()
        {
            //Arrange
            var projectForChangeName = new ProjectForChangeName { Name = "newTest" };
            var project = new Project { ProjectId = 1, Name = "test"};

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            mapperMock.Setup(m => m.Map(projectForChangeName, project)).Verifiable();

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(false));

            //Act
            var action = await controller.ChangeProjectName(1, 1, projectForChangeName) as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
            mapperMock.Verify(m => m.Map(projectForChangeName, project), Times.Once());
        }


        [Fact]
        public async Task ChangeProjectNameOkStatus()
        {
            //Arrange
            var projectForChangeName = new ProjectForChangeName { Name = "newTest" };
            var project = new Project { ProjectId = 1, Name = "test" };

            ProjectController controller = new ProjectController(mapperMock.Object, wrapperMock.Object);

            Authorization.GetIdentity(controller);

            wrapperMock.Setup(w => w.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));

            mapperMock.Setup(m => m.Map(projectForChangeName, project)).Verifiable();

            wrapperMock.Setup(w => w.SaveAll()).Returns(Task.FromResult(true));

            //Act
            var action = await controller.ChangeProjectName(1, 1, projectForChangeName) as OkResult;

            //Assert
            Assert.Equal(200, action.StatusCode);
            mapperMock.Verify(m => m.Map(projectForChangeName, project), Times.Once());
        }
    }
}














