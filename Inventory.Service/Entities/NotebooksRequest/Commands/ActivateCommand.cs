using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.NotebooksRequest.Commands
{
  public  class ActivateCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }
}
