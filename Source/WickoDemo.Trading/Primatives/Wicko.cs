// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 8/17/2016

using System;

namespace WickoDemo.Shared
{
    public class Wicko
    {
        public Symbol Symbol { get; internal set; }
        public DateTime OpenOn { get; internal set; }
        public DateTime CloseOn { get; internal set; }
        public double OpenRate { get; internal set; }
        public double HighRate { get; internal set; }
        public double LowRate { get; internal set; }
        public double CloseRate { get; internal set; }

        public Trend Trend
        {
            get
            {
                if (OpenRate < CloseRate)
                    return Trend.Rising;
                else if (OpenRate > CloseRate)
                    return Trend.Falling;
                else
                    return Trend.NoDelta;
            }
        }

        public double Spread => Math.Abs(
            (CloseRate - OpenRate).ToRoundedRate(Symbol));
    }
}
