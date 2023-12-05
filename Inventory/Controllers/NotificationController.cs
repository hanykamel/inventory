using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Entities.TransformationRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationBussiness _notificationBussiness;

        public NotificationController(INotificationBussiness notificationBussiness)
        {
            _notificationBussiness = notificationBussiness;
        }
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet]
        public IQueryable<UserNotification> Get() => _notificationBussiness.GetAll();

      
        [HttpPost("[action]")]
        public IActionResult MarkAsRead([FromQuery]Guid id) => Ok(new { res = _notificationBussiness.MarkAsRead(id) });
    }
}