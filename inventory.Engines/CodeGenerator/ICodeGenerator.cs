
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory.Engines.CodeGenerator
{
    public interface ICodeGenerator
    {
        string Generate( int serial);
        string GenerateBarcode(int budgetId, long baseItemId, int serial);
        string GenerateBarcodeBase64Image(string code);

    }
}
