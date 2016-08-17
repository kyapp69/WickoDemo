// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 8/17/2016

using SciChart.Charting.Model.DataSeries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using WickoDemo.Shared;

namespace WickoDemo.Viewer
{
    public class ViewModel : INotifyPropertyChanged
    {
        public class Metadata : IPointMetadata
        {
            private DateTime openOn;

            public event PropertyChangedEventHandler PropertyChanged;

            public Metadata(DateTime openOn)
            {
                OpenOn = openOn;
            }

            public bool IsSelected { get; set; }

            public DateTime OpenOn
            {
                get
                {
                    return openOn;
                }
                set
                {
                    openOn = value;

                    PropertyChanged?.Invoke(this,
                        new PropertyChangedEventArgs(nameof(OpenOn)));
                }
            }
        }

        private Symbol symbol;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            Symbols = Enum.GetValues(typeof(Symbol)).Cast<Symbol>().ToList();

            Symbol = Symbol.EURUSD;
        }

        public IOhlcDataSeries<DateTime, double> Series { get; } =
            new OhlcDataSeries<DateTime, double>();

        public string TickFormat { get; set; } = "0.00000";

        public List<Symbol> Symbols { get; }

        public Symbol Symbol
        {
            get
            {
                return symbol;
            }
            set
            {
                symbol = value;

                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(Symbol)));

                LoadWickos(Symbol);
            }
        }

        private void LoadWickos(Symbol symbol)
        {
            Series.Clear();

            var feed = new WickoFeed(symbol, 1.0, RateToUse.BidRate);

            var wickos = new List<Wicko>();

            feed.OnWicko += (s, e) => wickos.Add(e.Wicko);

            foreach (var tick in GetTicks(symbol))
                feed.HandleTick(tick);

            foreach (var wicko in wickos)
            {
                Series.Append(wicko.CloseOn, wicko.OpenRate, 
                    wicko.HighRate, wicko.LowRate, wicko.CloseRate, 
                    new Metadata(wicko.OpenOn));
            }

            Series.InvalidateParentSurface(RangeMode.ZoomToFit);
        }

        private List<Tick> GetTicks(Symbol symbol)
        {
            var ticks = new List<Tick>();

            string data;

            switch (symbol)
            {
                case Symbol.AUDCAD:
                    data = Properties.Resources.AUDUSD;
                    break;
                case Symbol.EURUSD:
                    data = Properties.Resources.EURUSD;
                    break;
                case Symbol.GBPUSD:
                    data = Properties.Resources.GBPUSD;
                    break;
                case Symbol.USDCAD:
                    data = Properties.Resources.USDCAD;
                    break;
                case Symbol.USDCHF:
                    data = Properties.Resources.USDCHF;
                    break;
                case Symbol.USDJPY:
                    data = Properties.Resources.USDJPY;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol));
            }

            using (var reader = new StringReader(data))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(',');

                    ticks.Add(new Tick()
                    {
                        Symbol = symbol,
                        TickOn = DateTime.ParseExact(fields[1],
                            "MM/dd/yyyy HH:mm:ss.fff", null),
                        BidRate = double.Parse(fields[2]),
                        AskRate = double.Parse(fields[3])
                    });
                }
            }

            return ticks;
        }
    }
}