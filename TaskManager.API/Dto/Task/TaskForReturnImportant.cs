﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Dto.Task
{
    public class TaskForReturnImportant
    {
        public string Name { get; set; }
        public int TimeToEnd { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
    }
}
