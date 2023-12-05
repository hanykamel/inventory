using AutoMapper;
using Inventory.Data.Entities;
using Inventory.Service.Entities.DelegationRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.DelegationRequest.Handlers
{
  public  class EditDelegationHandler : IRequestHandler<EditDelegationCommand, Guid>
    {

        private readonly IDelegationBussiness _iDelegationBussiness;
        private readonly IMapper _mapper;
        public EditDelegationHandler(IDelegationBussiness iDelegationBussiness, IMapper mapper)
        {
            _iDelegationBussiness = iDelegationBussiness;
            _mapper = mapper;
        }

     

        public async Task<Guid> Handle(EditDelegationCommand request, CancellationToken cancellationToken)
        {
            var delegation = _mapper.Map<Delegation>(request);
            foreach (var item in request.DelegationStores)
            {
                var delegationstore = new DelegationStore();
                delegationstore.StoreId = item;
                delegationstore.Id = delegation.Id;
                delegation.DelegationStore.Add(delegationstore);
            }
            var delegationReturn = await _iDelegationBussiness.SaveDelegation(delegation);
            var result = delegationReturn.Id;

            return result;
        }
    }
}

