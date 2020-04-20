using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Model;

namespace TaskManager.API.Data.Repository.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext dataContext;

        public UserRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<User> GetUser(int id)
        {
            return await dataContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByNick(string nick)
        {
            return await dataContext.Users.Where(u => u.Nickname == nick).FirstOrDefaultAsync();
        }
  
    }
}
