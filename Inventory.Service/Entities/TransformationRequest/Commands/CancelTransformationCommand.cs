using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.TransformationRequest.Commands
{
   public class CancelTransformationCommand : IRequest<bool>
    {
        public Guid TransformationId { get; set; }
    }
}
