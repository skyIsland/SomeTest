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
            //var flyConfig = ConfigHelper.GetBasicConfig().FlyCfg;
            var fly = new FlySignIn(
                "Hm_lvt_a353f6c055ee7ac4ac9d1af83b556ef4=1488963535,1489112224,1489546169; UM_distinctid=15e59c095a1218-0684c1f914c599-3e64430f-1fa400-15e59c095a24bf; CNZZDATA30088308=cnzz_eid%3D792246781-1502875390-http%253A%252F%252Fwww.layui.com%252F%26ntime%3D1516760465; fly-layui=s%3AzyYSaO8b3RspMIV9E1lzKW3J67Ef2uOd.eMRtFxRsPJwfwZrZd17bv4B2dGwzuWE2lS3mC8EazHA", "11b42a2ba0df589f65dc68dcf9e6657273ac1118");
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
