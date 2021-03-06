﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Data.Repository.UserRepo;
using TaskManager.API.Model;

namespace TaskManager.API.Data.Repository.TaskRepo
{
    public class TaskRepository : MainRepository, ITaskRepository
    {
        private readonly DataContext dataContext;

        public TaskRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<PTask> GetTask(int id)
        {
            return await dataContext.PTasks.Include(t => t.Project)
                .ThenInclude(p => p.UserProjects).ThenInclude(up => up.User)
                .Where(t => t.PTaskId == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PTask>> GetTasks(int projectId, int skip)
        {
            return await dataContext.PTasks.Include(t => t.Project)
                .ThenInclude(p => p.UserProjects).ThenInclude(up => up.User)
                .Where(t => t.Project.ProjectId == projectId && t.TimeToEnd > DateTime.Today)
                .OrderBy(t => t.TimeToEnd).Skip(skip).Take(15).ToListAsync();
        }
        public async Task<IEnumerable<PTask>> GetImportantTasks(int userId, int skip)
        {
            var importantTasks = await dataContext.PTasks.Include(t => t.Project)
                .Where(t => t.Owner == userId || t.Project.Owner == userId).Skip(skip).Take(15).ToListAsync();

            return importantTasks.Where(t => t.Priority == "High" && t.TimeToEnd > DateTime.Today
                    && t.Status != "Done").OrderBy(t => t.TimeToEnd);

                
        }
    }
}
