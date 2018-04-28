using Autofac;
using LightService.App_Start;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightService
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 开启webapi 服务器
            var webApp = WebApp.Start<Startup>(url: "http://*:8084/");

            // 开启消息接收客户端
            // IocConfig.IContainer.Resolve<HubClient>().Login();
        }
    }
}
