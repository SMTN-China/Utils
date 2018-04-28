using System.ServiceProcess;
using System.Windows.Forms;

namespace LightService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new LightService()
            };
            ServiceBase.Run(ServicesToRun);


            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Test());
        }
    }
}
