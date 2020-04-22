using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Model;

namespace TaskManager.API.Data.Repository.UserRepo
{
    public class UserRepository : MainRepository, IUserRepository
    {
        private readonly DataContext dataContext;

        public UserRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<User> GetLastUser()
        {
            return await dataContext.Users.OrderByDescending(u => u.Id).FirstAsync();
        }

        public async Task<User> GetUserByNick(string nick)
        {
            return await dataContext.Users.Where(u => u.Nickname == nick).FirstOrDefaultAsync();
        }

  
    }
}
