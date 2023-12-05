using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.JobTitleRequest.Commands
{
   public class ViewJobTitleCommand : IRequest<JobTitleOutputCommand>
    {
        public int Id { get; set; }
    }
}
