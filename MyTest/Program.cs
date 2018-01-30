using System;
using System.Linq;
using System.Net;
using MyTest.Job;
using Quartz;
using Quartz.Impl;
using Sky.Crawler.FlySign;

namespace MyTest
{
    class Program
    {
        static void Main(string[] args)
        {
           // Test();
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

            //StartSign();
            // new Run();
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

        static void Test()
        {
            using (var wc=new WebClient())
            {
                wc.Proxy=new WebProxy("58.252.6.165", 9000);
                var result=wc.DownloadString("https://www.baidu.com/");
                Console.WriteLine(result);
            }
        }
        public class Run
        {
            public Run()
            {
                // 创建一个调度器
                var factory = new StdSchedulerFactory();
                var scheduler = factory.GetScheduler();
                scheduler.Result.Start();

                // 创建一个任务对象
                IJobDetail job = JobBuilder.Create<RunJob>().WithIdentity("job1", "group1").Build();

                // 创建一个触发器
                //DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .WithCronSchedule("0 0 0 * * ?")     //凌晨12点触发 0 0 0 * * ? 0/5 * * * * ?
                    //.StartAt(runTime)
                    .Build();

                // 将任务与触发器添加到调度器中
                scheduler.Result.ScheduleJob(job, trigger);
                // 开始执行
                scheduler.Result.Start();
            }
        }
    }
}
