using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ItemCategoryRequest.Commands
{
   public class GetAllItemCategoryCommand : IRequest<List<ItemCategoryOutPutCommand>>
    {
    }
}
