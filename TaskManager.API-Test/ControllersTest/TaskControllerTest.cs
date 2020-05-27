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
using TaskManager.API.Dto.Task;
using TaskManager.API.Model;
using Xunit;

namespace TaskManager.API_Test.ControllersTest
{
    public class TaskControllerTest
    {
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepositoryWrapper> mockRepoWrapper;
        public TaskControllerTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepoWrapper = new Mock<IRepositoryWrapper>();
        }

        [Fact]
        public async Task GetTasksOkStatus()
        {
            //Arrange
            IEnumerable<PTask> task = new List<PTask>() {
                new PTask { Name = "test", PTaskId = 1 },  new PTask { Name = "test", PTaskId = 2 }
            };
            IEnumerable<TaskForReturn> taskForReturn = new List<TaskForReturn>() {
                new TaskForReturn { TaskId = 1, Name = "test" },  new TaskForReturn { TaskId = 2, Name = "test" }
            };

            mockRepoWrapper.Setup(x => x.TaskRepository.GetTasks(1, 0)).Returns(Task.FromResult(task));
            mockMapper.Setup(x => x.Map<IEnumerable<TaskForReturn>>(task)).Returns(taskForReturn);

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act 
            var action = await controller.GetTasks(1, 0) as OkObjectResult;
            var item = action.Value as IEnumerable<TaskForReturn>;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.Equal(2, item.Count());
        }

        [Fact]
        public async Task GetImportantTasksOkStatus()
        {
            //Arrange
            IEnumerable<PTask> tasks = new List<PTask>() 
            {
                new PTask { Name = "test", PTaskId = 1},
                new PTask { Name = "test", PTaskId = 2}
            };
            IEnumerable<TaskForReturnImportant> tasksForReturn = new List<TaskForReturnImportant>()
            {
                new TaskForReturnImportant { Name = "test", TaskId = 1},
                new TaskForReturnImportant { Name = "test", TaskId = 2},
            };

            mockRepoWrapper.Setup(x => x.TaskRepository.GetImportantTasks(1, 0)).Returns(Task.FromResult(tasks));
            mockMapper.Setup(x => x.Map<IEnumerable<TaskForReturnImportant>>(tasks)).Returns(tasksForReturn);

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.GetImportantTasks(1, 0) as OkObjectResult;
            var item = action.Value as IEnumerable<TaskForReturnImportant>;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.Equal(2, item.Count());
        }

        [Fact]
        public async Task GetTaskNotFoundStatus()
        {
            //Arrange
            mockRepoWrapper.Setup(x => x.TaskRepository.GetTask(1)).Returns(Task.FromResult((PTask)null));

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.GetTask(1) as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task GetTaskOkStatus()
        {
            //Arrange
            PTask task = new PTask { Name = "test", PTaskId = 1 };
            TaskForReturn taskForReturn = new TaskForReturn { Name = "test", TaskId = 1 };

            mockRepoWrapper.Setup(x => x.TaskRepository.GetTask(1)).Returns(Task.FromResult(task));
            mockMapper.Setup(x => x.Map<TaskForReturn>(task)).Returns(taskForReturn);

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.GetTask(1) as OkObjectResult;
            var item = action.Value as TaskForReturn;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.Equal(1, item.TaskId);
        }

        [Fact]
        public async Task AddTaskNotFoundStatus()
        {
            //Arrange
            mockRepoWrapper.Setup(x => x.ProjectRepository.GetProject(1)).Returns(Task.FromResult((Project)null));

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.AddTask(1, 1, It.IsAny<TaskForAdd>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task AddTaskCreatedAtRouteStatus()
        {
            //Arrange
            ICollection<PTask> tasks = new List<PTask>() { new PTask { Name = "test" } };
            Project project = new Project { Name = "test", ProjectId = 1, PTasks = tasks  };
            TaskForAdd taskForAdd = new TaskForAdd { Name = "test" };
            PTask task = new PTask { Name = "test" };

            mockRepoWrapper.Setup(x => x.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));
            mockMapper.Setup(x => x.Map<PTask>(taskForAdd)).Returns(task);
            mockRepoWrapper.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.AddTask(1, 1, taskForAdd) as CreatedAtRouteResult;
            var item = action.Value as TaskForReturn;

            //Assert
            Assert.Equal(201, action.StatusCode);
            Assert.Equal("GetTask", action.RouteName);
        }

        [Fact]
        public async Task AddTaskBadRequestStatus()
        {
            //Arrange
            ICollection<PTask> tasks = new List<PTask>() { new PTask { Name = "test" } };
            Project project = new Project { Name = "test", ProjectId = 1, PTasks = tasks };
            TaskForAdd taskForAdd = new TaskForAdd { Name = "test" };
            PTask task = new PTask { Name = "test" };

            mockRepoWrapper.Setup(x => x.ProjectRepository.GetProject(1)).Returns(Task.FromResult(project));
            mockMapper.Setup(x => x.Map<PTask>(taskForAdd)).Returns(task);
            mockRepoWrapper.Setup(x => x.SaveAll()).Returns(Task.FromResult(false));

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.AddTask(1, 1, taskForAdd) as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task ChangeStatusPriorityNotFoundStatus()
        {
            //Arrange
            mockRepoWrapper.Setup(x => x.TaskRepository.GetTask(1)).Returns(Task.FromResult((PTask)null));

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.ChangeStatusPriority(1, "status", "high") as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }


        [Fact]
        public async Task ChangeStatusPriorityOkStatus()
        {
            //Arrange
            PTask task = new PTask { Name = "test" };

            mockRepoWrapper.Setup(x => x.TaskRepository.GetTask(1)).Returns(Task.FromResult(task));
            mockRepoWrapper.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.ChangeStatusPriority(1, "status", "high") as OkResult;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.Equal("high", task.Status);
        }

        [Fact]
        public async Task ChangeStatusPriorityBadRequestStatus()
        {
            //Arrange
            PTask task = new PTask { Name = "test" };

            mockRepoWrapper.Setup(x => x.TaskRepository.GetTask(1)).Returns(Task.FromResult(task));
            mockRepoWrapper.Setup(x => x.SaveAll()).Returns(Task.FromResult(false));

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.ChangeStatusPriority(1, "status", "high") as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task DeleteTaskNotFoundStatus()
        {
            //Arrange

            mockRepoWrapper.Setup(x => x.TaskRepository.GetTask(1)).Returns(Task.FromResult((PTask)null));

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.DeleteTask(1) as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task DeleteTaskOkStatus()
        {
            //Arrange
            PTask task = new PTask { Name = "test" };

            mockRepoWrapper.Setup(x => x.TaskRepository.GetTask(1)).Returns(Task.FromResult(task));
            mockRepoWrapper.Setup(x => x.TaskRepository.Delete(task)).Verifiable();
            mockRepoWrapper.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.DeleteTask(1) as OkResult;

            //Assert
            Assert.Equal(200, action.StatusCode);
            mockRepoWrapper.Verify(x => x.TaskRepository.Delete(task), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskBadRequestStatus()
        {
            //Arrange
            PTask task = new PTask { Name = "test" };

            mockRepoWrapper.Setup(x => x.TaskRepository.GetTask(1)).Returns(Task.FromResult(task));
            mockRepoWrapper.Setup(x => x.TaskRepository.Delete(task)).Verifiable();
            mockRepoWrapper.Setup(x => x.SaveAll()).Returns(Task.FromResult(false));

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.DeleteTask(1) as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
            mockRepoWrapper.Verify(x => x.TaskRepository.Delete(task), Times.Once);
        }

        [Fact]
        public async Task ChangeTaskOwnerNotFountStatus()
        {
            //Arrange
            mockRepoWrapper.Setup(x => x.TaskRepository.GetTask(1)).Returns(Task.FromResult((PTask)null));

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.ChangeTaskOwner(1, 1, "test") as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, action.StatusCode);
            Assert.NotNull(action.Value);
        }


        [Fact]
        public async Task ChangeTaskOwnerWrongOwnerBadRequestStatus()
        {
            //Arrange
            PTask task = new PTask { Name = "test" };
            User user = new User { Nickname = "test", Id = 2 };

            mockRepoWrapper.Setup(x => x.TaskRepository.GetTask(1)).Returns(Task.FromResult(task));
            mockRepoWrapper.Setup(x => x.UserRepository.GetUserByNick("test")).Returns(Task.FromResult(user));

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.ChangeTaskOwner(2, 1, "test") as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
        }

        [Fact]
        public async Task ChangeTaskOwnerCreatedAtRouteStatus()
        {
            //Arrange
            PTask task = new PTask { Name = "test" };
            User user = new User { Nickname = "test", Id = 2 };
            TaskForReturnChangePhotoInfo taskForReturn = new TaskForReturnChangePhotoInfo { TaskOwner = 2 };

            mockRepoWrapper.Setup(x => x.TaskRepository.GetTask(1)).Returns(Task.FromResult(task));
            mockRepoWrapper.Setup(x => x.UserRepository.GetUserByNick("test")).Returns(Task.FromResult(user));
            mockRepoWrapper.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));
            mockMapper.Setup(x => x.Map<TaskForReturnChangePhotoInfo>(task)).Returns(taskForReturn);

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.ChangeTaskOwner(1, 1, "test") as CreatedAtRouteResult;

            //Assert
            Assert.Equal(201, action.StatusCode);
            Assert.Equal("GetTask", action.RouteName);
        }

        [Fact]
        public async Task ChangeTaskOwnerBadRequestStatus()
        {
            //Arrange
            PTask task = new PTask { Name = "test" };
            User user = new User { Nickname = "test", Id = 2 };
            TaskForReturnChangePhotoInfo taskForReturn = new TaskForReturnChangePhotoInfo { TaskOwner = 2 };

            mockRepoWrapper.Setup(x => x.TaskRepository.GetTask(1)).Returns(Task.FromResult(task));
            mockRepoWrapper.Setup(x => x.UserRepository.GetUserByNick("test")).Returns(Task.FromResult(user));
            mockRepoWrapper.Setup(x => x.SaveAll()).Returns(Task.FromResult(false));

            TaskController controller = new TaskController(mockMapper.Object, mockRepoWrapper.Object);

            //Act
            var action = await controller.ChangeTaskOwner(1, 1, "test") as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);
        }
    }

}
