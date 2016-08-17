// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 8/17/2016

using System;

namespace WickoDemo.Shared
{
    public static class MiscExtenders
    {
        public static double ToRoundedRate(this double value, Symbol symbol) =>
            Math.Round(value, symbol == Symbol.USDJPY ? 3 : 5);

        public static double PipsToRate(this double value, Symbol symbol)
        {
            var factor = (symbol == Symbol.USDJPY ? 100.0 : 10000.0);

            return (value / factor).ToRoundedRate(symbol);
        }

        //public static double ToTickSize(this Symbol symbol) =>
        //    symbol == Symbol.USDJPY ? 0.001 : 0.00001;

        //public static string ToDateString(this DateTime value) =>
        //    value.ToString("MM/dd/yyyy");

        //public static string ToTimeString(this DateTime value) =>
        //    value.ToString("HH:mm:ss.fff");

        //public static string ToDateTimeString(this DateTime value) =>
        //    value.ToString("MM/dd/yyyy HH:mm:ss.fff");

        //public static string ToRateString(this double value, Symbol symbol) =>
        //    value.ToString((symbol == Symbol.USDJPY) ? "N3" : "N5");
    }
}
