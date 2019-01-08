using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace FSTWinServiceApi
{
    public class AppSetting
    {
        /// <summary>
        /// 服务日志记录
        /// </summary>
        public static bool EnableLog => ConfigurationManager.AppSettings.Get("EnableLog")?.ToBoolean() ?? true;

        /// <summary>
        /// 请求日志记录
        /// </summary>
        public static bool EnableCors => ConfigurationManager.AppSettings.Get("EnableCors")?.ToBoolean() ?? true;

        /// <summary>
        /// 返回Xml格式
        /// </summary>
        public static bool SupportXml => ConfigurationManager.AppSettings.Get("SupportXml")?.ToBoolean() ?? false;

        
        /// <summary>
        /// 监听地址
        /// </summary>
        public static List<string> ListenAddress
        {
            get
            {
                var urls = ConfigurationManager.AppSettings.Get("ListenAddress") ?? string.Empty;

                if (!string.IsNullOrEmpty(urls))
                {
                    return urls.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }

                return null;
            }
        }

        /// <summary>
        /// WebApi程序集地址
        /// </summary>
        public static string WebApiDll => ConfigurationManager.AppSettings.Get("WebApiDll");


    }
}
