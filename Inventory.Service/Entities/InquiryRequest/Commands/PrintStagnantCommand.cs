using Inventory.Data.Models.Inquiry;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Inventory.Service.Entities.InquiryRequest.Commands
{
    public class PrintStagnantCommand: IRequest<MemoryStream>
    {
        public String DateFrom { get; set; }
        public List<StagnantBaseItemVM> StoreItems { get; set; }
    }


}
