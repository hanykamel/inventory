using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Interfaces
{
    public interface ICode
    {
        int Year { get; set; }
        int Serial { get; set; }
        string Code { get; set; }

    }
}
