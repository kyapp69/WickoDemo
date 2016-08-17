// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 8/17/2016

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WickoDemo.Shared;

namespace WickoDemo.Trading.Tests
{
    [TestClass()]
    public class WickoFeedTests
    {
        [TestMethod()]
        public void FallingFallingRising()
        {
            var feed = new WickoFeed(
                Symbol.EURUSD, 10000.0, RateToUse.BidRate);

            var wickos = new List<Wicko>();

            feed.OnWicko += (s, e) => wickos.Add(e.Wicko);

            feed.HandleTick(new Tick { BidRate = 10.0 });
            feed.HandleTick(new Tick { BidRate = 10.5 });
            feed.HandleTick(new Tick { BidRate = 9.5 });
            feed.HandleTick(new Tick { BidRate = 8.9 });

            feed.HandleTick(new Tick { BidRate = 9.1 });
            feed.HandleTick(new Tick { BidRate = 9.2 });
            feed.HandleTick(new Tick { BidRate = 8.5 });
            feed.HandleTick(new Tick { BidRate = 7.8 });

            feed.HandleTick(new Tick { BidRate = 7.6 });
            feed.HandleTick(new Tick { BidRate = 8.5 });
            feed.HandleTick(new Tick { BidRate = 9.2 });
            feed.HandleTick(new Tick { BidRate = 10.1 });

            Assert.AreEqual(wickos.Count, 3);

            Assert.AreEqual(wickos[0].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[0].OpenRate, 10.0);
            Assert.AreEqual(wickos[0].HighRate, 10.5);
            Assert.AreEqual(wickos[0].LowRate, 9.0);
            Assert.AreEqual(wickos[0].CloseRate, 9.0);
            Assert.AreEqual(wickos[0].Trend, Trend.Falling);
            Assert.AreEqual(wickos[0].Spread, 1.0);

            Assert.AreEqual(wickos[1].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[1].OpenRate, 9.0);
            Assert.AreEqual(wickos[1].HighRate, 9.2);
            Assert.AreEqual(wickos[1].LowRate, 8.0);
            Assert.AreEqual(wickos[1].CloseRate, 8.0);
            Assert.AreEqual(wickos[1].Trend, Trend.Falling);
            Assert.AreEqual(wickos[1].Spread, 1.0);

            Assert.AreEqual(wickos[2].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[2].OpenRate, 9.0);
            Assert.AreEqual(wickos[2].HighRate, 10.0);
            Assert.AreEqual(wickos[2].LowRate, 7.6);
            Assert.AreEqual(wickos[2].CloseRate, 10.0);
            Assert.AreEqual(wickos[2].Trend, Trend.Rising);
            Assert.AreEqual(wickos[2].Spread, 1.0);

            var openWicko = feed.OpenWicko;

            Assert.AreEqual(openWicko.OpenRate, 10.0);
            Assert.AreEqual(openWicko.HighRate, 10.1);
            Assert.AreEqual(openWicko.LowRate, 10.0);  
            Assert.AreEqual(openWicko.CloseRate, 10.1); 
        }
    }
}