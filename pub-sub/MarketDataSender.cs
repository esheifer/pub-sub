using System;
using System.Threading;
using PubSub;

namespace pub_sub
{
    public class MarketDataSender
    {
        //---------------------------------------------------------------------
        // [fields]
        //---------------------------------------------------------------------

        private Hub hub = Hub.Default;


        //---------------------------------------------------------------------
        // [constructor]
        //---------------------------------------------------------------------

        public MarketDataSender()
        {
            for (int i = 0; i < 200; i++)
            {
                var md = new MarketData()
                {
                    Close = 100 + i,
                    DateTime = DateTime.Now
                };

                //Console.WriteLine("sending " + i.ToString());
                hub.Publish(md);
                //Thread.Sleep(new Random().Next(0, 100));
            }
        }
    }
}
