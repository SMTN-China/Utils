using System;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(LightService.App_Start.Startup))]

namespace LightService.App_Start
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

            config.Filters.Add(new WebApiExceptionFilterAttribute());

            // 允许跨域
            app.UseCors(CorsOptions.AllowAll);

            //将默认xml返回数据格式改为json
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("datatype", "json", "application/json"));

            SwaggerConfig.Register(config);
            IocConfig.Register(config);

            app.UseWebApi(config);
        }
    }
}
