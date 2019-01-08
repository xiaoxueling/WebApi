using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace FSTWinServiceApi
{
    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                LogHelper.LogInfo("=============服务启动START=============");

                List<string> urls = AppSetting.ListenAddress;

                if (urls == null || urls.Count == 0)
                {
                    throw new Exception("监听地址未配置");
                }

                foreach (var url in urls)
                {
                    try
                    {
                        WebApp.Start<Startup>(url);
                        LogHelper.LogInfo($"监听地址 {url} :Success");
                        LogHelper.LogInfo(Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogInfo($"监听地址 {url}，Error--" + JsonConvert.SerializeObject(new { Message = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source }));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo($"服务启动 :Error--{ex.Message}");
                throw ex;
            }
            finally
            {
                LogHelper.LogInfo("=============服务启动END=============");
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        protected override void OnStop()
        {
            LogHelper.LogInfo("服务停止");
        }
    }
}
