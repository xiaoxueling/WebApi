using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Filters;
using System.Web.Http.SelfHost;

namespace FSTConsoleApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseAddress = "http://localhost:8081/";
            var config = new HttpSelfHostConfiguration(baseAddress);

            //跨域访问
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            //清除对xml格式的支持，只返回json数据
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            try
            {
                string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FSTWebApi.dll");
                var assembly = Assembly.UnsafeLoadFrom(dllPath);

                //AppDomain.CurrentDomain.Load(assembly.GetName());

                config.Routes.MapHttpRoute(
                    name: "default",
                    routeTemplate: "api/{controller}/{action}/{id}",
                    defaults: new
                    {
                        id = RouteParameter.Optional,
                        action = RouteParameter.Optional,
                    }
                 );

                Type[] classTypes = assembly.GetTypes();
                if (classTypes != null && classTypes.Length > 0)
                {
                    //筛选器
                    var filters = classTypes.Where(m => m.IsClass && m.IsPublic && m.IsVisible && m.IsSubclassOf(typeof(ActionFilterAttribute))).ToList();
                    if (filters != null && filters.Count > 0)
                    {
                        foreach (Type item in filters)
                        {
                            config.Filters.Add(Activator.CreateInstance(item) as ActionFilterAttribute);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Instantiating The Server...");
            using (var server = new HttpSelfHostServer(config))
            {
                //server.Configuration.Services.Replace(typeof(IAssembliesResolver), new ExtendedDefaultAssembliesResolver());

                server.OpenAsync().Wait();

                Console.WriteLine("Server is Running Now... @ " + baseAddress);
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
