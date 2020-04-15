using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Model
{
    public class PTask
    {
        public int PTaskId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int Owner { get; set; }
        public DateTime TimeToEnd { get; set; }
        public Project Project { get; set; }
    }
}
