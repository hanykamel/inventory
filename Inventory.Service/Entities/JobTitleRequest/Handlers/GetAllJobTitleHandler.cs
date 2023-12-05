using Inventory.Service.Entities.JobTitleRequest.Commands;
using Inventory.Service.Entities.UnitRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.UnitRequest.Handlers
{
    public class GetAllJobTitleHandler : IRequestHandler<GetAllJobTitleCommand, List<JobTitleOutputCommand>>
    {
        private readonly IJobTitleBussiness _IJobTitleBussiness;
        public GetAllJobTitleHandler(IJobTitleBussiness IJobTitleBussiness)
        {
            _IJobTitleBussiness = IJobTitleBussiness;
        }
        public Task<List<JobTitleOutputCommand>> Handle(GetAllJobTitleCommand request, CancellationToken cancellationToken)
        {
            List<JobTitleOutputCommand> _GetAllJobTitleCommand = new List<JobTitleOutputCommand>();
            var unitList = _IJobTitleBussiness.GetAllJobTitle();
            foreach (var item in unitList)
            {
                JobTitleOutputCommand _entity = new JobTitleOutputCommand();
                _entity.Id = item.Id;
                _entity.IsActive = item.IsActive;
                _entity.JobTitleName = item.Name;
                _GetAllJobTitleCommand.Add(_entity);
            }
            return Task.FromResult(_GetAllJobTitleCommand);
        }
    }
}
