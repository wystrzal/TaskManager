﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Dto.Task
{
    public class TaskForAdd
    {
        public string Name { get; set; }
        public string Priority { get; set; }
        public DateTime TimeToEnd { get; set; }
    }
}
