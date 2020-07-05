using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PubSub;

namespace pub_sub
{
    public class SimpleMovingAverageFloatingProcessor
    {
        //---------------------------------------------------------------------
        // [fields]
        //---------------------------------------------------------------------

        private const int waitIndefinitelyMilliseconds = -1;  //make global

        private TimeFrameAggregate timeFrame;
        private List<MarketData> allMarketData;
        private BlockingCollection<MarketData> incommingMarketData;
        

        //---------------------------------------------------------------------
        // [constructors]
        //---------------------------------------------------------------------

        public SimpleMovingAverageFloatingProcessor(TimeFrameAggregate timeFrame)
        {
            this.timeFrame = timeFrame;
            this.allMarketData = new List<MarketData>();
            this.incommingMarketData = new BlockingCollection<MarketData>();

            Task.Run(async () => { await ProcessMarketData(); });

            Hub.Default.Subscribe<MarketData>(this, message =>
            {
                this.incommingMarketData.Add(message);
            });
        }


        //---------------------------------------------------------------------
        // [methods]
        //---------------------------------------------------------------------

        private decimal? ProcessByTicks(int numberOfTicks)
        {
            this.allMarketData = this.allMarketData.TakeLast(numberOfTicks).ToList();
            if (this.allMarketData.Count() < numberOfTicks)
                return null;

            decimal result = this.allMarketData.Select(md => md.Close).Average();
            return result;
        }


        private decimal? ProcessBySeconds(int numberOfSeconds)
        {
            var cutoffTimestamp = DateTime.Now.AddSeconds(numberOfSeconds * -1);
            this.allMarketData.RemoveAll(md => md.DateTime < cutoffTimestamp);

            decimal result = this.allMarketData.Select(md=> md.Close).Average();
            return result;
        }

        /*private decimal? ProcessBySeconds(int numberOfSeconds)
        {
            var cutoffTimestamp = DateTime.Now.AddSeconds(numberOfSeconds * -1);
            this.allMarketData.RemoveAll(md => md.DateTime < cutoffTimestamp);

            var lastClosingValue = new Dictionary<DateTime, decimal>();
            var lastClosingDate = new DateTime();

            foreach(var singleMarketData in this.allMarketData)
            {
                var d = singleMarketData.DateTime;
                var dateKey = new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);

                if (!lastClosingValue.ContainsKey(dateKey))
                {
                    lastClosingValue[dateKey] = singleMarketData.Close;
                    continue;
                }

                if (lastClosingDate > singleMarketData.DateTime)
                {
                    lastClosingValue[dateKey] = singleMarketData.Close;
                    lastClosingDate = singleMarketData.DateTime;
                }
            }

            decimal result = lastClosingValue.Values.Average();
            return result;
        }*/

        private async Task ProcessMarketData()
        {
            while (!this.incommingMarketData.IsCompleted)
            {
                MarketData item;

                if (!this.incommingMarketData.TryTake(out item, waitIndefinitelyMilliseconds))
                    continue;

                this.allMarketData.Add(item);

                decimal? floatingSmaClose = (this.timeFrame.Ticks.HasValue) ?
                    ProcessByTicks(this.timeFrame.Ticks.Value) : ProcessBySeconds(this.timeFrame.Seconds.Value);

                if (!floatingSmaClose.HasValue)
                    continue;

                //await Task.Delay(new Random().Next(10, 50));
                Hub.Default.Publish(new ProcessedMessage("sma-floating:" + floatingSmaClose));
            }
        }
    }
}
