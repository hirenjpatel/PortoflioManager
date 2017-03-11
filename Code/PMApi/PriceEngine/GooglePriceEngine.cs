using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMApi.Models;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;

namespace PMApi.PriceEngine
{
    public class GooglePriceEngine : IPriceEngine
    {
        const string base_url =
               "http://finance.google.com/finance/info?client=ig&q=";
        public List<PriceValue> GetPrices(string[] symbols)
        {
            List<PriceValue> prices = new List<PriceValue>();
            string json;
            string joinedSymbols = string.Join(",", symbols);

            using (var web = new WebClient())
            {
                //var url = $"http://finance.google.com/finance/info?client=ig&amp;amp;q=NASDAQ%3A{tickers}";
                var url = $"http://finance.google.com/finance/info?client=ig&q={joinedSymbols}";
                json = web.DownloadString(url);
            }

            //Google adds a comment before the json for some unknown reason, so we need to remove it
            json = json.Replace("//", "");

            var v = JArray.Parse(json);

            foreach (var i in v)
            {
                PriceValue pricevalue = new PriceValue();
                pricevalue.Symbol = (string)i.SelectToken("t");
                pricevalue.Price = (decimal)i.SelectToken("l");
                pricevalue.PriceDate = (DateTime)i.SelectToken("lt_dts");
                pricevalue.IntradayChange = (decimal)i.SelectToken("c");

                //set the creation date
                pricevalue.CreationDate = DateTime.Now;
                pricevalue.CreationName = "SYSTEM";

                prices.Add(pricevalue);
            }

            return prices;
        }
    }
}