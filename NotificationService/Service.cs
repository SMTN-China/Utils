using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService
{
    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var webApp = WebApp.Start<Startup>(url: "http://*:" + ConfigurationManager.AppSettings["Port"] + "/");

        }

        protected override void OnStop()
        {
        }
    }
}
