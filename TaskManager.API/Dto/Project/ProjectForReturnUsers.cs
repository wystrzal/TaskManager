using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Dto.Project
{
    public class ProjectForReturnUsers
    {
        public int UserId { get; set; }
        public string Nickname { get; set; }
        public int PhotoId { get; set; }
    }
}
