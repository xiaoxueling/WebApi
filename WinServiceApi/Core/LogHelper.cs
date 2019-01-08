using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FSTWinServiceApi
{
     public class LogHelper
    {
        private static object _lock = new object();
        public static void LogInfo(string msg)
        {
            if (!AppSetting.EnableLog)
            {
                return;
            }

            Task.Run(() =>
            {
                lock (_lock)
                {
                    string file = AppDomain.CurrentDomain.BaseDirectory + "logs.txt";
                    File.AppendAllText(file, $"[{DateTime.Now.ToString()}]{msg}{Environment.NewLine}", Encoding.Default);
                }
            });
        }
    }
}
