using Inventory.Service.Entities.JobTitleRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.JobTitleRequest.Handlers
{
    public class ViewJobTitleHandler : IRequestHandler<ViewJobTitleCommand, JobTitleOutputCommand>
    {

        private readonly IJobTitleBussiness _IJobTitleBussiness;
        public ViewJobTitleHandler(IJobTitleBussiness IJobTitleBussiness)
        {
            _IJobTitleBussiness = IJobTitleBussiness;
        }
        public Task<JobTitleOutputCommand> Handle(ViewJobTitleCommand request, CancellationToken cancellationToken)
        {
        
            var JobTitleentity= _IJobTitleBussiness.ViewJobTitle(request.Id);
          
               JobTitleOutputCommand _entity = new JobTitleOutputCommand();
            _entity.Id = JobTitleentity.Id;
            _entity.JobTitleName = JobTitleentity.Name;
            _entity.IsActive = JobTitleentity.IsActive;
            return Task.FromResult(_entity);

        }
    }
}
