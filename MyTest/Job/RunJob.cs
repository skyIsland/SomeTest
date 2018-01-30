using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyTest.Config;
using Quartz;
using Sky.Crawler.FlySign;

namespace MyTest.Job
{
    public class RunJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {

            var info = ConfigHelper.GetBasicConfig().FlyCfg;
            var o = new FlySignIn(info.Email, info.Pwd);

            for (int i = 0; i < 5; i++)
            {
                o.Start();
            }
            return Task.CompletedTask;
        }
    }
}
