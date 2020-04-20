using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Dto.Project
{
    public class ProjectForReturn
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Owner { get; set; }
        public bool AnyUsers { get; set; }
        public int[] ProjectUsersId { get; set; }
        public string[] ProjectUsersNick { get; set; }

    }
}
