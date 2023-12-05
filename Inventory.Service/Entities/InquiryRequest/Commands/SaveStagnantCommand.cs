using Inventory.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Service.Entities.InquiryRequest.Commands
{
    public class SaveStagnantCommand: IRequest<bool>
    {
        [Required]
        public String DateFrom { get; set; }
        public List<StoreItem> StoreItems { get; set; }
    }
    public class StagnantModel
    {
        public Guid Id { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }
        public String UnitName { get; set; }
        public String StoreItemStatus { get; set; }
        public DateTime CreationDate { get; set; }
    }

}
