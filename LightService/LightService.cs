using Autofac;
using LightService.App_Start;
using LightService.Services;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;


namespace LightService
{
    public partial class LightService : ServiceBase
    {
        public LightService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // 开启webapi 服务器
            var webApp = WebApp.Start<Startup>(url: "http://*:"+ ConfigurationManager.AppSettings["Port"] + "/");

            // 开启消息接收客户端
            IocConfig.IContainer.Resolve<HubClient>().Login();
        }

        protected override void OnStop()
        {
            
        }
    }
}
