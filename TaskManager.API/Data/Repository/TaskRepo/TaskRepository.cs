using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Model;

namespace TaskManager.API.Data.Repository.TaskRepo
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DataContext dataContext;

        public TaskRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<PTask> GetTask(int id)
        {
            return await dataContext.PTasks.Where(t => t.PTaskId == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PTask>> GetTasks(int projectId)
        {
            return await dataContext.PTasks.Where(t => t.Project.ProjectId == projectId).ToListAsync();
        }
    }
}
