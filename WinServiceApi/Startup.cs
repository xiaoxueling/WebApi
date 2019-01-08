using Owin;
using System.Web.Http;
using Microsoft.Owin.Cors;
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Web.Http.Filters;

namespace FSTWinServiceApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            if (AppSetting.EnableCors)
            {
                //跨域访问
                appBuilder.UseCors(CorsOptions.AllowAll);
                LogHelper.LogInfo("开启跨域访问：Success");
            }
            

            HttpConfiguration config = new HttpConfiguration();

            if (!AppSetting.SupportXml)
            {
                //清除对xml格式的支持，只返回json数据
                config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
                LogHelper.LogInfo("清除对xml格式的支持：Success");
            }
           
            //加载WEBAPI程序集
            LoadWebApiDLL(config);

            //路由
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    action = RouteParameter.Optional
                }
            );

            LogHelper.LogInfo("路由配置：Success");

            appBuilder.UseWebApi(config);
        }

        /// <summary>
        /// 加载WEBAPI程序集
        /// </summary>
        private void LoadWebApiDLL(HttpConfiguration config)
        {
            try
            {
                if (string.IsNullOrEmpty(AppSetting.WebApiDll))
                {
                    LogHelper.LogInfo("WebApiDll 未配置");
                    return;
                }
                string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,AppSetting.WebApiDll);
                if (!File.Exists(dllPath))
                {
                    LogHelper.LogInfo($"WebApiDll配置错误：未找到DLL文件！--{dllPath}");
                    return;
                }

                var assembly = Assembly.UnsafeLoadFrom(dllPath);
                //AppDomain.CurrentDomain.Load(assembly.GetName());
                
                //加载筛选器
                Type[] classTypes = assembly.GetTypes();
                if (classTypes != null && classTypes.Length > 0)
                {
                    var filters = classTypes.Where(m => m.IsClass && m.IsPublic && m.IsVisible && m.IsSubclassOf(typeof(ActionFilterAttribute))).ToList();
                    if (filters != null && filters.Count > 0)
                    {
                        foreach (Type item in filters)
                        {
                            config.Filters.Add(Activator.CreateInstance(item) as ActionFilterAttribute);
                        }
                    }
                }

                LogHelper.LogInfo("加载WEBAPI程序集：Success");
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("加载WEBAPI程序集：Error--"+ex.Message);
                throw ex;
            }
        }
    }
}
