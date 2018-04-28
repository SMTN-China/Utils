using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using Hangfire;
using System.Data;
using System.Data.SqlClient;
using NotificationService.Services;

namespace NotificationService
{
    public class IocConfig
    {
        public static IContainer IContainer { get; set; }

        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            builder
                .Register(r => NLog.LogManager.GetCurrentClassLogger())
                .As<NLog.ILogger>()
                .SingleInstance().PropertiesAutowired();

            builder
                .Register<SqlConnection>(r=> new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["erp"].ConnectionString))
                .As<IDbConnection>()
                .SingleInstance().PropertiesAutowired();

            builder.RegisterType<HttpHelp>().SingleInstance().PropertiesAutowired();

            builder.RegisterType<ErpSyncService>().SingleInstance().PropertiesAutowired();

            builder.RegisterType<OutLifeReelMailService>().SingleInstance().PropertiesAutowired();

            IContainer = builder.Build();

            Hangfire.GlobalConfiguration.Configuration.UseAutofacActivator(IContainer);

            config.DependencyResolver = new AutofacWebApiDependencyResolver(IContainer);
        }
    }
}
