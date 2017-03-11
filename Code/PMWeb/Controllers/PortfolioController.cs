using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Newtonsoft.Json;
using PMApi.Models;
using PMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace PMWeb.Controllers
{
    public class PortfolioController : Controller
    {
        HttpClient client = new HttpClient();
        string url = null;
        public PortfolioController()
        {
            client = new HttpClient();
            url = "http://portfoliomanagerservice.azurewebsites.net/api/";//WebConfigurationManager.AppSettings["PMApiUrl"];
            client.BaseAddress = new Uri(url);

            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Portfolio
        public ActionResult Index()
        {
            return View();
        }

        [Route("portfolio/valuation/{portfolioId}/{date}")]
        public async Task<ActionResult> Valuation(int portfolioId, DateTime date)
        {
            DateTime begTime = date;
            DateTime endTime = date.AddDays(1);
            string tempurl = url + $"valuations/valuationsummary/1000/{portfolioId}/{begTime.ToString("yyyy-MM-dd")}/{endTime.ToString("yyyy-MM-dd")}";
            HttpResponseMessage responseMessage = await client.GetAsync(tempurl);

            if (responseMessage.IsSuccessStatusCode)

            {

                var responseData = responseMessage.Content.ReadAsStringAsync().Result;


                var valuationSummary = JsonConvert.DeserializeObject<PMApi.Models.ValuationSummary>(responseData);


                string xaxis = string.Join(",", valuationSummary.IntraDayChangeValuationDetailSummary.Keys.ToArray().Select(x => x.ToString()));

                string[] xaxisArray = xaxis.Split(',').Select(str => str.Trim()).ToArray();
                string yaxis = string.Join(",", valuationSummary.IntraDayChangeValuationDetailSummary.Values.ToArray().Select(x => x.ToString()));
                string[] yaxisArray = yaxis.Split(',').Select(str => str.Trim()).ToArray();

                var intraDayValues = valuationSummary.IntraDayChangeValuationDetailSummary.Select(i => new object[] { i.Value }).ToArray();

                Highcharts chart = new Highcharts("chart")
                    
         .SetCredits(new Credits { Enabled = false })
         .InitChart(new Chart { DefaultSeriesType = ChartTypes.Line, Height = 500, Width = 800     })
         .SetTitle(new Title { Text = "Portfolio Intraday" })
         .SetXAxis(new XAxis { Categories = xaxisArray, Labels = new XAxisLabels() { } })
         .SetYAxis(new YAxis
         {
             Title = new YAxisTitle { Text = "Intraday Change" }
         })
         .SetTooltip(new Tooltip { Formatter = "function() { return ''+  this.series.name +': '+ this.y +''; }" })
         .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { Color = System.Drawing.Color.Black }, Line = new PlotOptionsLine { Marker = new PlotOptionsLineMarker { Enabled = false } } })
         .SetSeries(new[]
                 {
               new Series { Name = "Intraday Change", Data = new Data(intraDayValues) }
                  });
          
            return View(chart);

               // return View(valuationSummary);
            }
            else
                return View();
        }

        [Route("portfolio/intraday/{portfolioId}/{date}")]
        public async Task<ActionResult> Intraday(int portfolioId, DateTime? date = null)
        {
            DateTime validDate = new DateTime();
            //if date is not specifiied set it to today's date
            validDate = date ?? DateTime.Now;
            DateTime begTime = validDate;
            DateTime endTime = validDate.AddDays(1);
            string tempurl = url + $"valuations/intraday/1000/{portfolioId}/{begTime.ToString("yyyy-MM-dd")}/{endTime.ToString("yyyy-MM-dd")}";
            HttpResponseMessage responseMessage = await client.GetAsync(tempurl);

            if (responseMessage.IsSuccessStatusCode)

            {

                var responseData = responseMessage.Content.ReadAsStringAsync().Result;


                var valuationSummary = JsonConvert.DeserializeObject<PMApi.Models.ValuationSummary>(responseData);


                string xaxis = string.Join(",", valuationSummary.IntraDayChangeValuationDetailSummary.Keys.ToArray().Select(x => x.ToString()));

                string[] xaxisArray = xaxis.Split(',').Select(str => str.Trim()).ToArray();
                string yaxis = string.Join(",", valuationSummary.IntraDayChangeValuationDetailSummary.Values.ToArray().Select(x => x.ToString()));
                string[] yaxisArray = yaxis.Split(',').Select(str => str.Trim()).ToArray();

                var intraDayValues = valuationSummary.IntraDayChangeValuationDetailSummary.Select(i => new object[] { i.Value }).ToArray();

                Highcharts chart = new Highcharts("chart")

         .SetCredits(new Credits { Enabled = false })
         .InitChart(new Chart { DefaultSeriesType = ChartTypes.Line, Height = 500, Width = 800 })
         .SetTitle(new Title { Text = "Portfolio Intraday" })
         .SetXAxis(new XAxis { Categories = xaxisArray, Labels = new XAxisLabels() { } })
         .SetYAxis(new YAxis
         {
             Title = new YAxisTitle { Text = "Intraday Change" }
         })
         .SetTooltip(new Tooltip { Formatter = "function() { return ''+  this.series.name +': '+ this.y +''; }" })
         .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { Color = System.Drawing.Color.Black }, Line = new PlotOptionsLine { Marker = new PlotOptionsLineMarker { Enabled = false } } })
         .SetSeries(new[]
                 {
               new Series { Name = "Intraday Change", Data = new Data(intraDayValues) }
                  });


                var model = GenerateValuationViewModel(valuationSummary, chart);
                return View(model);

                // return View(valuationSummary);
            }
            else
                return View();
        }

        public ValuationViewModel GenerateValuationViewModel(ValuationSummary summary, Highcharts chart)
        {
            ValuationViewModel model = new ValuationViewModel();

            //set the valuation summary
            model.ValuationSummary = summary;
            model.Chart = chart;
            //set the intraday change by getting the last value from the dictionary
            model.IntradayChange = Math.Round(summary.IntraDayChangeValuationDetailSummary.Values.Last(),2).ToString();
            model.PercentageChange = Math.Round(((summary.IntraDayChangeValuationDetailSummary.Values.Last() / summary.ValuationDetailSummary.Values.Last()) * 100),2).ToString();
            model.PortfolioValue = summary.ValuationDetailSummary.Values.Last().ToString();
            model.IntradayHigh = Math.Round(summary.IntraDayChangeValuationDetailSummary.Values.Max(),2).ToString();
            model.IntradayLow = Math.Round(summary.IntraDayChangeValuationDetailSummary.Values.Min(),2).ToString();

            return model;

        }

        
    }

}