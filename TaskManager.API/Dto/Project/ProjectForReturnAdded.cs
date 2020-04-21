using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Dto.Project
{
    public class ProjectForReturnAdded
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
