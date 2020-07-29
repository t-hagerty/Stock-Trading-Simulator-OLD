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

        //https://iexcloud.io/docs/api/#quote
        public async Task<StockCandlestick> GetStockData(string tickerSymbol)
        {
            StockCandlestick stockCandlestick = null;
            QuoteCandleStickWrapper wrapperCandlestick;
            HttpResponseMessage response = client.GetAsync("stock/" + tickerSymbol + "/quote" + TOKEN).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                wrapperCandlestick = await response.Content.ReadAsAsync<QuoteCandleStickWrapper>();
                stockCandlestick = new StockCandlestick();
                stockCandlestick.ID = -1;
                stockCandlestick.CompanyId = -1;
                stockCandlestick.Company = null;
                stockCandlestick.Timestamp = DateTime.Parse(wrapperCandlestick.latestTime);
                stockCandlestick.Open = wrapperCandlestick.open;
                stockCandlestick.High = wrapperCandlestick.high;
                stockCandlestick.Low = wrapperCandlestick.low;
                stockCandlestick.Close = wrapperCandlestick.close;
                stockCandlestick.Volume = wrapperCandlestick.volume;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return stockCandlestick;
        }

        //https://iexcloud.io/docs/api/#historical-prices
        public async Task<IEnumerable<StockCandlestick>> GetStockDataRange(string tickerSymbol, string range)
        {
            //Avoid calling the API if range is not one of the accepted inputs:
            if(range != "5y" && range != "2y" && range != "1y" && range != "6m" && range != "3m" && range != "1m" && range != "5d")
            {
                return null;
            }

            List<StockCandlestick> stockCandlesticks = null;
            List<DailyRangeCandlestickWrapper> wrapperCandlesticks;
            HttpResponseMessage response = client.GetAsync("stock/" + tickerSymbol + "/chart/" + range + TOKEN).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                wrapperCandlesticks = await response.Content.ReadAsAsync<List<DailyRangeCandlestickWrapper>>();
                stockCandlesticks = wrapperCandlesticks.ConvertAll<StockCandlestick>(x => new StockCandlestick()
                {
                    ID = -1,
                    CompanyId = -1,
                    Company = null,
                    Timestamp = DateTime.Parse(x.date + " 16:00"),
                    Open = x.open,
                    High = x.high,
                    Low = x.low,
                    Close = x.close,
                    Volume = x.volume
                });
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return stockCandlesticks;
        }

        //https://iexcloud.io/docs/api/#intraday-prices
        public async Task<IEnumerable<StockCandlestick>> GetStockDataDayMinutes(string tickerSymbol)
        {
            List<StockCandlestick> stockCandlesticks = null;
            List<DailyMinutesCandlestickWrapper> wrapperCandlesticks;
            HttpResponseMessage response = client.GetAsync("stock/" + tickerSymbol + "/intraday-prices" + TOKEN + "&chartIEXWhenNull=true").GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                wrapperCandlesticks = await response.Content.ReadAsAsync<List<DailyMinutesCandlestickWrapper>>();
                wrapperCandlesticks.RemoveAll(w => w.high == null);
                stockCandlesticks = wrapperCandlesticks.ConvertAll<StockCandlestick>(x => new StockCandlestick()
                {
                    ID = -1,
                    CompanyId = -1,
                    Company = null,
                    Timestamp = DateTime.Parse(x.date + " " + x.minute),
                    Open = x.open.Value,
                    High = x.high.Value,
                    Low = x.low.Value,
                    Close = x.close.Value,
                    Volume = x.volume
                });
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return stockCandlesticks;
        }

        //https://iexcloud.io/docs/api/#historical-prices
        public async Task<StockCandlestick> GetStockDataDate(string tickerSymbol, DateTime date)
        {
            List<StockCandlestick> stockCandlesticks = null;
            HttpResponseMessage response = client.GetAsync("stock/" + tickerSymbol + "/chart/date/" + date.ToString("yyyyMMdd") + TOKEN + "&chartByDay=true").GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                stockCandlesticks = await response.Content.ReadAsAsync<List<StockCandlestick>>();
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            try
            {
                return stockCandlesticks.First();
            }
            catch(InvalidOperationException e)
            {
                return null;
            }
        }
    }

    public class DailyMinutesCandlestickWrapper
    {
        public string date { get; set; }
        public string minute { get; set; }
        public string label { get; set; }
        public decimal? high { get; set; }
        public decimal? low { get; set; }
        public decimal? open { get; set; }
        public decimal? close { get; set; }
        public double? average { get; set; }
        public int volume { get; set; }
        public double notional { get; set; }
        public int numberOfTrades { get; set; }
    }

    public class DailyRangeCandlestickWrapper
    {
        public string date { get; set; }
        public decimal open { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal close { get; set; }
        public int volume { get; set; }
        public decimal uOpen { get; set; }
        public decimal uHigh { get; set; }
        public decimal uLow { get; set; }
        public decimal uClose { get; set; }
        public int uVolume { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
        public string label { get; set; }
        public double changeOverTime { get; set; }
    }

    public class QuoteCandleStickWrapper
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string calculationPrice { get; set; }
        public decimal open { get; set; }
        public UInt64 openTime { get; set; }
        public decimal close { get; set; }
        public UInt64 closeTime { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal latestPrice { get; set; }
        public string latestSource { get; set; }
        public string latestTime { get; set; }
        public UInt64 latestUpdate { get; set; }
        public int latestVolume { get; set; }
        public int volume { get; set; }
        public decimal iexRealtimePrice { get; set; }
        public int iexRealtimeSize { get; set; }
        public UInt64 iexLastUpdated { get; set; }
        public decimal delayedPrice { get; set; }
        public UInt64 delayedPriceTime { get; set; }
        public decimal oddLotDelayedPrice { get; set; }
        public UInt64 oddLotDelayedPriceTime { get; set; }
        public decimal extendedPrice { get; set; }
        public decimal extendedChange { get; set; }
        public decimal extendedChangePercent { get; set; }
        public UInt64 extendedPriceTime { get; set; }
        public decimal previousClose { get; set; }
        public int previousVolume { get; set; }
        public decimal change { get; set; }
        public decimal changePercent { get; set; }
        public decimal iexMarketPercent { get; set; }
        public int iexVolume { get; set; }
        public int avgTotalVolume { get; set; }
        public decimal iexBidPrice { get; set; }
        public int iexBidSize { get; set; }
        public decimal iexAskPrice { get; set; }
        public UInt64 marketCap { get; set; }
        public decimal week52High { get; set; }
        public decimal week52Low { get; set; }
        public decimal ytdChange { get; set; }
        public decimal peRatio { get; set; }
        public UInt64 lastTradeTime { get; set; }
        public bool isUsMarketOpen { get; set; }
    }
}