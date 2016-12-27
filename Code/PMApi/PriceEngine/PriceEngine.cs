using PMApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace PMApi.PriceEngine
{
    public class PriceEngine
    {
        const string base_url =
               "http://download.finance.yahoo.com/d/quotes.csv?s=@&f=sl1d1t1c1";

        public List<PriceValue> GetRealTimePrices(string[] symbols)
        {

            List<PriceValue> prices = new List<PriceValue>();
            string joinedSymbols = string.Join("+", symbols);
            string url = "";


            // Remove the trailing plus sign.
            
            url = joinedSymbols.Substring(0, joinedSymbols.Length - 1);

            // Prepend the base URL.

            url = base_url.Replace("@", url);

            // Get the response.
            try
            {
                // Get the web response.
                string result = GetWebResponse(url);
                Console.WriteLine(result.Replace("\\r\\n", "\r\n"));

                // Pull out the current prices.
                string[] lines = result.Split(
                    new char[] { '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach(string line in lines)
                {
                    PriceValue pricevalue = new PriceValue();
                    pricevalue.Symbol = line.Split(',')[0];
                    pricevalue.Price = Convert.ToDecimal(line.Split(',')[1]);

                    prices.Add(pricevalue);
                }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return prices;

        }
        private string GetWebResponse(string url)
        {
            // Make a WebClient.
            WebClient web_client = new WebClient();

            // Get the indicated URL.
            Stream response = web_client.OpenRead(url);

            // Read the result.
            using (StreamReader stream_reader = new StreamReader(response))
            {
                // Get the results.
                string result = stream_reader.ReadToEnd();

                // Close the stream reader and its underlying stream.
                stream_reader.Close();

                // Return the result.
                return result;
            }
        }
    }
}