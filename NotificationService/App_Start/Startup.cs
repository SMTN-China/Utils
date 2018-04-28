using System.Configuration;
using System.Net.Http.Formatting;
using System.Web.Http;
using Hangfire;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using NotificationService.App_Start;
using Owin;

[assembly: OwinStartup(typeof(NotificationService.Startup))]

namespace NotificationService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888

            HttpConfiguration config = new HttpConfiguration();

            config.Routes.MapHttpRoute(name: "DefaultApi",
                routeTemplate: "api/{Controller}/{action}",
                defaults: new
                {
                    id = RouteParameter.Optional
                });

            // 允许跨域
            app.UseCors(CorsOptions.AllowAll);

            //将默认xml返回数据格式改为json
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("datatype", "json", "application/json"));
            config.Filters.Add(new WebApiExceptionFilterAttribute());

            SwaggerConfig.Register(config);
            IocConfig.Register(config);

            app.MapSignalR();

            Hangfire.GlobalConfiguration.Configuration
               .UseStorage(new Hangfire.MySql.MySqlStorage("Hangfire"));

            app.UseHangfireDashboard();

            app.UseHangfireServer();

            app.UseWebApi(config);
        }
    }
}
