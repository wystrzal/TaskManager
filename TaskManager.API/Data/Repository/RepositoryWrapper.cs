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
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly DataContext dataContext;
        private IUserRepository userRepository;
        private IMessageRepository messageRepository;
        private IProjectRepository projectRepository;
        private ITaskRepository taskRepository;

        public RepositoryWrapper(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }



        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(dataContext);
                }

                return userRepository;
            }
        }

        public IMessageRepository MessageRepository
        {
            get
            {
                if (messageRepository == null)
                {
                    messageRepository = new MessageRepository(dataContext);
                }

                return messageRepository;
            }
        }

        public IProjectRepository ProjectRepository
        {
            get
            {
                if (projectRepository == null)
                {
                    projectRepository = new ProjectRepository(dataContext);
                }

                return projectRepository;
            }
        }

        public ITaskRepository TaskRepository
        {
            get
            {
                if (taskRepository == null)
                {
                    taskRepository = new TaskRepository(dataContext);
                }

                return taskRepository;
            }
        }

        public async Task<bool> SaveAll()
        {
            return await dataContext.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
