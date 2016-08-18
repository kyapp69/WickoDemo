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
    }
}
