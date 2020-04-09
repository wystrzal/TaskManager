using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Model;

namespace TaskManager.API.Data.Repository.ProjectRepo
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext dataContext;

        public ProjectRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<IEnumerable<Project>> GetInvitationsToProject(int userId)
        {
            return await dataContext.UserProjects
                .Where(up => up.UserId == userId && up.Status == "invited").Select(up => up.Project)
                .ToListAsync();
        }

        public async Task<Project> GetProject(int projectId)
        {
            return await dataContext.Projects.Include(p => p.UserProjects).Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Project>> GetProjects(int userId, string type, int skip)
        {
            if (type == "all")
            {
                return await dataContext.UserProjects
                   .Where(up => up.UserId == userId && up.Status == "active").Select(up => up.Project)
                   .Skip(skip).Take(15).ToListAsync();
            }
            else
            {
                return await dataContext.UserProjects
                   .Where(up => up.UserId == userId && up.Project.Type == type && up.Status == "active")
                   .Select(up => up.Project)
                   .Skip(skip).Take(15).ToListAsync();
            }
        }
    }
}
