using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Data.Repository.MessageRepo;
using TaskManager.API.Data.Repository.ProjectRepo;
using TaskManager.API.Data.Repository.TaskRepo;
using TaskManager.API.Data.Repository.UserRepo;

namespace TaskManager.API.Data.Repository
{
    public interface IRepositoryWrapper
    {
        IUserRepository UserRepository { get; }
        IMessageRepository MessageRepository { get; }
        IProjectRepository ProjectRepository { get; }
        ITaskRepository TaskRepository { get; }
        Task<bool> SaveAll();
    }
}
