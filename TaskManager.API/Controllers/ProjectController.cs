using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.API.Data.Repository;
using TaskManager.API.Data.Repository.ProjectRepo;
using TaskManager.API.Dto.Project;
using TaskManager.API.Model;

namespace TaskManager.API.Controllers
{
    [Route("api/user/{userId}/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IMainRepository mainRepository;
        private readonly IProjectRepository projectRepository;

        public ProjectController(IMapper mapper, IMainRepository mainRepository, IProjectRepository projectRepository)
        {
            this.mapper = mapper;
            this.mainRepository = mainRepository;
            this.projectRepository = projectRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectForAddDto projectForAddDto, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var projectToAdd = mapper.Map<Project>(projectForAddDto);

            projectToAdd.Owner = userId;

            mainRepository.Add(projectToAdd);

            if (await mainRepository.SaveAll())
            {
                var userProject = new UserProject
                {
                    ProjectId = projectToAdd.ProjectId,
                    UserId = userId,
                    Status = "active"
                };

                mainRepository.Add(userProject);

                if (await mainRepository.SaveAll())
                {
                    var projectForReturn = mapper.Map<ProjectForReturn>(projectToAdd);
                    return CreatedAtRoute("GetProject", new { userId = userId, projectId = projectToAdd.ProjectId }, projectForReturn);
                }
            }
 
            
            return BadRequest("Could not add the project.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProject(int userId ,int projectId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var project = await projectRepository.GetProject(projectId);

            if (project == null)
                return NotFound("Could not found the project.");

            mainRepository.Delete(project);

            if (await mainRepository.SaveAll())

                return Ok();

            return BadRequest("Could not delete the project.");
        }

        [HttpGet("{projectId}", Name = "GetProject")]
        public async Task<IActionResult> GetProject(int userId, int projectId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var project = await projectRepository.GetProject(projectId);

            if (project == null)
                return NotFound("Could not found the project");

            var projectForReturn = mapper.Map<ProjectForReturn>(project);

            return Ok(projectForReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects(int userId, [FromQuery]string type, [FromQuery]int skip)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var projects = await projectRepository.GetProjects(userId, type, skip);

            var projectsForReturn = mapper.Map<IEnumerable<ProjectForReturn>>(projects);

            return Ok(projectsForReturn);
        }

        [HttpPost("{projectId}/new/{newUser}")]
        public async Task<IActionResult> AddToProject(int userId, int projectId, int newUser)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var project = await projectRepository.GetProject(projectId);

            if (project.Owner != userId)
                return Unauthorized();

            if (project == null)
                return NotFound("Could not found the project.");

            var userProject = new UserProject
            {
                ProjectId = project.ProjectId,
                UserId = newUser,
                Status = "inactive"
            };

            mainRepository.Add(userProject);

            if (await mainRepository.SaveAll())
                return Ok();

            return BadRequest("This user already belong to project.");
        }
    }
}
