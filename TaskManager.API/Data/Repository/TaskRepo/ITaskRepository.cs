using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Model;

namespace TaskManager.API.Data.Repository.TaskRepo
{
    public interface ITaskRepository : IMainRepository
    {
        Task<PTask> GetTask(int id);
        Task<IEnumerable<PTask>> GetTasks(int projectId, int skip);

        Task<IEnumerable<PTask>> GetImportantTasks(int userId, int skip);
        
    }
}
