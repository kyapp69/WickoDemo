// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 8/17/2016

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using WickoDemo.Shared;

namespace WickoDemo.Trading.Tests
{
    [TestClass()]
    public class WickoFeedTests
    {
        [TestMethod()]
        public void NoFallingWicko()
        {
            var feed = new WickoFeed(
                Symbol.EURUSD, 10000.0, RateToUse.BidRate);

            var wickos = new List<Wicko>();

            feed.OnWicko += (s, e) => wickos.Add(e.Wicko);

            feed.HandleTick(new Tick { BidRate = 10.0 });
            feed.HandleTick(new Tick { BidRate = 9.1 });

            Assert.AreEqual(wickos.Count, 0);

            var openWicko = feed.OpenWicko;

            Assert.AreEqual(openWicko.OpenRate, 10.0);
            Assert.AreEqual(openWicko.HighRate, 10.0);
            Assert.AreEqual(openWicko.LowRate, 9.1);
            Assert.AreEqual(openWicko.CloseRate, 9.1);
        }

        [TestMethod()]
        public void NoRisingWicko()
        {
            var feed = new WickoFeed(
                Symbol.EURUSD, 10000.0, RateToUse.BidRate);

            var wickos = new List<Wicko>();

            feed.OnWicko += (s, e) => wickos.Add(e.Wicko);

            feed.HandleTick(new Tick { BidRate = 10.0 });
            feed.HandleTick(new Tick { BidRate = 10.9 });

            Assert.AreEqual(wickos.Count, 0);

            var openWicko = feed.OpenWicko;

            Assert.AreEqual(openWicko.OpenRate, 10.0);
            Assert.AreEqual(openWicko.HighRate, 10.9);
            Assert.AreEqual(openWicko.LowRate, 10.0);
            Assert.AreEqual(openWicko.CloseRate, 10.9);
        }

        [TestMethod()]
        public void NoFallingWickoKissLimit()
        {
            var feed = new WickoFeed(
                Symbol.EURUSD, 10000.0, RateToUse.BidRate);

            var wickos = new List<Wicko>();

            feed.OnWicko += (s, e) => wickos.Add(e.Wicko);

            feed.HandleTick(new Tick { BidRate = 10.0 });
            feed.HandleTick(new Tick { BidRate = 9.0 });

            Assert.AreEqual(wickos.Count, 0);

            var openWicko = feed.OpenWicko;

            Assert.AreEqual(openWicko.OpenRate, 10.0);
            Assert.AreEqual(openWicko.HighRate, 10.0);
            Assert.AreEqual(openWicko.LowRate, 9.0);
            Assert.AreEqual(openWicko.CloseRate, 9.0);
        }

        [TestMethod()]
        public void NoRisingWickoKissLimit()
        {
            var feed = new WickoFeed(
                Symbol.EURUSD, 10000.0, RateToUse.BidRate);

            var wickos = new List<Wicko>();

            feed.OnWicko += (s, e) => wickos.Add(e.Wicko);

            feed.HandleTick(new Tick { BidRate = 10.0 });
            feed.HandleTick(new Tick { BidRate = 11.0 });

            Assert.AreEqual(wickos.Count, 0);

            var openWicko = feed.OpenWicko;

            Assert.AreEqual(openWicko.OpenRate, 10.0);
            Assert.AreEqual(openWicko.HighRate, 11.0);
            Assert.AreEqual(openWicko.LowRate, 10.0);
            Assert.AreEqual(openWicko.CloseRate, 11.0);
        }

        [TestMethod()]
        public void OneFallingWicko()
        {
            var feed = new WickoFeed(
                Symbol.EURUSD, 10000.0, RateToUse.BidRate);

            var wickos = new List<Wicko>();

            feed.OnWicko += (s, e) => wickos.Add(e.Wicko);

            var tickOn1 = new DateTime(2016, 1, 1, 17, 0, 0, 0);
            var tickOn2 = new DateTime(2016, 1, 1, 17, 0, 0, 1);

            feed.HandleTick(new Tick { TickOn = tickOn1, BidRate = 10.0 });
            feed.HandleTick(new Tick { TickOn = tickOn2, BidRate = 8.9 });

            Assert.AreEqual(wickos.Count, 1);

            Assert.AreEqual(wickos[0].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[0].OpenRate, 10.0);
            Assert.AreEqual(wickos[0].HighRate, 10.0);
            Assert.AreEqual(wickos[0].LowRate, 9.0);
            Assert.AreEqual(wickos[0].CloseRate, 9.0);
            Assert.AreEqual(wickos[0].Trend, Trend.Falling);
            Assert.AreEqual(wickos[0].Spread, 1.0);
            Assert.AreEqual(wickos[0].OpenOn, tickOn1);
            Assert.AreEqual(wickos[0].CloseOn, tickOn2);

            var openWicko = feed.OpenWicko;

            Assert.AreEqual(openWicko.OpenOn, tickOn2);
            Assert.AreEqual(openWicko.CloseOn, tickOn2);
            Assert.AreEqual(openWicko.OpenRate, 9.0);
            Assert.AreEqual(openWicko.HighRate, 9.0);
            Assert.AreEqual(openWicko.LowRate, 8.9);
            Assert.AreEqual(openWicko.CloseRate, 8.9);
        }

        [TestMethod()]
        public void OneRisingWicko()
        {
            var feed = new WickoFeed(
                Symbol.EURUSD, 10000.0, RateToUse.BidRate);

            var wickos = new List<Wicko>();

            feed.OnWicko += (s, e) => wickos.Add(e.Wicko);

            var tickOn1 = new DateTime(2016, 1, 1, 17, 0, 0, 0);
            var tickOn2 = new DateTime(2016, 1, 1, 17, 0, 0, 1);

            feed.HandleTick(new Tick { TickOn = tickOn1, BidRate = 9.0 });
            feed.HandleTick(new Tick { TickOn = tickOn2, BidRate = 10.1 });

            Assert.AreEqual(wickos.Count, 1);

            Assert.AreEqual(wickos[0].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[0].OpenRate, 9.0);
            Assert.AreEqual(wickos[0].HighRate, 10.0);
            Assert.AreEqual(wickos[0].LowRate, 9.0);
            Assert.AreEqual(wickos[0].CloseRate, 10.0);
            Assert.AreEqual(wickos[0].Trend, Trend.Rising);
            Assert.AreEqual(wickos[0].Spread, 1.0);
            Assert.AreEqual(wickos[0].OpenOn, tickOn1);
            Assert.AreEqual(wickos[0].CloseOn, tickOn2);

            var openWicko = feed.OpenWicko;

            Assert.AreEqual(openWicko.OpenOn, tickOn2);
            Assert.AreEqual(openWicko.CloseOn, tickOn2);
            Assert.AreEqual(openWicko.OpenRate, 10.0);
            Assert.AreEqual(openWicko.HighRate, 10.1);
            Assert.AreEqual(openWicko.LowRate, 10.0);
            Assert.AreEqual(openWicko.CloseRate, 10.1);
        }

        [TestMethod()]
        public void TwoFallingThenOneRisingWickos()
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

        [TestMethod()]
        public void TwoRisingThenOneFallingWickos()
        {
            var feed = new WickoFeed(
                Symbol.EURUSD, 10000.0, RateToUse.BidRate);

            var wickos = new List<Wicko>();

            feed.OnWicko += (s, e) => wickos.Add(e.Wicko);

            feed.HandleTick(new Tick { BidRate = 10.0 });
            feed.HandleTick(new Tick { BidRate = 9.6 });
            feed.HandleTick(new Tick { BidRate = 10.5 });
            feed.HandleTick(new Tick { BidRate = 11.1 });

            feed.HandleTick(new Tick { BidRate = 11.0 });
            feed.HandleTick(new Tick { BidRate = 10.7 });
            feed.HandleTick(new Tick { BidRate = 11.6 });
            feed.HandleTick(new Tick { BidRate = 12.3 });

            feed.HandleTick(new Tick { BidRate = 12.3 });
            feed.HandleTick(new Tick { BidRate = 12.4 });
            feed.HandleTick(new Tick { BidRate = 11.5 });
            feed.HandleTick(new Tick { BidRate = 9.9 });

            Assert.AreEqual(wickos.Count, 3);

            Assert.AreEqual(wickos[0].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[0].OpenRate, 10.0);
            Assert.AreEqual(wickos[0].HighRate, 11.0);
            Assert.AreEqual(wickos[0].LowRate, 9.6);
            Assert.AreEqual(wickos[0].CloseRate, 11.0);
            Assert.AreEqual(wickos[0].Trend, Trend.Rising);
            Assert.AreEqual(wickos[0].Spread, 1.0);

            Assert.AreEqual(wickos[1].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[1].OpenRate, 11.0);
            Assert.AreEqual(wickos[1].HighRate, 12.0);
            Assert.AreEqual(wickos[1].LowRate, 10.7);
            Assert.AreEqual(wickos[1].CloseRate, 12.0);
            Assert.AreEqual(wickos[1].Trend, Trend.Rising);
            Assert.AreEqual(wickos[1].Spread, 1.0);

            Assert.AreEqual(wickos[2].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[2].OpenRate, 11.0);
            Assert.AreEqual(wickos[2].HighRate, 12.4);
            Assert.AreEqual(wickos[2].LowRate, 10.0);
            Assert.AreEqual(wickos[2].CloseRate, 10.0);
            Assert.AreEqual(wickos[2].Trend, Trend.Falling);
            Assert.AreEqual(wickos[2].Spread, 1.0);

            var openWicko = feed.OpenWicko;

            Assert.AreEqual(openWicko.OpenRate, 10.0);
            Assert.AreEqual(openWicko.HighRate, 10.0);
            Assert.AreEqual(openWicko.LowRate, 9.9);
            Assert.AreEqual(openWicko.CloseRate, 9.9);
        }

        [TestMethod()]
        public void ThreeRisingGapWickos()
        {
            var feed = new WickoFeed(
                Symbol.EURUSD, 10000.0, RateToUse.BidRate);

            var wickos = new List<Wicko>();

            feed.OnWicko += (s, e) => wickos.Add(e.Wicko);

            var tickOn1 = new DateTime(2016, 1, 1, 17, 0, 0, 0);
            var tickOn2 = new DateTime(2016, 1, 1, 17, 0, 0, 1);

            feed.HandleTick(new Tick { TickOn = tickOn1, BidRate = 10.0 });
            feed.HandleTick(new Tick { TickOn = tickOn2, BidRate = 14.0 });

            Assert.AreEqual(wickos.Count, 3);

            Assert.AreEqual(wickos[0].OpenOn, tickOn1);
            Assert.AreEqual(wickos[0].CloseOn, tickOn2);
            Assert.AreEqual(wickos[0].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[0].OpenRate, 10.0);
            Assert.AreEqual(wickos[0].HighRate, 11.0);
            Assert.AreEqual(wickos[0].LowRate, 10.0);
            Assert.AreEqual(wickos[0].CloseRate, 11.0);
            Assert.AreEqual(wickos[0].Trend, Trend.Rising);
            Assert.AreEqual(wickos[0].Spread, 1.0);

            Assert.AreEqual(wickos[1].OpenOn, tickOn2);
            Assert.AreEqual(wickos[1].CloseOn, tickOn2);
            Assert.AreEqual(wickos[1].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[1].OpenRate, 11.0);
            Assert.AreEqual(wickos[1].HighRate, 12.0);
            Assert.AreEqual(wickos[1].LowRate, 11.0);
            Assert.AreEqual(wickos[1].CloseRate, 12.0);
            Assert.AreEqual(wickos[1].Trend, Trend.Rising);
            Assert.AreEqual(wickos[1].Spread, 1.0);

            Assert.AreEqual(wickos[2].OpenOn, tickOn2);
            Assert.AreEqual(wickos[2].CloseOn, tickOn2);
            Assert.AreEqual(wickos[2].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[2].OpenRate, 12.0);
            Assert.AreEqual(wickos[2].HighRate, 13.0);
            Assert.AreEqual(wickos[2].LowRate, 12.0);
            Assert.AreEqual(wickos[2].CloseRate, 13.0);
            Assert.AreEqual(wickos[2].Trend, Trend.Rising);
            Assert.AreEqual(wickos[2].Spread, 1.0);

            var openWicko = feed.OpenWicko;

            Assert.AreEqual(openWicko.OpenOn, tickOn2);
            Assert.AreEqual(openWicko.CloseOn, tickOn2);
            Assert.AreEqual(openWicko.OpenRate, 13.0);
            Assert.AreEqual(openWicko.HighRate, 14.0);
            Assert.AreEqual(openWicko.LowRate, 13.0);
            Assert.AreEqual(openWicko.CloseRate, 14.0);
        }

        [TestMethod()]
        public void ThreeFallingGapWickos()
        {
            var feed = new WickoFeed(
                Symbol.EURUSD, 10000.0, RateToUse.BidRate);

            var wickos = new List<Wicko>();

            feed.OnWicko += (s, e) => wickos.Add(e.Wicko);

            var tickOn1 = new DateTime(2016, 1, 1, 17, 0, 0, 0);
            var tickOn2 = new DateTime(2016, 1, 1, 17, 0, 0, 1);

            feed.HandleTick(new Tick { TickOn = tickOn1, BidRate = 14.0 });
            feed.HandleTick(new Tick { TickOn = tickOn2, BidRate = 10.0 });

            Assert.AreEqual(wickos.Count, 3);

            Assert.AreEqual(wickos[0].OpenOn, tickOn1);
            Assert.AreEqual(wickos[0].CloseOn, tickOn2);
            Assert.AreEqual(wickos[0].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[0].OpenRate, 14.0);
            Assert.AreEqual(wickos[0].HighRate, 14.0);
            Assert.AreEqual(wickos[0].LowRate, 13.0);
            Assert.AreEqual(wickos[0].CloseRate, 13.0);
            Assert.AreEqual(wickos[0].Trend, Trend.Falling);
            Assert.AreEqual(wickos[0].Spread, 1.0);

            Assert.AreEqual(wickos[1].OpenOn, tickOn2);
            Assert.AreEqual(wickos[1].CloseOn, tickOn2);
            Assert.AreEqual(wickos[1].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[1].OpenRate, 13.0);
            Assert.AreEqual(wickos[1].HighRate, 13.0);
            Assert.AreEqual(wickos[1].LowRate, 12.0);
            Assert.AreEqual(wickos[1].CloseRate, 12.0);
            Assert.AreEqual(wickos[1].Trend, Trend.Falling);
            Assert.AreEqual(wickos[1].Spread, 1.0);

            Assert.AreEqual(wickos[2].OpenOn, tickOn2);
            Assert.AreEqual(wickos[2].CloseOn, tickOn2);
            Assert.AreEqual(wickos[2].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[2].OpenRate, 12.0);
            Assert.AreEqual(wickos[2].HighRate, 12.0);
            Assert.AreEqual(wickos[2].LowRate, 11.0);
            Assert.AreEqual(wickos[2].CloseRate, 11.0);
            Assert.AreEqual(wickos[2].Trend, Trend.Falling);
            Assert.AreEqual(wickos[2].Spread, 1.0);

            var openWicko = feed.OpenWicko;

            Assert.AreEqual(openWicko.OpenRate, 11.0);
            Assert.AreEqual(openWicko.HighRate, 11.0);
            Assert.AreEqual(openWicko.LowRate, 10.0);
            Assert.AreEqual(openWicko.CloseRate, 10.0);
        }

        [TestMethod()]
        public void TwoFallingThenThreeRisingGapWickos()
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
            feed.HandleTick(new Tick { BidRate = 12.1 });

            Assert.AreEqual(wickos.Count, 5);

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

            Assert.AreEqual(wickos[3].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[3].OpenRate, 10.0);
            Assert.AreEqual(wickos[3].HighRate, 11.0);
            Assert.AreEqual(wickos[3].LowRate, 10.0);
            Assert.AreEqual(wickos[3].CloseRate, 11.0);
            Assert.AreEqual(wickos[3].Trend, Trend.Rising);
            Assert.AreEqual(wickos[3].Spread, 1.0);

            Assert.AreEqual(wickos[4].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[4].OpenRate, 11.0);
            Assert.AreEqual(wickos[4].HighRate, 12.0);
            Assert.AreEqual(wickos[4].LowRate, 11.0);
            Assert.AreEqual(wickos[4].CloseRate, 12.0);
            Assert.AreEqual(wickos[4].Trend, Trend.Rising);
            Assert.AreEqual(wickos[4].Spread, 1.0);

            var openWicko = feed.OpenWicko;

            Assert.AreEqual(openWicko.OpenRate, 12.0);
            Assert.AreEqual(openWicko.HighRate, 12.1);
            Assert.AreEqual(openWicko.LowRate, 12.0);
            Assert.AreEqual(openWicko.CloseRate, 12.1);
        }

        [TestMethod()]
        public void TwoRisingThenThreeFallingGapWickos()
        {
            var feed = new WickoFeed(
                Symbol.EURUSD, 10000.0, RateToUse.BidRate);

            var wickos = new List<Wicko>();

            feed.OnWicko += (s, e) => wickos.Add(e.Wicko);

            feed.HandleTick(new Tick { BidRate = 10.0 });
            feed.HandleTick(new Tick { BidRate = 9.6 });
            feed.HandleTick(new Tick { BidRate = 10.5 });
            feed.HandleTick(new Tick { BidRate = 11.1 });

            feed.HandleTick(new Tick { BidRate = 11.0 });
            feed.HandleTick(new Tick { BidRate = 10.7 });
            feed.HandleTick(new Tick { BidRate = 11.6 });
            feed.HandleTick(new Tick { BidRate = 12.3 });

            feed.HandleTick(new Tick { BidRate = 12.3 });
            feed.HandleTick(new Tick { BidRate = 12.4 });
            feed.HandleTick(new Tick { BidRate = 11.5 });
            feed.HandleTick(new Tick { BidRate = 7.9 });

            Assert.AreEqual(wickos.Count, 5);

            Assert.AreEqual(wickos[0].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[0].OpenRate, 10.0);
            Assert.AreEqual(wickos[0].HighRate, 11.0);
            Assert.AreEqual(wickos[0].LowRate, 9.6);
            Assert.AreEqual(wickos[0].CloseRate, 11.0);
            Assert.AreEqual(wickos[0].Trend, Trend.Rising);
            Assert.AreEqual(wickos[0].Spread, 1.0);

            Assert.AreEqual(wickos[1].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[1].OpenRate, 11.0);
            Assert.AreEqual(wickos[1].HighRate, 12.0);
            Assert.AreEqual(wickos[1].LowRate, 10.7);
            Assert.AreEqual(wickos[1].CloseRate, 12.0);
            Assert.AreEqual(wickos[1].Trend, Trend.Rising);
            Assert.AreEqual(wickos[1].Spread, 1.0);

            Assert.AreEqual(wickos[2].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[2].OpenRate, 11.0);
            Assert.AreEqual(wickos[2].HighRate, 12.4);
            Assert.AreEqual(wickos[2].LowRate, 10.0);
            Assert.AreEqual(wickos[2].CloseRate, 10.0);
            Assert.AreEqual(wickos[2].Trend, Trend.Falling);
            Assert.AreEqual(wickos[2].Spread, 1.0);

            Assert.AreEqual(wickos[3].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[3].OpenRate, 10.0);
            Assert.AreEqual(wickos[3].HighRate, 10.0);
            Assert.AreEqual(wickos[3].LowRate, 9.0);
            Assert.AreEqual(wickos[3].CloseRate, 9.0);
            Assert.AreEqual(wickos[3].Trend, Trend.Falling);
            Assert.AreEqual(wickos[3].Spread, 1.0);

            Assert.AreEqual(wickos[4].Symbol, Symbol.EURUSD);
            Assert.AreEqual(wickos[4].OpenRate, 9.0);
            Assert.AreEqual(wickos[4].HighRate, 9.0);
            Assert.AreEqual(wickos[4].LowRate, 8.0);
            Assert.AreEqual(wickos[4].CloseRate, 8.0);
            Assert.AreEqual(wickos[4].Trend, Trend.Falling);
            Assert.AreEqual(wickos[4].Spread, 1.0);

            var openWicko = feed.OpenWicko;

            Assert.AreEqual(openWicko.OpenRate, 8.0);
            Assert.AreEqual(openWicko.HighRate, 8.0);
            Assert.AreEqual(openWicko.LowRate, 7.9);
            Assert.AreEqual(openWicko.CloseRate, 7.9);
        }
    }
}