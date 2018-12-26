using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPdf;

namespace PdfService
{
    public class IronPdfPackage
    {
        public static void UrlToPdf(string url)
        {
            try
            {
                Console.WriteLine("Creating pdf....");
                var renderer = new HtmlToPdf();
                var pdf = renderer.RenderUrlAsPdf(url);
                pdf.SaveAs(@"C:\Users\hp\Desktop\Desktop\UrlPdf.pdf");
                Console.WriteLine("Pdf created");
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong");
                Console.WriteLine(e.Message);
               
            }

        }

        public static void HtmlToPdf(string html)
        {                       
            try
            {
                Console.WriteLine("Creating pdf....");
                var renderer = new HtmlToPdf();
                var pdf = renderer.RenderHtmlAsPdf(html);
                pdf.SaveAs(@"C:\Users\hp\Desktop\Desktop\HtmlPdf.pdf");
                Console.WriteLine("Pdf created");
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong");
                Console.WriteLine(e.Message);

            }

        }
        public static void HtmlFileToPdf(string htmlFilePath)
        {
            try
            {
                Console.WriteLine("Creating pdf....");
                var renderer = new HtmlToPdf();
                var pdf = renderer.RenderHTMLFileAsPdf(htmlFilePath);
                pdf.SaveAs(@"C:\Users\hp\Desktop\Desktop\HtmlFilePdf.pdf");
                Console.WriteLine("Pdf created");
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong");
                Console.WriteLine(e.Message);

            }

        }
    }
}
