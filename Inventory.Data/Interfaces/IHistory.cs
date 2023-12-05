using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Interfaces
{
    public interface IHistory<T>
    {

        string AuditAction { get; set; }
        T HistoryId { get; set; }
        DateTime? AuditDate { get; set; }
    }
}
