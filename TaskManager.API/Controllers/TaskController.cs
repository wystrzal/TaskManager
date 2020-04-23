using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Data.Repository;
using TaskManager.API.Dto.Task;
using TaskManager.API.Model;

namespace TaskManager.API.Controllers
{
    [Route("api/user/{userId}/project/{projectId}/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRepositoryWrapper repositoryWrapper;

        public TaskController(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            this.mapper = mapper;
            this.repositoryWrapper = repositoryWrapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(int userId, int projectId, [FromQuery]int skip)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var tasks = await repositoryWrapper.TaskRepository.GetTasks(projectId, skip);

            var tasksForReturn = mapper.Map<IEnumerable<TaskForReturn>>(tasks);

            return Ok(tasksForReturn);
        }

        [HttpGet("important")]
        public async Task<IActionResult> GetImportantTasks(int userId, [FromQuery]int skip)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var importantTasks = await repositoryWrapper.TaskRepository.GetImportantTasks(userId, skip);

            var importantTasksForReturn = mapper.Map<IEnumerable<TaskForReturnImportant>>(importantTasks);

            return Ok(importantTasksForReturn);
        }

        [HttpGet("{taskId}", Name = "GetTask")]
        public async Task<IActionResult> GetTask(int userId, int taskId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var task = await repositoryWrapper.TaskRepository.GetTask(taskId);

            if (task == null)
            {
                return NotFound("Could not find task.");
            }

            var taskForReturn = mapper.Map<TaskForReturn>(task);

            return Ok(taskForReturn);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(int userId, int projectId, TaskForAdd taskForAdd)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var project = await repositoryWrapper.ProjectRepository.GetProject(projectId);

            if (project == null)
            {
                return NotFound("Could not find project.");
            }

            var taskToAdd = mapper.Map<PTask>(taskForAdd);

            taskToAdd.Status = "To Do";
            taskToAdd.Owner = userId;

            project.PTasks.Add(taskToAdd);

            if (await repositoryWrapper.SaveAll())
            {
                var taskForReturn = mapper.Map<TaskForReturn>(taskToAdd);
                return CreatedAtRoute("GetTask", new { userId, projectId, taskId = taskToAdd.PTaskId }, taskForReturn);
            }

            return BadRequest("Could not add task.");
        }

        [HttpPut("change/{taskId}")]
        public async Task<IActionResult> ChangeStatusPriority(int userId, int taskId, [FromQuery]string action,
            [FromQuery]string newStatPrior)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var task = await repositoryWrapper.TaskRepository.GetTask(taskId);

            if (task == null)
            {
                return NotFound("Could not find task.");
            }

            if (action == "status")
            {
                task.Status = newStatPrior;
            }
            else
            {
                task.Priority = newStatPrior;
            }

            if (await repositoryWrapper.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Task failed.");
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int userId, int taskId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var task = await repositoryWrapper.TaskRepository.GetTask(taskId);

            if (task == null)
            {
                return NotFound("Could not find task.");
            }

            repositoryWrapper.TaskRepository.Delete(task);

            if (await repositoryWrapper.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Could not delete task.");
        }

        [HttpPut("{taskId}/changeOwner/{newOwner}")]
        public async Task<IActionResult> ChangeTaskOwner(int userId, int taskId, string newOwner)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var task = await repositoryWrapper.TaskRepository.GetTask(taskId);

            if (task == null)
            {
                return NotFound("Could not find task.");
            }

            var user = await repositoryWrapper.UserRepository.GetUserByNick(newOwner);

            if (task.Owner == user.Id)
            {
                return BadRequest("Selected user is currently owner of this task.");
            }

            task.Owner = user.Id;

            if (await repositoryWrapper.SaveAll())
            {
                var taskForReturn = mapper.Map<TaskForReturnChangePhotoInfo>(task);
                return CreatedAtRoute("GetTask", new { userId, projectId = 0, taskId}, taskForReturn);
            }

            return BadRequest("Could not change task owner");
        }
    }
}