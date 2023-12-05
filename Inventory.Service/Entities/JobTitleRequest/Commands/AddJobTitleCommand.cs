using Inventory.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.JobTitleRequest.Commands
{
  public  class AddJobTitleCommand : IRequest<JobTitle>
    {
        public string Name { get; set; }
    }
}
