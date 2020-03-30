using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TaskManager.API.Model
{
    public class User : IdentityUser<int>
    {
        public string Nickname { get; set; }
        public DateTime LastActive { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }
        public ICollection<Message> MessagesSended { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
    }
}
