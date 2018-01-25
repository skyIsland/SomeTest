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
            var fileName = GetFilePath("/Config/")+ "Config.json";

            // 不存在文件 则自动生成
            if (!File.Exists(fileName))
            {
                var fs = File.Create(fileName);
                fs.Close();
                var obj = new Config();
               var jsonStr = obj.ToJson();
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.Write(jsonStr);
                }               
            }
            using (var sw=new StreamReader(fileName,Encoding.UTF8))
            {
                 result = sw.ReadToEnd();
            }
            return result.ToJsonEntity<Config>(); ;
        }
        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="config"></param>
        public static void SetBasicConfig(Config config)
        {
            var path = GetFilePath("/Config/") + "Config.json";
            var fs = File.Create(path);
            fs.Close();
            var jsonStr = config.ToJson();
            var fr = new StreamWriter(path);
            fr.Write(jsonStr);
            fr.Close();
        }

        /// <summary>
        /// 根据相对路径或绝对路径获取绝对路径  todo:该方法暂时没有作用...
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
