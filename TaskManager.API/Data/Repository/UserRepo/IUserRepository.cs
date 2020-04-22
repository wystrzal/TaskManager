using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Model;

namespace TaskManager.API.Data.Repository.UserRepo
{
    public interface IUserRepository
    {
        Task<User> GetUserByNick(string nick);
        Task<User> GetLastUser();
    }
}
