using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.BaseItemRequest.Commands
{
   public class AddBaseItemCommand : IRequest<long>
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public bool Consumed { get; set; }
        public int DefaultUnitId { get; set; }
        public int? WarningLevel { get; set; }
        public int ItemCategoryId { get; set; }
    }
}
