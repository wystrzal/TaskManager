﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Model;

namespace TaskManager.API.Data.Repository.ProjectRepo
{
    public interface IProjectRepository : IMainRepository
    {
        Task<IEnumerable<Project>> GetProjects(int userId, string type, int skip);
        Task<IEnumerable<Project>> GetInvitationsToProject(int userId);
        Task<Project> GetProject(int projectId);
    }
}
