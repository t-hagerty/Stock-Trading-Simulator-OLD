using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace StockSimulator.Models
{
    public class IEX_DataSource
    {
        /*  BASE_URL:
         *  Production API: https://cloud.iexapis.com/ 
         *  Testing Sandbox: https://sandbox.iexapis.com/ 
         */
        private const string BASE_URL = "https://cloud.iexapis.com/";
        private const string VERSION = "stable";
        private const string TOKEN = "?token=pk_e9c5e9cb317443b08fbc2bfd102efc2b";
        HttpClient client;

        public IEX_DataSource()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(BASE_URL + VERSION + "/")
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<StockCandlestick> GetStockData(string tickerSymbol)
        {
            StockCandlestick stockCandlestick = null;
            HttpResponseMessage response = client.GetAsync("stock/" + tickerSymbol + "/quote" + TOKEN).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                stockCandlestick = await response.Content.ReadAsAsync<StockCandlestick>();
                Console.WriteLine(stockCandlestick.ToString());
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return stockCandlestick;
        }

        public IEnumerable<StockCandlestick> GetStockDataRange(string tickerSymbol)
        {
            throw new NotImplementedException();
        }
    }
}