using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Data.Repository;
using TaskManager.API.Data.Repository.ProjectRepo;
using TaskManager.API.Data.Repository.TaskRepo;
using TaskManager.API.Dto.Task;
using TaskManager.API.Model;

namespace TaskManager.API.Controllers
{
    [Route("api/user/{userId}/project/{projectId}/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IMainRepository mainRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IMapper mapper;
        private readonly ITaskRepository taskRepository;

        public TaskController(IMainRepository mainRepository, IProjectRepository projectRepository, IMapper mapper, 
            ITaskRepository taskRepository)
        {
            this.mainRepository = mainRepository;
            this.projectRepository = projectRepository;
            this.mapper = mapper;
            this.taskRepository = taskRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(int userId, int projectId, [FromQuery]int skip)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var tasks = await taskRepository.GetTasks(projectId, skip);

            var tasksForReturn = mapper.Map<IEnumerable<TaskForReturn>>(tasks);

            return Ok(tasksForReturn);
        }

        [HttpGet("important")]
        public async Task<IActionResult> GetImportantTasks(int userId, [FromQuery]int skip)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var importantTasks = await taskRepository.GetImportantTasks(userId, skip);

            var importantTasksForReturn = mapper.Map<IEnumerable<TaskForReturnImportant>>(importantTasks);

            return Ok(importantTasksForReturn);
        }

        [HttpGet("{taskId}", Name = "GetTask")]
        public async Task<IActionResult> GetTask(int userId, int taskId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var task = await taskRepository.GetTask(taskId);

            if (task == null)
                return NotFound("Could not find task.");

            var taskForReturn = mapper.Map<TaskForReturn>(task);

            return Ok(taskForReturn);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(int userId, int projectId, TaskForAdd taskForAdd)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var project = await projectRepository.GetProject(projectId);

            if (project == null)
                return NotFound("Could not find project.");

            var taskToAdd = mapper.Map<PTask>(taskForAdd);

            taskToAdd.Status = "To Do";

            project.PTasks.Add(taskToAdd);

            if (await mainRepository.SaveAll())
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
                return Unauthorized();

            var task = await taskRepository.GetTask(taskId);

            if (task == null)
                return NotFound("Could not find task.");

            if (action == "status")
                task.Status = newStatPrior;
            else
                task.Priority = newStatPrior;

            if (await mainRepository.SaveAll())
                return Ok();

            return BadRequest("Task failed.");
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int userId, int taskId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var task = await taskRepository.GetTask(taskId);

            if (task == null)
                return NotFound("Could not find task.");

            mainRepository.Delete(task);

            if (await mainRepository.SaveAll())
                return Ok();

            return BadRequest("Could not delete task.");
        }
    }
}