using System;
using System.Linq;
using MyTest.Config;
using Sky.Crawler.FlySign;

namespace MyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var str = "第54页";

            //var s = from p in str where p.ToString().ToInt(-1) > 0 select p;

            //var result = string.Empty;

            //foreach (var c in str)
            //{
            //    var a = c.ToInt(-1);
            //    if ( a> 0)
            //    {
            //        result += c;
            //    }
            //}
            //Console.WriteLine("s = "+s.Join(""));
            //Console.WriteLine("Result = " + result);

            StartSign();
            Console.ReadKey();
        }

        /// <summary>
        /// 签到 todo:定时轮子:http://blog.csdn.net/u013711462/article/details/53449799
        /// </summary>
        static void StartSign()
        {
            var flyConfig = ConfigHelper.GetBasicConfig().FlyCfg;
            var fly = new FlySignIn(
                "Hm_lvt_a353f6c055ee7ac4ac9d1af83b556ef4=1488963535,1489112224,1489546169; UM_distinctid=15e59c095a1218-0684c1f914c599-3e64430f-1fa400-15e59c095a24bf; CNZZDATA30088308=cnzz_eid%3D792246781-1502875390-http%253A%252F%252Fwww.layui.com%252F%26ntime%3D1516760465; fly-layui=s%3AzyYSaO8b3RspMIV9E1lzKW3J67Ef2uOd.eMRtFxRsPJwfwZrZd17bv4B2dGwzuWE2lS3mC8EazHA", "11b42a2ba0df589f65dc68dcf9e6657273ac1118");
            fly.Start();
        }
    }
}
