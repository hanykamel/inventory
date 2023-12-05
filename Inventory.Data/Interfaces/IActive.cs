using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Interfaces
{
    public interface IActive
    {
        bool? IsActive { get; set; }
    }
}
