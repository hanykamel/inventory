using AutoMapper;
using Inventory.Data.Models.Inquiry;
using Inventory.Service.Entities.InquiryRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Data.Entities;
using System.Linq;

namespace Inventory.Service.Entities.InquiryRequest.Handlers
{
    public class SaveStagnantHandler : IRequestHandler<SaveStagnantCommand, bool>
    {
        private readonly IStagnantBussiness _iStagnantBussiness;
        private readonly IStoreItemBussiness _iStoreItemBussiness;
        public SaveStagnantHandler(
            IStagnantBussiness iStagnantBussiness,
            IStoreItemBussiness iStoreItemBussiness
            )
        {
            _iStagnantBussiness = iStagnantBussiness;
            _iStoreItemBussiness = iStoreItemBussiness;
        }

        public async Task<bool> Handle(SaveStagnantCommand request, CancellationToken cancellationToken)
        {

            Stagnant stagnantModel = new Stagnant()
            {
                Id = Guid.NewGuid(),
                DateFrom = Convert.ToDateTime(request.DateFrom),
                
            };
            stagnantModel.StagnantStoreItem= new List<StagnantStoreItem>();

            for (int i = 0; i < request.StoreItems.Count; i++)
            {
                stagnantModel.StagnantStoreItem.Add(new StagnantStoreItem()
                {
                    StoreItemId = request.StoreItems[i].Id,
                    Id = Guid.NewGuid()
                });
            }

            _iStoreItemBussiness.UpdateStagnantStoreItemAsync(request.StoreItems.Select(x=>x.Id).ToList());
 
            return await _iStagnantBussiness.saveStagnantStoreItem(stagnantModel);
        
        }
    }
}

