﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.API.Data.Repository;
using TaskManager.API.Data.Repository.ProjectRepo;
using TaskManager.API.Data.Repository.UserRepo;
using TaskManager.API.Dto.Project;
using TaskManager.API.Helpers.Filters;
using TaskManager.API.Model;

namespace TaskManager.API.Controllers
{
    [UserAuthorizationFilter]
    [Route("api/user/{userId}/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRepositoryWrapper repositoryWrapper;

        public ProjectController(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            this.mapper = mapper;
            this.repositoryWrapper = repositoryWrapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectForAdd projectForAddDto, int userId)
        {
            var projectForAdd = mapper.Map<Project>(projectForAddDto);

            projectForAdd.Owner = userId;

            repositoryWrapper.ProjectRepository.Add(projectForAdd);

            if (await repositoryWrapper.SaveAll())
            {
                var userProject = new UserProject
                {
                    ProjectId = projectForAdd.ProjectId,
                    UserId = userId,
                    Status = "active"
                };

                repositoryWrapper.ProjectRepository.Add(userProject);

                if (await repositoryWrapper.SaveAll())
                {
                    var projectForReturn = mapper.Map<ProjectForReturnAdded>(projectForAdd);
                    return CreatedAtRoute("GetProject", new { userId, projectForAdd.ProjectId }, projectForReturn);
                }
            }

            return BadRequest("Could not add project.");
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(int userId, int projectId)
        {
            var project = await repositoryWrapper.ProjectRepository.GetProject(projectId);

            if (project == null)
            {
                return NotFound("Could not find project.");
            }

            if (project.Owner != userId)
            {
                return Unauthorized();
            }

            repositoryWrapper.ProjectRepository.Delete(project);

            if (await repositoryWrapper.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Could not delete project.");
        }

        [HttpGet("{projectId}", Name = "GetProject")]
        public async Task<IActionResult> GetProject(int userId, int projectId)
        {
            var project = await repositoryWrapper.ProjectRepository.GetProject(projectId);

            if (project == null)
            {
                return NotFound("Could not find project");
            }

            var projectForReturn = mapper.Map<ProjectForReturn>(project);

            return Ok(projectForReturn);
        }

        [HttpGet("{projectId}/users")]
        public async Task<IActionResult> GetProjectUsers(int userId, int projectId)
        {
            var users = await repositoryWrapper.UserRepository.GetProjectUsers(projectId);

            var usersToReturn = mapper.Map<IEnumerable<ProjectForReturnUsers>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects(int userId, [FromQuery]string type, [FromQuery]int skip)
        {
            var projects = await repositoryWrapper.ProjectRepository.GetProjects(userId, type, skip);

            var projectsForReturn = mapper.Map<IEnumerable<ProjectForReturn>>(projects);

            return Ok(projectsForReturn);
        }

        [HttpGet("invitations")]
        public async Task<IActionResult> GetInvitations(int userId)
        {
            var projects = await repositoryWrapper.ProjectRepository.GetInvitationsToProject(userId);

            var projectsForReturn = mapper.Map<IEnumerable<ProjectForReturnInvitations>>(projects);

            return Ok(projectsForReturn);
        }

        [HttpPost("{projectId}/new/{userNick}")]
        public async Task<IActionResult> AddToProject(int userId, int projectId, string userNick)
        {
            var newUser = await repositoryWrapper.UserRepository.GetUserByNick(userNick.ToLower());

            if (newUser == null)
            {
                return NotFound("Could not find user.");
            }
  
            var project = await repositoryWrapper.ProjectRepository.GetProject(projectId);

            if (project == null)
            {
                return NotFound("Could not find project.");
            }

            if (project.Owner != userId)
            {
                return Unauthorized();
            }

            var checkStatus = project.UserProjects
                .Where(up => up.UserId == newUser.Id).Select(up => up.Status).FirstOrDefault();

            if (checkStatus == "invited" || checkStatus == "rejected")
            {
                return BadRequest("This user is currently invited, or your invite was rejected.");
            } 
            else if (checkStatus == "active")
            {
                return BadRequest("This user already belong to project.");
            }    
            else
            {
                var userProject = new UserProject
                {
                    ProjectId = project.ProjectId,
                    UserId = newUser.Id,
                    Status = "invited"
                };

                repositoryWrapper.ProjectRepository.Add(userProject);

                if (await repositoryWrapper.SaveAll())
                {
                    return Ok();
                }
            
                return BadRequest("This user already belong to project.");
            }
        }

        [HttpDelete("{projectId}/delete/{userToDelete}")]
        public async Task<IActionResult> DeleteFromProject(int userId, int projectId, int userToDelete)
        {
            var project = await repositoryWrapper.ProjectRepository.GetProject(projectId);

            if (project == null)
            {
                return NotFound("Could not find project.");
            }

            var user = project.UserProjects.Where(up => up.UserId == userToDelete).FirstOrDefault();

            if (user == null)
            {
                return NotFound("This user does not belong to project.");
            }

            repositoryWrapper.ProjectRepository.Delete(user);

            if (await repositoryWrapper.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Could not delete user from project.");
        }

        [HttpPost("join/{projectId}")]
        public async Task<IActionResult> JoinToProject(int userId, int projectId, [FromQuery]int action)
        {
            var project = await repositoryWrapper.ProjectRepository.GetProject(projectId);

            if (project == null)
            {
                return NotFound("Could not find project.");
            }

            var joinUser = project.UserProjects.Where(up => up.UserId == userId && up.ProjectId == projectId).FirstOrDefault();

            // 1 = accept
            if (action == 1)
            {
                joinUser.Status = "active";
            }
            else
            {
                joinUser.Status = "rejected";
            }

            if (await repositoryWrapper.SaveAll())
            {
                if (action == 1)
                {
                    var projectForReturn = mapper.Map<ProjectForReturn>(project);
                    return Ok(projectForReturn);
                }
                else
                {
                    return Ok();
                }
            }

            return BadRequest("Task failed.");
        }      

        [HttpPost("leave/{projectId}")]
        public async Task<IActionResult> LeaveProject(int userId, int projectId)
        {
            var project = await repositoryWrapper.ProjectRepository.GetProject(projectId);

            if (project == null)
            {
                return NotFound("Could not find project.");
            }   

            var userLeave = project.UserProjects.Where(up => up.UserId == userId && up.ProjectId == projectId).FirstOrDefault();

            userLeave.Status = "inactive";

            if (await repositoryWrapper.SaveAll())
            {
                return Ok();
            }        

            return BadRequest("Could not leave project.");
        }

        [HttpPut("change/{projectId}")]
        public async Task<IActionResult> ChangeProjectName(int userId, int projectId, ProjectForChangeName projectForChangeName)
        {
            var project = await repositoryWrapper.ProjectRepository.GetProject(projectId);

            if (project == null)
            {
                return NotFound("Could not find project.");
            }

            mapper.Map(projectForChangeName, project);

            if (await repositoryWrapper.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Could not change project name.");
        }
    }
}
