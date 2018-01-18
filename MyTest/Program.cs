using System;
using System.Linq;

namespace MyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = "第54页";

            var s = from p in str where p.ToString().ToInt(-1) > 0 select p;

            var result = string.Empty;

            foreach (var c in str)
            {
                var a = c.ToInt(-1);
                if ( a> 0)
                {
                    result += c;
                }
            }
            Console.WriteLine("s = "+s.Join(""));
            Console.WriteLine("Result = " + result);
            Console.ReadKey();
        }
    }
}
