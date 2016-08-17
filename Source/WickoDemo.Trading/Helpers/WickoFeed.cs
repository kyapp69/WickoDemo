// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 8/17/2016

using System;

namespace WickoDemo.Shared
{
    public class WickoFeed
    {
        public class WickoArgs : EventArgs
        {
            public WickoArgs(Wicko wicko)
            {
                Wicko = wicko;
            }

            public Wicko Wicko { get; }
        }

        private bool firstTick = true;
        private Wicko lastWicko = null;

        private DateTime openOn;
        private DateTime closeOn;
        private double openRate;
        private double highRate;
        private double lowRate;
        private double closeRate;
        private double wickoSize;
        private RateToUse rateToUse;

        public event EventHandler<WickoArgs> OnWicko;

        public WickoFeed(Symbol symbol, double wickoPips, RateToUse rateToUse)
        {
            Symbol = symbol;
            wickoSize = wickoPips.PipsToRate(symbol);
            this.rateToUse = rateToUse;
        }

        public Symbol Symbol { get; }

        private Wicko GetNewWicko(double openRate, 
            double highRate, double lowRate, double closeRate)
        {
            return new Wicko()
            {
                Symbol = Symbol,
                OpenOn = openOn,
                CloseOn = closeOn,
                OpenRate = openRate,
                HighRate = highRate,
                LowRate = lowRate,
                CloseRate = closeRate
            };
        }

        private void Rising()
        {
            double limit;

            while (closeRate > (limit = (openRate + wickoSize)
                .ToRoundedRate(Symbol)))
            {
                var wicko = GetNewWicko(
                    openRate, limit, lowRate, limit);

                lastWicko = wicko;

                OnWicko?.Invoke(this, new WickoArgs(wicko));

                openOn = closeOn;
                openRate = limit;
                lowRate = limit;
            }
        }

        private void Falling()
        {
            double limit;

            while (closeRate < (limit = (openRate - wickoSize)
                .ToRoundedRate(Symbol)))
            {
                var wicko = GetNewWicko(
                    openRate, highRate, limit, limit);

                lastWicko = wicko;

                OnWicko?.Invoke(this, new WickoArgs(wicko));

                openOn = closeOn;
                openRate = limit;
                highRate = limit;
            }
        }

        internal Wicko OpenWicko
        {
            get
            {
                return GetNewWicko(
                    openRate, highRate, lowRate, closeRate);
            }
        }

        public void HandleTick(Tick tick)
        {
            var rate = tick.ToRate(rateToUse);

            if (firstTick)
            {
                firstTick = false;

                openOn = tick.TickOn;
                closeOn = tick.TickOn;
                openRate = rate;
                highRate = rate;
                lowRate = rate;
                closeRate = rate;
            }
            else
            {
                closeOn = tick.TickOn;

                if (rate > highRate)
                    highRate = rate;

                if (rate < lowRate)
                    lowRate = rate;

                closeRate = rate;

                if (closeRate > openRate) 
                {
                    if (lastWicko == null ||
                        (lastWicko.Trend == Trend.Rising))
                    {
                        Rising();

                        return;
                    }

                    var limit = (lastWicko.OpenRate + wickoSize)
                        .ToRoundedRate(Symbol);

                    if (closeRate > limit)
                    {
                        var wicko = GetNewWicko(
                            lastWicko.OpenRate, limit, lowRate, limit);

                        lastWicko = wicko;

                        OnWicko?.Invoke(this, new WickoArgs(wicko));

                        openOn = closeOn;
                        openRate = limit;
                        lowRate = limit;

                        Rising();
                    }
                }
                else if (closeRate < openRate) 
                {
                    if (lastWicko == null || 
                        (lastWicko.Trend == Trend.Falling))
                    {
                        Falling();

                        return;
                    }

                    var limit = (lastWicko.OpenRate - wickoSize)
                        .ToRoundedRate(Symbol);

                    if (closeRate < limit)
                    {
                        var wicko = GetNewWicko(
                            lastWicko.OpenRate, highRate, limit, limit);

                        lastWicko = wicko;

                        OnWicko?.Invoke(this, new WickoArgs(wicko));

                        openOn = closeOn;
                        openRate = limit;
                        highRate = limit;

                        Falling();
                    }
                }
            }
        }
    }
}