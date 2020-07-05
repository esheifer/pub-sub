using System;
using System.Threading.Tasks;
using PubSub;

namespace pub_sub
{
    class Program
    {
        static void Main(string[] args)
        { 
            var pm = new ProcessedListner();
            var l = new SimpleMovingAverageFloatingProcessor(new TimeFrameAggregate() { Seconds = 3 });
            var l2 = new SimpleMovingAverageFloatingProcessor(new TimeFrameAggregate() { Ticks = 10 });
            var s = new MarketDataSender();


            Console.WriteLine("");
            Console.WriteLine("any key");
            Console.ReadKey();
        }
    }
}
