using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Dto.Task
{
    public class TaskForReturnChangePhotoInfo
    {
        public int TaskOwner { get; set; }
        public int TaskOwnerPhoto { get; set; }
        public string TaskOwnerNick { get; set; }
    }
}
