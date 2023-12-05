using Inventory.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ItemCategoryRequest.Commands
{
   public class AddItemCategoryCommand : IRequest<ItemCategory>
    {
        public string name { get; set; }
    }
}
