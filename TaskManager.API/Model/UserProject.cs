﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Model
{
    public class UserProject
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
