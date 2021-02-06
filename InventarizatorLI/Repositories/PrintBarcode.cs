using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Printing;
using InventarizatorLI.Model;

namespace InventarizatorLI.Repositories
{
    public class PrintBarcode: PrintDocument
    {
        private PrintDocument printDocument1 = new PrintDocument();

        private Conteiner item;
        private string productName;

        public PrintBarcode()
        {
            PaperSize pS = new PaperSize("Custom Size", 160, 95);
            printDocument1.DefaultPageSettings.PaperSize = pS;
            printDocument1.PrinterSettings.PrinterName = "Xprinter XP-237B";
            printDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pS;
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            IronBarCode.GeneratedBarcode MyBarCode = IronBarCode.BarcodeWriter.CreateBarcode(
                "&" + item.Id + "&",
                IronBarCode.BarcodeWriterEncoding.Code128);

            MyBarCode.ResizeTo(120, 40).SetMargins(10, 0, -5, 0);
            e.Graphics.DrawImage(MyBarCode.ToBitmap(), 10, 0);
            if (productName.Length > 30)
                productName = productName.Insert(31, "\n");
            e.Graphics.DrawString(productName,
                new Font(new FontFamily("Arial"),
                    9,
                    FontStyle.Regular,
                    GraphicsUnit.Pixel),
                new SolidBrush(Color.Black),
                0,
                60);
        }

        public void Print(Conteiner item, string productName)
        {
            this.item = item;
            this.productName = productName;

            printDocument1.Print();
        }

    }
}
