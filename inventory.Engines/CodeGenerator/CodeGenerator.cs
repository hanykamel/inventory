using inventory.Engines.CodeGenerator;
using Inventory.CrossCutting.Tenant;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace inventory.Engines.CodeGenerator
{
    public class CodeGenerator : ICodeGenerator
    {
        private readonly ITenantProvider _tenantProvider;

        public CodeGenerator(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }
        public string Generate(int serial)
        {
            int year;
            if (DateTime.Now <= new DateTime(DateTime.Now.Year, 6, 30))
                year = DateTime.Now.Year - 1;
            else
                year = DateTime.Now.Year;

                return
                   year +
                    "/" +
                    serial;
        }

        public string GenerateBarcode(int budgetId,long  baseItemId,int serial)
        {
                return _tenantProvider.GetTenant() +

                "/" +
                budgetId +
                "/" +
                baseItemId +
                "/" +
                serial;
        }
        public string GenerateBarcodeBase64Image(string code)
        {
            var options = new ZXing.QrCode.QrCodeEncodingOptions
            {
                //DisableECI = true,
                CharacterSet = "UTF-8",  
                Width = 200,
                Height = 32,
            };
            var qr = new ZXing.BarcodeWriterPixelData();
            qr.Options = options;
            qr.Format = ZXing.BarcodeFormat.CODE_128;
            var result = (qr.Write(code.Trim()));
            var pixelData = result;
            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            using (var ms = new MemoryStream())
            {
                var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                    pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                // save to stream as PNG
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray()));
            }
        }
    }
}
