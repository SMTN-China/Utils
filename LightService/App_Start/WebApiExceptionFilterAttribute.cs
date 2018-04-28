using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace LightService.App_Start
{
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public ILogger ILogger { get; set; }
        public WebApiExceptionFilterAttribute()
        {
            ILogger = NLog.LogManager.GetCurrentClassLogger();
        }

        //重写基类的异常处理方法
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            ILogger.Error(actionExecutedContext.Exception);

            base.OnException(actionExecutedContext);
        }
    }
}
