using System;
using PubSub;

namespace pub_sub
{
    public class ProcessedListner
    {
        public ProcessedListner()
        {
            Hub.Default.Subscribe<ProcessedMessage>(this, pm => {
                Console.WriteLine("PROCESSED: "+ pm.Data);
            });
        }
    }
}
