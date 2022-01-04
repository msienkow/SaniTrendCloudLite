using System;
using System.Diagnostics;
using libplctag;
using libplctag.DataTypes;



namespace SaniTrendCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            const string gateway = "10.10.135.173";
            // const string path = "1,0";
            var realTag = new Tag<RealPlcMapper, float>()
            {
                Name = "Test_Analog",
                Gateway = gateway,
                // Path = path,
                PlcType = PlcType.Micro800,
                Protocol = Protocol.ab_eip
            };
           
            TimeSpan TimeAction(Action blockingAction)
            {
                Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
                blockingAction();
                stopWatch.Stop();
                return stopWatch.Elapsed;
            }
            var inittime = TimeAction(() =>
            {
                realTag.Initialize();
            });
            Console.WriteLine(inittime);
            float totalms = 0;
            for (int i = 0; i < 1000; i++) {
                var elapsed = TimeAction(() =>
                {
                    long start = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    realTag.Read();
                    // Console.WriteLine(realTag.Value);
                    long end = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    totalms += (end - start);

                });
                Console.WriteLine(elapsed);
            }
            
            float avg = totalms / 1000;
            Console.WriteLine($"Average Read: {avg}ms");
        }
    }
}