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
        public async Task<IActionResult> GetTasks(int userId, int projectId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var tasks = await taskRepository.GetTasks(projectId);

            var tasksForReturn = mapper.Map<IEnumerable<PTask>>(tasks);

            return Ok(tasksForReturn);
        }

        [HttpGet("{taskId}", Name = "GetTask")]
        public async Task<IActionResult> GetTask(int userId, int taskId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var task = await taskRepository.GetTask(taskId);

            if (task == null)
                return NotFound("Could not find task");

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
                return NotFound("Could not find project");

            var taskToAdd = mapper.Map<PTask>(taskForAdd);

            project.PTasks.Add(taskToAdd);

            if (await mainRepository.SaveAll())
            {
                var taskForReturn = mapper.Map<TaskForReturn>(taskToAdd);
                return CreatedAtRoute("GetTask", new { userId, projectId, taskId = taskToAdd.PTaskId }, taskForReturn);
            }

            return BadRequest("Could not add task");
        }


    }
}