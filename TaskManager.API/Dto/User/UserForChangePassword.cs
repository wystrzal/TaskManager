using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Dto.User
{
    public class UserForChangePassword
    {
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
    }
}
