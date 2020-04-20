using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Dto
{
    public class UserForRegister
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string RepeatPassword { get; set; }

        public DateTime LastActive { get; set; }

        public UserForRegister()
        {
            LastActive = DateTime.Now;
        }

    }
}
