using Inventory.Service.Entities.UnitRequest.Commands;
using MediatR;
using System.Collections.Generic;

namespace Inventory.Service.Entities.JobTitleRequest.Commands
{
  public  class GetAllJobTitleCommand : IRequest<List<JobTitleOutputCommand>>
    {
    }
}
