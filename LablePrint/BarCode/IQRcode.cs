using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LablePrint.BarCode
{
    interface IQRcode
    {
        void CreateQRcode(Graphics graphics);
        Rectangle BarcodeRect { get; set; }
        bool ShowText { get; set; }
        bool ShowTextOnTop { get; set; }
        QRcodeTextAlign barcodeTextAlign { get; set; }
        int BarcodeHeight { get; set; }
        string BarcodeValue { get; set; }
        QRcodeRotation barcodeRotation { get; set; }
        Font ValueTextFont { get; set; }
        QRCodeWeight BarcodeWeight { get; set; }
        Rectangle GetBarcodeRectangle();
    }
}
