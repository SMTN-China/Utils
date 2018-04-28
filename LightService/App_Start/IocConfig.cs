using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using System;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using LightService.Services;

namespace LightService.App_Start
{
    public class IocConfig
    {
        public static IContainer IContainer { get; set; }

        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            builder.Register<NLog.ILogger>(r => NLog.LogManager.GetCurrentClassLogger()).SingleInstance().PropertiesAutowired();

            builder.RegisterType<LYLightUtils>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<LYLightService>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<HubClient>().SingleInstance().PropertiesAutowired();

            IContainer = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(IContainer);
        }
    }
}
