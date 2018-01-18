using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sky.Crawler.Models;

namespace Sky.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            NewLife.Log.XTrace.LogPath = "D:\\Log";
            NewLife.Log.XTrace.WriteLine("开始...");
            //var o = new GifHelper();
            //o.Start();

            Console.ReadKey();
        }
    }
}
