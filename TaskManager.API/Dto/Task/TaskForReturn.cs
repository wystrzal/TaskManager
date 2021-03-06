﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Dto.Task
{
    public class TaskForReturn
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int TimeToEnd { get; set; }
        public int TaskOwner { get; set; }
        public int TaskOwnerPhoto { get; set; }
        public string TaskOwnerNick { get; set; }
    }
}
