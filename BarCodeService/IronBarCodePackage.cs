using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronBarCode;
namespace BarCodeService
{
    public class IronBarCodePackage
    {
        public static void CreateBarCode(string data)
        {
            try
            {
                BarcodeWriter.CreateBarcode(data, BarcodeWriterEncoding.QRCode).SaveAsJpeg(@"C: \Users\hp\Desktop\Desktop\test.jpg");

            }
            catch (Exception e)
            {

                throw new Exception("from CreateBarcode : ", e);
            }
        }


        public static string ReadBarCode(string path)
        {
            try
            {
                var barcodeResult = BarcodeReader.QuicklyReadOneBarcode(path);

                return barcodeResult.Text;
            }
            catch (Exception e)
            {
                return null;
            }
            
        }
    }
}
