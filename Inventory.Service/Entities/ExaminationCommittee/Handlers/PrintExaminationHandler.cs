using inventory.Engines.WordGenerator;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Service.Entities.ExaminationCommittee.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.ExaminationCommittee.Handlers
{
    class PrintExaminationHandler : IRequestHandler<PrintExaminationCommand, MemoryStream>
    {
        private readonly IExaminationBusiness _iExaminationBussiness;
        private readonly IWordBusiness _wordBusiness;

        public PrintExaminationHandler(IExaminationBusiness iExaminationBussiness ,IWordBusiness wordBusiness)
        {
            _iExaminationBussiness = iExaminationBussiness;
            _wordBusiness = wordBusiness;
        }
        public Task<MemoryStream> Handle(PrintExaminationCommand request, CancellationToken cancellationToken)
        {
            ExaminationCommitte examinationCommitte = _iExaminationBussiness.GetExaminationById(request.ExaminationId);
            if (examinationCommitte != null)
            {
                return Task.FromResult(_wordBusiness.printExaminationDocument(examinationCommitte));
            }
            return Task.FromResult(new MemoryStream());
        }
    }
}
