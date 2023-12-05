using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.BookRequest.Commands
{
    public class ViewBookInputCommand : IRequest<ViewBookOutputCommand>
    {
        public int Id { get; set; }
    }
}
