using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Inventory.Service.Entities.ExaminationCommittee.Commands
{
    public class PrintExaminationCommand:IRequest<MemoryStream>
    {
        public Guid ExaminationId { get; set; }
    }
}
