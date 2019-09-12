using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace StockSimulator.Models
{
    /*
     * Models the price range of a stock at a given point in time https://www.investopedia.com/trading/candlestick-charting-what-is-it/
     */
    public class StockCandlestick
    {
        public int ID { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public int Volume { get; set; }
    }
}