using System;
using System.Linq;
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
            var fly = new FlySignIn("1434787335@qq.com","961031787meng");
            fly.Start();
        }
    }
}
