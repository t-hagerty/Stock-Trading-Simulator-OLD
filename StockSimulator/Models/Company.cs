using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace StockSimulator.Models
{
    public class Company
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string TickerSymbol { get; set; }
        public DateTime StartDataDate { get; set; }

        public virtual ICollection<StockCandlestick> StockCandlesticks { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Stock> Transactions { get; set; }
    }
}