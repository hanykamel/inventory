using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ItemCategoryRequest.Commands
{
  public  class ViewItemCategoryCommand : IRequest<ItemCategoryOutPutCommand>
    {
        public int Id { get; set; }
    }
}
