using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace LablePrint.BarCode.Control
{
    public partial class FrmSetName : Form
    {
        public FrmSetName()
        {
            InitializeComponent();
            int SH = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            int SW = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            this.Location = new Point(SW, SH);
        }
        public FrmSetName(string name)
        {
            InitializeComponent();
            int SH = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            int SW = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            this.Location = new Point(SW, SH);
        }
        public string Name = "";
        private void btn_ok_Click(object sender, EventArgs e)
        {
            
            this.DialogResult = DialogResult.Yes;
        }
    }
}
