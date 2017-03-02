using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;

namespace PMWeb.Models
{
    public class ValuationViewModel
    {
        public PMApi.Models.ValuationSummary ValuationSummary { get; set; }

        public Highcharts Chart { get; set; }

        public string PercentageChange { get; set; }

        public string LastChange { get; set; }

        public string PortfolioValue { get; set; }

        public string IntradayChange { get; set; }
        public string IntradayLow { get; set; }
        public string IntradayHigh { get; set; }
    }
}