using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FSTWinServiceApi.Filters
{
    public class RequestFilter:ActionFilterAttribute
    {
        private static object _lock = new object();
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Task.Run(() =>
            {
                lock (_lock)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/RequestLog/" + DateTime.Now.ToString("yyyy年MM月") + "/";
                    DirectoryInfo info = new DirectoryInfo(path);
                    if (!info.Exists)
                    {
                        info.Create();
                    }
                    string file = path + DateTime.Now.Day + "日.txt";
                    File.AppendAllText(file,actionContext.Request.RequestUri.AbsolutePath+Environment.NewLine+actionContext.Request.Method.Method, Encoding.Default);
                }
            });

            base.OnActionExecuting(actionContext);
        }
    }
}
