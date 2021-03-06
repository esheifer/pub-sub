﻿using System;
namespace pub_sub
{
    public class MarketData
    {
        //---------------------------------------------------------------------
        // [properties]
        //---------------------------------------------------------------------

        public int Index { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public long Volume { get; set; }
        public DateTime DateTime { get; set; }
    }
}
