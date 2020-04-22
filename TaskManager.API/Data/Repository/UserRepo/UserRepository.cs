using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Model;
using TaskManager.API_Test;

namespace TaskManager.API.Data.Repository.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext dataContext;
        private TestDataContext context;

        public UserRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public UserRepository(TestDataContext context)
        {
            this.context = context;
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
