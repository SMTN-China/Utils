using LablePrint.App_Start;
using Microsoft.Owin.Hosting;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LablePrint
{
    public partial class Main : Form
    {
        public static BarTender.Application btApp = new BarTender.Application();
        public static BarTender.Format btFormat;

        public Main()
        {
            InitializeComponent();

        }



        private void BtnAutoStart_Click(object sender, EventArgs e)
        {
            // LablePrint.exe
            RunWhenStart(true, Application.ProductName, Application.StartupPath + @"\LablePrint.exe");
        }

        public static void RunWhenStart(bool started, string name, string path)
        {
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey Run = HKLM.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            if (started)
            {
                try
                {
                    Run.SetValue(name, path);
                    HKLM.Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "LablePrint", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    Run.DeleteValue(name);
                    HKLM.Close();
                }
                catch { }
            }
        }

        private void btnNoAutoStart_Click(object sender, EventArgs e)
        {
            RunWhenStart(false, Application.ProductName, Application.StartupPath + "\\LablePrint.exe");
        }

        private void 最小化到托盘ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (最小化到托盘ToolStripMenuItem.Text == "还原")
            {
                this.Show();
                this.ShowInTaskbar = true;
                contextMenuStrip1.Items[0].Text = "最小化到托盘";
            }
            else
            {
                this.ShowInTaskbar = false;
                this.Hide();
                contextMenuStrip1.Items[0].Text = "还原";
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // btFormat.Close();
            Process.GetProcessById(btApp.ProcessId).Kill();
            Process.GetCurrentProcess().Kill();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Hide();
            contextMenuStrip1.Items[0].Text = "还原";
            e.Cancel = true;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            var webApp = WebApp.Start<Startup>(url: "http://*:" + ConfigurationManager.AppSettings["Port"] + "/");
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (最小化到托盘ToolStripMenuItem.Text == "还原")
            {
                this.Show();
                this.ShowInTaskbar = true;
                contextMenuStrip1.Items[0].Text = "最小化到托盘";
            }
            else
            {
                this.ShowInTaskbar = false;
                this.Hide();
                contextMenuStrip1.Items[0].Text = "还原";
            }
        }
        //public static bool PrintLbl(Entity_Barcode entity_Barcode)
        //{
        //    try
        //    {

        //        string Path = System.Environment.CurrentDirectory + "\\label.btw";
        //        btFormat = btApp.Formats.Open(@Path, true, "");
        //        btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
        //        btApp.Visible = false;
        //        btFormat.PrintSetup.NumberSerializedLabels = 1;
        //        btFormat.SetNamedSubStringValue("ci", entity_Barcode.ci);
        //        btFormat.SetNamedSubStringValue("gaodu", entity_Barcode.gaodu);
        //        btFormat.SetNamedSubStringValue("partno", entity_Barcode.partno);
        //        btFormat.SetNamedSubStringValue("PL", entity_Barcode.PL);
        //        btFormat.SetNamedSubStringValue("qingdian", entity_Barcode.qingdian);
        //        btFormat.SetNamedSubStringValue("qty", entity_Barcode.qty);
        //        btFormat.SetNamedSubStringValue("reelid", entity_Barcode.reelid);
        //        btFormat.SetNamedSubStringValue("size", entity_Barcode.size);
        //        btFormat.SetNamedSubStringValue("user", entity_Barcode.userid);

        //        //btFormat.SetNamedSubStringValue("DESC", entity_Barcode.Desc);
        //        //btFormat.SetNamedSubStringValue("iqc", entity_Barcode.IQC);
        //        //btFormat.SetNamedSubStringValue("lot", entity_Barcode.Lot);
        //        btFormat.PrintOut(false, false);
        //        btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }


}
