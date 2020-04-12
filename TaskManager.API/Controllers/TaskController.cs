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

        public TaskController(IMainRepository mainRepository, IProjectRepository projectRepository, IMapper mapper)
        {
            this.mainRepository = mainRepository;
            this.projectRepository = projectRepository;
            this.mapper = mapper;
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
                return Ok();

            return BadRequest("Could not add task");
        }
    }
}