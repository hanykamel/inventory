using Inventory.Service.Entities.SupplierRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.SupplierRequest.Handlers
{
  public  class GetAllSupplierHandler: IRequestHandler<GetAllSupplierCommand, List<SupplierOutputCommand>>
    {


        private readonly ISupplierBussiness _ISupplierBussiness;
    public GetAllSupplierHandler(ISupplierBussiness ISupplierBussiness)
    {
        _ISupplierBussiness = ISupplierBussiness;
    }


    public Task<List<SupplierOutputCommand>> Handle(GetAllSupplierCommand request, CancellationToken cancellationToken)
    {
        List<SupplierOutputCommand> _SupplierOutputCommand = new List<SupplierOutputCommand>();
        var SupplierList = _ISupplierBussiness.GetAllSupplier();
        foreach (var item in SupplierList)
        {
            SupplierOutputCommand _entity = new SupplierOutputCommand();

            _entity.Name = item.Name;

            _SupplierOutputCommand.Add(_entity);
        }
        return Task.FromResult(_SupplierOutputCommand);
    }
}
}

