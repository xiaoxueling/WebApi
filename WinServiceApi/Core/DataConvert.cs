using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;

namespace FSTWinServiceApi
{
    /// <summary>
    /// 数据转换扩展类
    /// </summary>
    public static class DataConvert
    {
        /// <summary>
        /// string To int32
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static int ToInt(this string str,int defaultValue=0)
        {
            int result;
            try
            {
                if (!int.TryParse(str, out result))
                {
                    result = defaultValue;
                };
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// string To int64
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static Int64 ToInt64(this string str, Int64 defaultValue=0)
        {
            Int64 result;
            try
            {
                if (!Int64.TryParse(str, out result))
                {
                    result = defaultValue;
                };
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// string To double
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static double ToDouble(this string str,double defaultValue=0)
        {
            double result;

            try
            {
                if (!double.TryParse(str, out result))
                {
                    result = defaultValue;
                };
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// string To decimal
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string str, decimal defaultValue=0)
        {
            decimal result;
            try
            {
                if (!decimal.TryParse(str, out result))
                {
                    result = defaultValue;
                };
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// string To long
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static long ToLong(this string str, long defaultValue = 0)
        {
            long result;
            try
            {
                if (!long.TryParse(str, out result))
                {
                    result = defaultValue;
                };
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// IP地址转换为整数
        /// </summary>
        /// <param name="ip">ip地址（***.***.***.***）</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static long ToLongForIp(this string ip, long defaultValue = 0)
        {
            long result;
            try
            {
                IPAddress ipaddress;

                if (IPAddress.TryParse(ip, out ipaddress))
                {
                    byte[] addbuffer = ipaddress.GetAddressBytes();
                    Array.Reverse(addbuffer);
                    result = BitConverter.ToUInt32(addbuffer, 0);
                }
                else
                {
                    result = defaultValue;
                }
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// string To float
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static float ToFloat(this string str, float defaultValue = 0)
        {
            float result;
            try
            {
                if (!float.TryParse(str, out result))
                {
                    result = defaultValue;
                };
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// string To boolean
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static bool ToBoolean(this string str,bool defaultValue = false)
        {
            bool result;
            try
            {
                List<string> trueList = new List<string>()
                {
                    "1","是","对","正确","OK","YES","Y"
                };

                if (trueList.Any(a => a.Equals(str.ToUpper())))
                {
                    return true;
                }

                if (!bool.TryParse(str, out result))
                {
                    result = defaultValue;
                };
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// string To DateTime
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str,DateTime? defaultValue=null)
        {
            DateTime result=DateTime.MinValue;
            try
            {
                if (!DateTime.TryParse(str, out result))
                {
                    if (defaultValue.HasValue)
                    {
                        result = defaultValue.Value;
                    }
                };
            }
            catch
            {
                if (defaultValue.HasValue)
                {
                    result=defaultValue.Value;
                }
            }
            return result;
        }

        /// <summary>
        /// 时间戳转为时间
        /// </summary>
        /// <returns></returns>
        public static DateTime ToDateTimeFromTimestamp(this string timestamp)
        {
            try
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                var lTime = (timestamp + "0000000").ToLong();
                TimeSpan toNow = new TimeSpan(lTime);
                return dtStart.Add(toNow);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 当前时间转为时间戳
        /// </summary>
        /// <returns></returns>
        public static long ToUnixTime(DateTime time)
        {
            try
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                return (long)(time - startTime).TotalSeconds;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary> 
        /// 序列化对象为XML
        /// </summary> 
        /// <returns></returns> 
        public static string XmlSerialize<T>(this T t)
            where T : class, new()
        {
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    XmlSerializer xz = new XmlSerializer(typeof(T));
                    xz.Serialize(sw, t);
                    return sw.ToString();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary> 
        /// 反序列化XML为指定类型对象 
        /// </summary> 
        /// <returns></returns> 
        public static T XmlDeserialize<T>(this string xml)
            where T : class, new()
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xz = new XmlSerializer(typeof(T));
                    return xz.Deserialize(sr) as T;
                }
            }
            catch
            {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// 序列化对象为Json
        /// </summary>
        /// <returns></returns>
        public static string JsonSerialize<T>(this T t)
        {
            try
            {
                return JsonConvert.SerializeObject(t);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary> 
        /// 反序列化JSON为指定类型对象 
        /// </summary> 
        /// <returns></returns> 
        public static T JsonDeserialize<T>(this string jsonStr)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonStr);
            }
            catch 
            {
                return Activator.CreateInstance<T>();
            }

        }
    }
}