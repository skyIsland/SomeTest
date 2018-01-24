using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NewLife.Serialization;

namespace MyTest.Config
{
    /// <summary>
    /// 取出配置信息
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        public static Config GetBasicConfig()
        {
            string result;
            var fileName = GetFilePath("~/Configs/")+ "Config.json";

            // 不存在文件 则自动生成
            if (!File.Exists(fileName))
            {
                Console.WriteLine("配置文件不存在,自动生成....");
                var fs = File.Create(fileName);
                fs.Close();
                var obj = new Config();
                var jsonStr = JsonHelper.ToJson(obj);
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.Write(jsonStr);
                }               
            }
            using (var sw=new StreamReader(fileName,Encoding.UTF8))
            {
                 result = sw.ReadToEnd();
            }
            return JsonHelper.ToJsonEntity<Config>(result); ;
        }
        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="setting"></param>
        public static void SetBasicConfig(Config config)
        {
            var path = GetFilePath("~/Configs/") + "Config.json";
            var fs = File.Create(path);
            fs.Close();
            var jsonStr = JsonHelper.ToJson(config);
            var fr = new StreamWriter(path);
            fr.Write(jsonStr);
            fr.Close();
        }

        /// <summary>
        /// 根据相对路径或绝对路径获取绝对路径
        /// </summary>
        /// <param name="strPath">相对路径或绝对路径</param>
        /// <returns>绝对路径</returns>
        public static string GetFilePath(string strPath)
        {
            strPath = strPath.Replace("/", "\\");
            if (strPath.StartsWith("\\"))
            {
                strPath = strPath.TrimStart('\\');
            }
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);

        }
    }
}
