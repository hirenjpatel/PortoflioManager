using PMApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        //	s: Symbol 
        //  l1: Last Trade (Price Only)
        //  d1: Last Trade Date
        //  t1: Last Trade Time
        //  c1: Change

        public static List<PriceValue> GetRealTimePrices(string[] symbols)
         {

            List<PriceValue> prices = new List<PriceValue>();
            string joinedSymbols = string.Join("+", symbols);
            string url = "";


            // Remove the trailing plus sign.

            url = joinedSymbols;

            // Prepend the base URL.

            url = base_url.Replace("@", url);

            // Get the response.
            try
            {
                // Get the web response.
                string result = GetWebResponse(url);
             

                // Pull out the current prices.
                string[] lines = result.Split(
                    new char[] { '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach(string line in lines)
                {
                    PriceValue pricevalue = new PriceValue();
                    char[] charsToTrim = { '*','\"', '"' };
                    //set the stock symbol
                    pricevalue.Symbol = line.Split(',')[0].Trim(charsToTrim) ;
                    pricevalue.Price = Convert.ToDecimal(line.Split(',')[1]);
                    //compile the date time by combining the Date and the time
                    string date = line.Split(',')[2].Trim(charsToTrim);
                    string time = line.Split(',')[3].Trim(charsToTrim);
                    DateTime dt = Convert.ToDateTime(date + " " + time);
                    //convert the date time to CST from EST
                    DateTime priceDateTime = dt.Subtract(TimeSpan.FromHours(1));
                    //set the pricedate
                    pricevalue.PriceDate = priceDateTime;
                    //set the creation date
                    pricevalue.CreationDate = DateTime.Now;
                    pricevalue.CreationName = "SYSTEM";




                    prices.Add(pricevalue);
                }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return prices;

        }
        /// <summary>
        /// Call the url and get the web response
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetWebResponse(string url)
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