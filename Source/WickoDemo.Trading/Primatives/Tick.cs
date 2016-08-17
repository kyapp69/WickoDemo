// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 8/17/2016

using System;

namespace WickoDemo.Shared
{
    public class Tick
    {
        public Symbol Symbol { get; set; }
        public DateTime TickOn { get; set; }
        public double BidRate { get; set; }
        public double AskRate { get; set; }

        public double MidRate =>
            ((BidRate + AskRate) / 2.0).ToRoundedRate(Symbol);

        public double ToRate(RateToUse rateToUse)
        {
            switch (rateToUse)
            {
                case RateToUse.BidRate:
                    return BidRate;
                case RateToUse.AskRate:
                    return BidRate;
                default:
                    return MidRate;
            }
        }
    }
}
