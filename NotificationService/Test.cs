using Autofac;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotificationService
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var webApp = WebApp.Start<Startup>(url: "http://*:" + ConfigurationManager.AppSettings["Port"] + "/");


            IocConfig.IContainer.Resolve<NLog.ILogger>().Info( "ggggggggggggggg");
            IocConfig.IContainer.Resolve<NLog.ILogger>().Info("ggggggggggggggg");

            IocConfig.IContainer.Resolve<NLog.ILogger>().Info("ggggggggggggggg");

            IocConfig.IContainer.Resolve<NLog.ILogger>().Info("ggggggggggggggg");

        }
    }
}
