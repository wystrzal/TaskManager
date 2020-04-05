using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Model
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Owner { get; set; }

        public ICollection<UserProject> UserProjects { get; set; }
        public ICollection<PTask> PTasks { get; set; }
    }
}
