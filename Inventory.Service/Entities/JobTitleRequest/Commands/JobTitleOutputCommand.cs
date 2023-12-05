using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.JobTitleRequest.Commands
{
   public class JobTitleOutputCommand
    {
        public int Id { get; set; }
        public string JobTitleName { get; set; }
        public bool? IsActive { get; set; }
    }
}
