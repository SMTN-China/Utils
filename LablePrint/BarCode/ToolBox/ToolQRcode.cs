using LablePrint.ToolBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LablePrint.BarCode.ToolBox
{
    [ToolAttribute("二维码", "Resources.QRCode.png", Order = 8)]
    public class ToolQRcode : ToolBase
    {
        public ToolQRcode()
        {
            ToolCursor = Cursors.Cross;
        }
        public override void OnMouseDown(Designer designer, MouseEventArgs e)
        {
            AddNewObject(designer, new DrawBarcodes(e.X, e.Y));
        }

        public override void OnMouseMove(Designer designer, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //designer.Items[0].Move(e.X,e.Y);
                designer.Refresh();
                //designer.SelectedItem(designer.Items[0]);
            }
        }
    }
}
